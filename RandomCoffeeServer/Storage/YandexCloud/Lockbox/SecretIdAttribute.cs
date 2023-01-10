namespace RandomCoffeeServer.Storage.YandexCloud.Lockbox;

public class SecretIdAttribute : Attribute
{
    public SecretIdAttribute(string secretId)
    {
        SecretId = secretId;
    }

    public readonly string SecretId;
}