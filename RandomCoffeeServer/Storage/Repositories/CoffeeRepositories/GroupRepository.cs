﻿using RandomCoffeeServer.Domain.Models;
using RandomCoffeeServer.Storage.DbSchema;
using RandomCoffeeServer.Storage.YandexCloud.Ydb;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.Repositories.CoffeeRepositories;

public class GroupRepository : RepositoryBase
{
    private YdbTable Groups { get; }

    public GroupRepository(YdbService ydb)
        : base(ydb)
    {
        Groups = Schema.Groups;
    }

    public async Task AddGroup(Group group)
    {
        await Groups
            .Replace(group.ToYdb())
            .ExecuteNonData(Ydb);
    }

    public async Task<Group?> FindGroup(Guid groupId)
    {
        var groups = await Groups
            .Select()
            .Where("group_id", groupId.ToYdb())
            .ExecuteData(Ydb);

        return groups.SingleOrNull(Group.FromYdbRow);
    }

    public async Task<IEnumerable<Group>> FindPublicGroups()
    {
        var notPrivateValue = YdbValue.MakeInt32(0);
        var groups = await Groups
            .Select()
            .Where("is_private", notPrivateValue)
            .ExecuteData(Ydb);

        return groups.Select(Group.FromYdbRow);
    }
}