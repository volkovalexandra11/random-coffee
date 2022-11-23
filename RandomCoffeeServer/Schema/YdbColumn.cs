﻿using Ydb.Sdk.Value;
using Type = Ydb.Type;

namespace RandomCoffee.schema;

public class YdbColumn
{
    public string Name { get; }
    public Type.Types.PrimitiveTypeId Type { get; }

    public YdbColumn(string name, Type.Types.PrimitiveTypeId type)
    {
        Name = name;
        Type = type;
    }
}