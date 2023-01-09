using RandomCoffeeServer.Storage.DbSchema;
using RandomCoffeeServer.Storage.YandexCloud.Ydb;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;

namespace RandomCoffeeServer.Storage.Repositories;

public abstract class RepositoryBase
{
    protected RepositoryBase(YdbService ydb, string tableName)
    {
        this.Ydb = ydb;
        this.Table = Schema.Tables.Single(ydbTable => ydbTable.TableName == tableName);
        this.DeclareStatement = QueryBuilder.AsTableDeclare(this.Table);
    }

    protected readonly YdbService Ydb;
    protected readonly YdbTable Table;
    protected readonly string DeclareStatement;
}