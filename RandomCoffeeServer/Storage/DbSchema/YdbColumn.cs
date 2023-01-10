using Type = Ydb.Type;

namespace RandomCoffeeServer.Storage.DbSchema;

public class YdbColumn
{
    public string Name { get; }

    public Type.Types.PrimitiveTypeId Type { get; }

    public YdbColumn(string name, Type.Types.PrimitiveTypeId type)
    {
        Name = name;
        Type = type;
    }

    public string ToDdl()
    {
        return $"{Name} {Type}";
    }
}