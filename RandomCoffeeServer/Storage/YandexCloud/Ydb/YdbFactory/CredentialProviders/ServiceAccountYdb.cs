using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Yandex.Cloud.Generated;
using Yandex.Cloud.Iam.V1;
using Ydb.Sdk.Auth;
using Ydb.Sdk.Yc;

namespace RandomCoffeeServer.Storage.YandexCloud.Ydb.YdbFactory;

// todo exists for creating Jwt with "nbf" that is more merciful to devs with wrong system clock time
// https://github.com/ydb-platform/ydb-dotnet-yc/blob/main/src/Ydb.Sdk.Yc.Auth/src/ServiceAccount.cs  
public class ServiceAccountProvider : IamProviderBase
{
    private static readonly TimeSpan JwtTtl = TimeSpan.FromHours(1);
    private readonly ILogger _logger;
    private readonly string _serviceAccountId;
    private readonly RsaSecurityKey _privateKey;

    public ServiceAccountProvider(string saFilePath, ILoggerFactory? loggerFactory = null)
        : base(loggerFactory)
    {
        loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
        _logger = loggerFactory.CreateLogger<ServiceAccountProvider>();

        var saInfo = JsonSerializer.Deserialize<SaJsonInfo>(File.ReadAllText(saFilePath));
        if (saInfo == null)
        {
            throw new InvalidCredentialsException("Failed to parse service account file.");
        }
        saInfo.EnsureValid();

        _logger.LogDebug("Successfully parsed service account file.");

        _serviceAccountId = saInfo.service_account_id;

        using (var reader = new StringReader(saInfo.private_key))
        {
            var parameters = new PemReader(reader).ReadObject() as RsaPrivateCrtKeyParameters;
            if (parameters == null)
            {
                throw new InvalidCredentialsException("Failed to parse service account key.");
            }

            RSAParameters rsaParams = DotNetUtilities.ToRSAParameters(parameters);
            _privateKey = new RsaSecurityKey(rsaParams) { KeyId = saInfo.id };
        }

        _logger.LogInformation("Successfully parsed service account key.");
    }

    protected override async Task<IamTokenData> FetchToken()
    {
        _logger.LogInformation("Fetching IAM token by service account key.");

        var services = new Services(new Yandex.Cloud.Sdk(new EmptyYcCredentialsProvider()));

        var request = new CreateIamTokenRequest
        {
            Jwt = MakeJwt()
        };

        var response = await services.Iam.IamTokenService.CreateAsync(request);

        var iamToken = new IamTokenData(
            token: response.IamToken,
            expiresAt: response.ExpiresAt.ToDateTime()
        );

        return iamToken;
    }

    private string MakeJwt()
    {
        var handler = new JsonWebTokenHandler();
        var now = DateTime.UtcNow;

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _serviceAccountId,
            Audience = YcAuth.DefaultAudience,
            IssuedAt = now,
            NotBefore = now.Subtract(TimeSpan.FromMinutes(1)), // (s.lugovykh) added implicit NotBefore 
            Expires = now.Add(JwtTtl),
            SigningCredentials = new SigningCredentials(_privateKey, SecurityAlgorithms.RsaSsaPssSha256),
        };

        return handler.CreateToken(descriptor);
    }

    private class SaJsonInfo
    {
        public string id { get; set; } = "";
        public string service_account_id { get; set; } = "";
        public string private_key { get; set; } = "";

        public void EnsureValid()
        {
            if (string.IsNullOrEmpty(id) ||
                string.IsNullOrEmpty(service_account_id) ||
                string.IsNullOrEmpty(private_key))
            {
                throw new InvalidCredentialsException("Invalid service account file.");
            }
        }
    }
}