using Type = Ydb.Type;

namespace RandomCoffeeServer.DbSchema;

public class YdbColumn
{
    public string Name { get; }
    public Type.Types.PrimitiveTypeId Type { get; }
    // Only for PK columns, makes T instead of Optional<T>
    public bool IsNotNull { get; }

    public YdbColumn(string name, Type.Types.PrimitiveTypeId type, bool isNotNull = false)
    {
        Name = name;
        Type = type;
        IsNotNull = IsNotNull;
    }

    public string ToDdl()
    {
        return $"{Name} {Type}{(IsNotNull ? " NOT NULL" : string.Empty)}";
    }
}