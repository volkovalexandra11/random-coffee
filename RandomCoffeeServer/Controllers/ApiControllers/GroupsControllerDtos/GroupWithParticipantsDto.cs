﻿using RandomCoffeeServer.Domain.Dtos;

namespace RandomCoffeeServer.Controllers.ApiControllers.GroupsControllerDtos;

public class GroupWithParticipantsDto
{
    public Guid GroupId { get; init; }
    public ParticipantDto Admin { get; init; }
    public string Name { get; init; }
    public string Tag { get; init; }
    public bool IsPrivate { get; init; }
    public List<ParticipantDto> Participants { get; init; }
    public DateTime NextRoundDate { get; init; }
    public string? GroupPictureUrl { get; init; }
    public string? GroupDescription { get; init; }
}