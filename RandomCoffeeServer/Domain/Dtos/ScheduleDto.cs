﻿namespace RandomCoffeeServer.Domain.Dtos;

public class ScheduleDto
{
    public Guid GroupId { get; init; }
    public int IntervalDays { get; init; }
    public long NextRoundId { get; init; }
}