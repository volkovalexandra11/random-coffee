using System.Net;
using System.Text.Json;
using Yandex.Cloud.Credentials;

namespace RandomCoffeeServer.Storage.YandexCloud.Ydb.YdbFactory;

// todo exists for debugging metadata problems in cloud
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
        Task<string> task = this.FetchToken();
        task.Wait();
        return task.Exception == null ? task.Result : throw task.Exception;
    }

    private async Task<string> FetchToken(int retry = 0)
    {
        if (retry == 2)
            throw new ApplicationException("failed to get token from metadata service after 3 retries");
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
            "http://169.254.169.254/computeMetadata/v1/instance/service-accounts/default/token");
        request.Headers.Add("Metadata-Flavor", "Google");
        HttpResponseMessage httpResponseMessage = await this._client.SendAsync(request);
        log.LogInformation(httpResponseMessage.StatusCode.ToString());
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        log.LogInformation(content);
        return httpResponseMessage.StatusCode != HttpStatusCode.OK
            ? await this.FetchToken(++retry)
            : JsonSerializer
                .Deserialize<Token>(content)
                .access_token;
    }

    private class Token
    {
        public string access_token { get; set; }
    }
}