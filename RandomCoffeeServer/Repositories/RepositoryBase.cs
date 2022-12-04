using RandomCoffee.QueryBuilder;
using RandomCoffee.schema;
using Ydb.Sdk.Value;

namespace RandomCoffee.Repositories;

public abstract class RepositoryBase
{
    protected RepositoryBase(YdbService ydb, string tableName)
    {
        this.Ydb = ydb;
        this.Table = Schema.Tables.Single(ydbTable => ydbTable.TableName == tableName);
        this.DeclareStatement = QueryBuilder.QueryBuilder.AsTableDeclare(this.Table);
    }

    protected readonly YdbService Ydb;
    protected readonly YdbTable Table;
    protected readonly string DeclareStatement;
}