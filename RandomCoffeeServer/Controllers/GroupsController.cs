using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer.Controllers.GroupsControllerDtos;
using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Services.Coffee;

namespace RandomCoffeeServer.Controllers;

[ApiController]
[Route("api/[controller]")]
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

    // [HttpGet]
    // public async Task<ActionResult> Get()
    // {
    //     return await db.Users.ToListAsync();
    // }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(Guid id)
    {
        return Ok(await groupService.GetGroup(id));
    }

    private readonly GroupService groupService;
}