using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Repositories;
namespace RandomCoffeeServer.Services.Coffee;

public class RoundService
{
    public RoundService(RoundRepository roundRepository)
    {
        this.roundRepository = roundRepository;
    }

    public async Task AddRound(RoundDto round)
    {
        await roundRepository.AddRound(round);
    }

    public async Task<RoundDto?> GetRoundByGroupId(Guid groupId)
    {
        return await roundRepository.FindRoundByGroupId(groupId);
    }

    private readonly RoundRepository roundRepository;
}