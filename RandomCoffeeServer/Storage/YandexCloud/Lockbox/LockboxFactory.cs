using Yandex.Cloud.Credentials;

namespace RandomCoffeeServer.Storage.YandexCloud.Lockbox;

public class LockboxFactory
{
    public LockboxService Create(IHostEnvironment environment)
    {
        if (environment.IsProduction())
        {
            return new LockboxService(new MetadataCredentialsProvider());
        }
        if (environment.IsDevelopment())
        {
            if (Environment.GetEnvironmentVariable("DEV_OAUTH_TOKEN") is not { } devOauthToken)
                throw new InvalidProgramException("DEV_OAUTH_TOKEN env variable required");

            return new LockboxService(new OAuthCredentialsProvider(devOauthToken));
        }
        throw new InvalidProgramException("Unsupported environment");
    }
}