using System.Text;
using Grpc.Core;
using Newtonsoft.Json;
using RandomCoffee.Lockbox;
using Yandex.Cloud;
using Yandex.Cloud.Credentials;
using Yandex.Cloud.Iam.V1;
using Yandex.Cloud.Lockbox.V1;

namespace RandomCoffee;

    public class SaCredentialsProvider : ICredentialsProvider
    {
        private readonly string iam;
    
        public SaCredentialsProvider(string saIam)
        {
            iam = saIam;
        }
    
        public string GetToken()
        {
            return iam;
        }
    }
    
public class LockboxService
{

    public LockboxService()
    {
        var sdk = new Sdk(new OAuthCredentialsProvider("тоже мой токен"));
        
        var iam = sdk.Services.Iam.IamTokenService
            .CreateForServiceAccountAsync(
                new CreateIamTokenForServiceAccountRequest() { ServiceAccountId = "ajeta551repsehi6ko29" }).GetAwaiter()
            .GetResult().IamToken;

        var lockboxSdk = new Sdk(new SaCredentialsProvider(iam));
        
        var a = lockboxSdk.Services.Lockbox.PayloadService.GetAsync(new GetPayloadRequest { SecretId = SecretsIds.Coffee }).GetAwaiter().GetResult();
            
        Console.WriteLine(a);
        Console.WriteLine(JsonConvert.SerializeObject(a));
        Console.WriteLine();
    }
}