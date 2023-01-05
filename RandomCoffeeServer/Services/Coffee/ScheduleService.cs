using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Repositories;
namespace RandomCoffeeServer.Services.Coffee;

public class ScheduleService
{
    public ScheduleService(ScheduleRepository scheduleRepository)
    {
        this.scheduleRepository = scheduleRepository;
    }

    public async Task AddSchedule(ScheduleDto schedule)
    {
        await scheduleRepository.AddSchedule(schedule);
    }

    public async Task<ScheduleDto?> GetScheduleByGroupId(Guid groupId)
    {
        return await scheduleRepository.FindSchedule(groupId);
    }

    private readonly ScheduleRepository scheduleRepository;
}