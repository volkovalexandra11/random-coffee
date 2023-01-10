using RandomCoffeeServer.Storage.YandexCloud.Ydb;

namespace RandomCoffeeServer.Storage.Repositories;

public abstract class RepositoryBase
{
    protected RepositoryBase(YdbService ydb)
    {
        this.Ydb = ydb;
    }

    protected readonly YdbService Ydb;
}