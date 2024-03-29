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

    public async Task UpdateGroupAsync(Guid groupId, Group group)
    {
        var whereParameters = new Dictionary<string, YdbValue>
        {
            {
                "group_id",
                groupId.ToYdb()
            }
        };
        var setParameters = group.ToYdb();
        setParameters.Remove("group_id");
        await Groups.Update(setParameters, whereParameters).ExecuteData(Ydb);
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

    public async Task<IEnumerable<Group>> FindGroups(Dictionary<string, YdbValue> filterParameters)
    {
        var query = Groups.Select();
        foreach (var (filterKey, filterValue) in filterParameters)
        {
            query = query.Where(filterKey, filterValue);
        }

        var groups = await query.ExecuteData(Ydb);

        return groups.Select(Group.FromYdbRow);
    }
}