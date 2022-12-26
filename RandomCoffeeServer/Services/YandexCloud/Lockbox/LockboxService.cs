using Yandex.Cloud;
using Yandex.Cloud.Credentials;
using Yandex.Cloud.Lockbox.V1;

namespace RandomCoffeeServer.Services.YandexCloud.Lockbox;

public class LockboxService
{
    public LockboxService(ICredentialsProvider credentialsProvider)
    {
        payloadService = new Sdk(credentialsProvider).Services.Lockbox.PayloadService;
    }

    public async Task<byte[]> GetCoffeeDbServiceAccountKey()
    {
        var secret = await GetBinaryAsync(SecretId.CoffeeDbSaKey);
        return secret.Single().Value;
    }

    public async Task<string> GetCoffeeLocalOpenIdSecret()
    {
        var secret = await GetTextAsync(SecretId.CoffeeLocalSashaOpenIdSecret);
        return secret.Single().Value;
    }
    
    public async Task<string> GetCoffeeLocalOpenIdId()
    {
        var secret = await GetTextAsync(SecretId.CoffeeLocalSashaOpenIdId);
        return secret.Single().Value;
    }

    private async Task<Dictionary<string, string>> GetTextAsync(SecretId secretId)
    {
        var secret = await GetAsync(secretId);
        return secret.Entries.ToDictionary(entry => entry.Key, entry => entry.TextValue);
    }
    
    private async Task<Dictionary<string, byte[]>> GetBinaryAsync(SecretId secretId)
    {
        var secret = await GetAsync(secretId);
        return secret.Entries.ToDictionary(entry => entry.Key, entry => entry.BinaryValue.ToByteArray());
    }

    private async Task<Payload> GetAsync(SecretId secretId)
    {
        var secretIdString = secretId.AsIdString();
        return await payloadService.GetAsync(new GetPayloadRequest { SecretId = secretIdString });
    }

    private readonly PayloadService.PayloadServiceClient payloadService;
}