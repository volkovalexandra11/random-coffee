using Yandex.Cloud;
using Yandex.Cloud.Credentials;
using Yandex.Cloud.Iam.V1;

namespace RandomCoffeeServer.Services.YandexCloud.Lockbox;

// idk what it is, prob doesnt work
// see https://github.com/yandex-cloud/dotnet-sdk/blob/master/Yandex.Cloud.SDK/credentials/OAuthCredentialsProvider.cs
public class OAuthCredentialsForSaProvider : ICredentialsProvider
{
    private readonly IamTokenService.IamTokenServiceClient tokenService;
    private readonly string saId;
    private CreateIamTokenResponse? iamToken;

    public OAuthCredentialsForSaProvider(string saId, string oauthToken)
    {
        var developerSdk = new Sdk(new OAuthCredentialsProvider(oauthToken));
        tokenService = developerSdk.Services.Iam.IamTokenService;
        this.saId = saId;
    }

    public string GetToken()
    {
        var expiration = DateTimeOffset.Now.ToUnixTimeSeconds() + 300;
        if (iamToken == null || iamToken.ExpiresAt.Seconds > expiration)
        {
            iamToken = tokenService.CreateForServiceAccount(new CreateIamTokenForServiceAccountRequest
            {
                ServiceAccountId = saId,
            });
        }

        return iamToken.IamToken;
    }
}