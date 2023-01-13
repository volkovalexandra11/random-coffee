using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.Repositories;
using RandomCoffeeServer.Storage.DbSchema;
using RandomCoffeeServer.Storage.YandexCloud.Ydb;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.Repositories.AspIdentityStorages;

// (used for getting user session from AspNet.Identity.Application cookie as id)
// todo: remove this monster and move to jwt authentication
public class DataProtectionKeysUnprotectedRepository : RepositoryBase, IXmlRepository
{
    private YdbTable DataProtectionKeysAsp { get; }

    public DataProtectionKeysUnprotectedRepository(YdbService ydb) : base(ydb)
    {
        DataProtectionKeysAsp = Schema.DataProtectionKeysAsp;
    }

    public IReadOnlyCollection<XElement> GetAllElements()
    {
        return GetAllElementsAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    private async Task<IReadOnlyCollection<XElement>> GetAllElementsAsync()
    {
        var allElements = await DataProtectionKeysAsp
            .Select()
            .ExecuteData(Ydb);

        return allElements.Select(row => XElement.Parse(row["xml"].GetNonNullUtf8())).ToList().AsReadOnly();
    }

    public void StoreElement(XElement element, string friendlyName)
    {
        StoreElementAsync(element, friendlyName).ConfigureAwait(false).GetAwaiter().GetResult();
    }

    private async Task StoreElementAsync(XElement element, string friendlyName)
    {
        var @params = new Dictionary<string, YdbValue>
        {
            ["element_id"] = Guid.NewGuid().ToYdb(),
            ["friendly_name"] = friendlyName.ToYdb(),
            ["xml"] = element.ToString(SaveOptions.DisableFormatting).ToYdb()
        };
        await DataProtectionKeysAsp
            .Insert(@params)
            .ExecuteData(Ydb);
    }
}