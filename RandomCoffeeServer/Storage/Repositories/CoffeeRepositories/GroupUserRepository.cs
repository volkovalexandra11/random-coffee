using RandomCoffeeServer.Domain.Dtos;
using RandomCoffeeServer.Storage.DbSchema;
using RandomCoffeeServer.Storage.YandexCloud.Ydb;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;

namespace RandomCoffeeServer.Storage.Repositories.CoffeeRepositories;

public class GroupUserRepository : RepositoryBase
{
    private YdbTable GroupsUsers { get; }

    public GroupUserRepository(YdbService ydb)
        : base(ydb)
    {
        GroupsUsers = Schema.GroupsUsers;
    }

    public async Task AddToGroup(Guid groupId, Guid userId)
    {
        await GroupsUsers
            .Replace(new GroupUserDto
                {
                    GroupId = groupId,
                    UserId = userId
                }.ToYdb()
            )
            .ExecuteNonData(Ydb);
    }

    // Guid[] if found some users, null if no users found (<=> group doesn't exist) 
    public async Task<Guid[]?> FindUsersInGroup(Guid groupId)
    {
        var userIds = await GroupsUsers
            .Select("user_id")
            .Where("group_id", groupId.ToYdb())
            .ExecuteData(Ydb);

        return userIds.Count > 0
            ? userIds.Select(row => row["user_id"].GetNonNullGuid()).ToArray()
            : null;
    }

    public async Task<Guid[]?> FindGroupsByParticipant(Guid userId)
    {
        var groupIds = await GroupsUsers
            .Select("group_id")
            .ViewByColumn("user_id")
            .Where("user_id", userId.ToYdb())
            .ExecuteData(Ydb);

        return groupIds.Count > 0
            ? groupIds.Select(row => row["group_id"].GetNonNullGuid()).ToArray()
            : null;
    }

    public async Task<int?> GetParticipantsCount(Guid groupId)
    {
        var counts = await GroupsUsers
            .Select("Count(*)")
            .Where("group_id", groupId.ToYdb())
            .ExecuteData(Ydb);

        return counts.SingleOrNoValue(row => (int)row[0].GetUint64());
    }
}