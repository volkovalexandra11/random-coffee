using Microsoft.AspNetCore.Mvc;
using RandomCoffee.Dtos;
using RandomCoffee.Models;

namespace RandomCoffee.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupsController : ControllerBase
{
    public GroupsController(GroupService groupService)
    {
        this.groupService = groupService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupDto createGroupDto)
    {
        // var userId = authTokenToUserService.Find(Http.Cookies.MyCoolAuthToken); or something like that....
        var userId = Guid.Empty; //tmp

        var groupId = Guid.NewGuid();

        await groupService.AddGroup(
            new GroupDto
            {
                Name = createGroupDto.Name,
                GroupId = groupId,
                IsPrivate = createGroupDto.IsPrivate,
                AdminUserId = userId
            });

        return Ok();
    }

    private readonly GroupService groupService;
}