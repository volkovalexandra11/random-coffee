using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer.Controllers.ApiControllers.GroupsControllerDtos;
using RandomCoffeeServer.Controllers.ApiControllers.QueryStrings;
using RandomCoffeeServer.Domain;
using RandomCoffeeServer.Domain.Dtos;
using RandomCoffeeServer.Domain.Models;
using RandomCoffeeServer.Domain.Services.Coffee;
using RandomCoffeeServer.Domain.Services.Coffee.Rounds;
using RandomCoffeeServer.Storage.Repositories.AspIdentityStorages.IdentityModel;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Controllers.ApiControllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    public GroupsController(
        GroupService groupRepository,
        GroupRoundMakerService roundMakerService,
        UserManager<IdentityCoffeeUser> userManager)
    {
        this.groupService = groupRepository;
        this.roundMakerService = roundMakerService;
        this.userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupDto createGroupDto)
    {
        if (await HttpContext.GetUserAsync(userManager) is not { } groupAdmin)
            throw new InvalidProgramException();

        var group = await groupService.AddGroup(new GroupService.CreateGroupDto
        {
            Name = createGroupDto.Name,
            AdminUserId = groupAdmin.UserId,
            IsPrivate = createGroupDto.IsPrivate,
            NextRoundDate = createGroupDto.NextRoundDate,
            IntervalDays = createGroupDto.IntervalDays,
            GroupPictureUrl = createGroupDto.GroupPictureUrl,
            GroupDescription = createGroupDto.GroupDescription
        });

        var adminDto = new ParticipantDto
        {
            UserId = groupAdmin.UserId,
            FirstName = groupAdmin.FirstName,
            LastName = groupAdmin.LastName,
            ProfilePictureUrl = groupAdmin.ProfilePictureUrl
        };
        var groupWithParticipant = new GroupWithParticipantsDto()
        {
            GroupId = group.GroupId,
            Name = group.Name,
            Tag = group.Tag,
            Admin = adminDto,
            Participants = new List<ParticipantDto> { adminDto },
            IsPrivate = group.IsPrivate,
            NextRoundDate = DateTime.Now,
            GroupPictureUrl = group.GroupPictureUrl
        };
        return CreatedAtAction(nameof(Get), new { groupId = group.GroupId }, groupWithParticipant);
    }

    [HttpGet]
    public async Task<ActionResult<object>> Find([FromQuery] GroupsQueryStringParameters queryParameters)
    {
        if (queryParameters.UserId.HasValue &&
            queryParameters.UserId == Guid.Empty ||
            queryParameters.AdminUserId.HasValue &&
            queryParameters.AdminUserId == Guid.Empty ||
            queryParameters.UsersCount is < 0)
        {
            return BadRequest();
        }

        var filterParameters = new Dictionary<string, YdbValue>();
        filterParameters.Add("is_private", YdbValue.MakeInt32(0));
        if (queryParameters.Name != null && !string.IsNullOrEmpty(queryParameters.Name))
            filterParameters.Add("name", queryParameters.Name.ToYdb());
        if (queryParameters.Tag != null && !string.IsNullOrEmpty(queryParameters.Tag))
            filterParameters.Add("tag", queryParameters.Tag.ToLower().ToYdb());
        if (queryParameters.AdminUserId != null)
            filterParameters.Add("admin_user_id", queryParameters.AdminUserId.Value.ToYdb());
        
        var groups = (await groupService.FilterGroups(filterParameters).ConfigureAwait(false)).AsQueryable();
        var groupIdsFilteredByUserInfo = await FilterByUsersInfoAsync(
                groups.Select(g => g.GroupId),
                queryParameters.UserId,
                queryParameters.UsersCount)
            .ConfigureAwait(false);
        var resultGroups = groups.Where(g => groupIdsFilteredByUserInfo.Contains(g.GroupId));

        var pageList = PageList<Group>.ToPageList(
            Request,
            resultGroups,
            queryParameters.Page,
            queryParameters.PageSize);

        return Ok(pageList);
    }

    private async Task<IEnumerable<Guid>> FilterByUsersInfoAsync(
        IEnumerable<Guid> groupIds,
        Guid? userId,
        int? usersCount)
    {
        if (!userId.HasValue && !usersCount.HasValue)
            return groupIds;

        var groups = new List<Guid>();
        foreach (var groupId in groupIds)
        {
            var groupUserIds = await groupService.GetParticipantsInGroup(groupId).ConfigureAwait(false);
            if (groupUserIds is null) //note(Cockamamie): Не может быть, если смотрим список групп из groupService.FilterGroups
                continue;
            var hasUserId = userId.HasValue && groupUserIds.Contains(userId.Value);
            var hasUsersCount = usersCount.HasValue && groupUserIds.Length == usersCount.Value;
            if (hasUserId && hasUsersCount || hasUserId && !usersCount.HasValue || hasUsersCount && !userId.HasValue)
                groups.Add(groupId);
        }

        return groups;
    }

    [HttpGet("{groupId:guid}")]
    public async Task<ActionResult> Get(Guid groupId)
    {
        if (groupId == Guid.Empty)
            return BadRequest();

        var (group, participants) = await groupService.GetGroupWithParticipantModels(groupId);
        if (group is null)
            return NotFound();

        var participantsAsDto = participants!.Select(
                user => new ParticipantDto
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfilePictureUrl = user.ProfilePictureUrl
                })
            .ToList();

        var groupWithParticipants = new GroupWithParticipantsDto
        {
            GroupId = group.GroupId,
            Admin = participantsAsDto.First(participant => participant.UserId == group.AdminUserId),
            Name = group.Name,
            Tag = group.Tag,
            IsPrivate = group.IsPrivate,
            Participants = participantsAsDto,
            NextRoundDate = DateTime.Now,
            GroupPictureUrl = group.GroupPictureUrl,
            GroupDescription = group.GroupDescription
        };


        return Ok(groupWithParticipants);
    }

    [Authorize]
    [HttpPost("{groupId:guid}/join")]
    public async Task<IActionResult> Join(Guid groupId)
    {
        if (groupId == Guid.Empty)
            return BadRequest();

        if (HttpContext.GetUserId() is not { } userId)
            throw new InvalidProgramException();

        await groupService.AddParticipantToGroup(userId, groupId);
        return Ok();
    }

    [Authorize]
    [HttpPost("{groupId:guid}/leave")]
    public async Task<IActionResult> Leave(Guid groupId)
    {
        if (groupId == Guid.Empty)
            return BadRequest();

        if (HttpContext.GetUserId() is not { } userId)
            throw new InvalidProgramException();

        var deletionResult = await groupService.TryLeaveFromGroup(userId, groupId);
        switch (deletionResult)
        {
            case GroupService.DeleteParticipantResult.Success:
                return Ok();
            case GroupService.DeleteParticipantResult.NoGroupError:
                return NotFound();
            case GroupService.DeleteParticipantResult.DeletedParticipantIsGroupAdminError:
                return Conflict("Can't leave your own group");
            default:
                throw new InvalidProgramException();
        }
    }

    [Authorize]
    [HttpPost("{groupId:guid}/kick")]
    public async Task<IActionResult> Kick(Guid groupId, [FromBody] KickUserFromGroupDto? kickUserFromGroupDto)
    {
        if (groupId == Guid.Empty)
            return BadRequest("Bad path");
        if (kickUserFromGroupDto is null || kickUserFromGroupDto.UserId == Guid.Empty)
            return BadRequest("Bad body");

        if (HttpContext.GetUserId() is not { } kickingUserId)
            throw new InvalidProgramException();

        var deletionResult = await groupService.TryKickFromGroup(
            kickedUserId: kickUserFromGroupDto.UserId,
            kickingUserId,
            groupId
        );
        return deletionResult switch
        {
            GroupService.DeleteParticipantResult.Success => Ok(),
            GroupService.DeleteParticipantResult.NoGroupError => NotFound(),
            GroupService.DeleteParticipantResult.DeletedParticipantIsGroupAdminError => Conflict(
                "Can't leave your own group"),
            GroupService.DeleteParticipantResult.Forbidden => Forbid(),
            _ => throw new InvalidProgramException()
        };
    }

    [Authorize]
    [HttpPost("{groupId:guid}/make-round")]
    public async Task<IActionResult> Start(Guid groupId)
    {
        if (groupId == Guid.Empty)
            return BadRequest();

        var group = await groupService.GetGroup(groupId);

        if (group is null)
            return NotFound();

        var userId = HttpContext.GetUserId();
        if (userId != group.AdminUserId)
        {
            return Unauthorized();
        }

        await roundMakerService.MakeRound(groupId);
        return Ok();
    }

    private readonly GroupService groupService;
    private readonly GroupRoundMakerService roundMakerService;
    private readonly UserManager<IdentityCoffeeUser> userManager;
}