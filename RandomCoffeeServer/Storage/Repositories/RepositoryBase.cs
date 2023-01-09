using RandomCoffeeServer.Storage.DbSchema;
using RandomCoffeeServer.Storage.YandexCloud.Ydb;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;

namespace RandomCoffeeServer.Storage.Repositories;

public abstract class RepositoryBase
{
    protected RepositoryBase(YdbService ydb)
    {
        this.Ydb = ydb;
    }

    protected readonly YdbService Ydb;
}