using RandomCoffeeServer.Domain.Models;

namespace RandomCoffeeServer.Domain.Dtos;

public class GroupWithUsersModels
{
    public Guid GroupId { get; init; }
    public User Admin { get; init; }
    public string Name { get; init; }
    public bool IsPrivate { get; init; }
    public List<User> Users { get; init; }
    public DateTime NextRoundDate { get; init; }
}