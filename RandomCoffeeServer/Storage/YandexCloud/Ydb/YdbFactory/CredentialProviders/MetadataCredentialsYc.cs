using System.Net;
using System.Text.Json;
using Yandex.Cloud.Credentials;

namespace RandomCoffeeServer.Storage.YandexCloud.Ydb.YdbFactory;

// todo exists for debugging metadata problems in cloud
// https://github.com/yandex-cloud/dotnet-sdk/blob/master/Yandex.Cloud.SDK/credentials/MetadataCredentialsProvider.cs
public class MetadataCredentialsProvider : ICredentialsProvider
{
    private readonly HttpClient _client = new HttpClient();
    private readonly ILogger<MetadataCredentialsProvider> log;

    public MetadataCredentialsProvider(ILoggerFactory log)
    {
        this.log = log.CreateLogger<MetadataCredentialsProvider>();
    }

    public string GetToken()
    {
        var task = FetchToken();

        task.Wait();
        if (task.Exception != null)
        {
            throw task.Exception;
        }

        return task.Result;
    }

    private async Task<string> FetchToken(int retry = 0)
    {
        if (retry == 2)
        {
            throw new ApplicationException("failed to get token from metadata service after 3 retries");
        }

        var request = new HttpRequestMessage(HttpMethod.Get,
            "http://169.254.169.254/computeMetadata/v1/instance/service-accounts/default/token");
        request.Headers.Add("Metadata-Flavor", "Google");

        var response = await _client.SendAsync(request);
        log.LogInformation(response.StatusCode.ToString());
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return await FetchToken(++retry);
        }

        var data = await response.Content.ReadAsStringAsync();
        log.LogInformation(data);
        var token = JsonSerializer.Deserialize<Token>(data);
        return token.access_token;
    }

    class Token
    {
        public string access_token { get; set; }

        //public string expires_in;
        //public string token_type;
    }
}