using System.Reflection;

namespace RandomCoffeeServer.Services.YandexCloud.Lockbox;

public enum SecretId
{
    [SecretId("e6qitbtg9d22o4t33oee")]
    Coffee,

    [SecretId("e6qpcaa6n128re2a5qrq")]
    CoffeeDbSaKey
}

public static class SecretIdExtensions
{
    public static string AsIdString(this SecretId secretId)
    {
        var secretType = typeof(SecretId);
        
        var secretName = Enum.GetName(secretType, secretId);
        if (secretName is null)
        {
            throw new ArgumentException($"Argument {secretId} is not of type {nameof(SecretId)}");
        } 
        
        var secretIdAttribute = secretType.GetField(secretName)!.GetCustomAttribute<SecretIdAttribute>();
        if (secretIdAttribute is null)
        {
            throw new InvalidProgramException($"Secret id {secretId} doesn't have {nameof(SecretIdAttribute)}");
        }

        return secretIdAttribute.SecretId;
    }
}