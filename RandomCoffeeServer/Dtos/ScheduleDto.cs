namespace RandomCoffee.Dtos;

public class ScheduleDto
{
    public long GroupId { get; init; }
    public int IntervalDays { get; init; }
    public long NextRoundId { get; init; }
}