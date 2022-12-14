using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Services.Coffee;

namespace RandomCoffeeServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupUserController : ControllerBase
{
    public GroupUserController(GroupUserService groupUserService)
    {
        this.groupUserService = groupUserService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddUserToGroup([FromBody] GroupUserDto groupUserDto)
    {
        await groupUserService.AddUserToGroup(groupUserDto);

        return Ok();
    }

    private readonly GroupUserService groupUserService;
}