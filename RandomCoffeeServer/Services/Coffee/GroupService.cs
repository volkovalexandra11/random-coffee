using RandomCoffee.Dtos;
using RandomCoffee.Repositories;
using RandomCoffee.schema;
using Ydb.Sdk.Value;

namespace RandomCoffee;

public class GroupService
{
    public GroupService(
        GroupRepository groupRepository,
        GroupUserRepository groupUserRepository)
    {
        this.groupRepository = groupRepository;
        this.groupUserRepository = groupUserRepository;
    }

    public async Task AddGroup(GroupDto group)
    {
        await groupRepository.AddGroup(group);
    }

    public async Task AddToGroup(GroupUserDto groupUser)
    {
        await groupUserRepository.AddToGroup(groupUser);
    }

    private readonly GroupRepository groupRepository;
    private readonly GroupUserRepository groupUserRepository;
}