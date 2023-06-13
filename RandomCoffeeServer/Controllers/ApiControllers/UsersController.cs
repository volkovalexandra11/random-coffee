using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer.Controllers.ApiControllers.UsersControllerDtos;
using RandomCoffeeServer.Domain.Models;
using RandomCoffeeServer.Domain.Services.Coffee;

namespace RandomCoffeeServer.Controllers.ApiControllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService userService;
    private readonly GroupService groupService;

    public UsersController(UserService userService, GroupService groupService)
    {
        this.userService = userService;
        this.groupService = groupService;
    }

    [HttpGet]
    public async Task<IActionResult> Find([FromQuery] Guid groupId)
    {
        if (groupId == Guid.Empty)
            return BadRequest();

        var users = await groupService.GetParticipantsInGroup(groupId);
        if (users is null)
            return NotFound();

        return Ok(users);
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<User>> GetUser(Guid userId)
    {
        if (userId == Guid.Empty)
            return BadRequest();

        var user = await userService.GetUser(userId);
        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto updateUserDto) 
    {
        if (userId == Guid.Empty)
            return BadRequest();
        
        var user = await userService.GetUser(userId).ConfigureAwait(false);
        if (user is null)
            return NotFound();
        
        var updatedUser = FromUpdateUserDto(userId, updateUserDto);
        await userService.UpdateUser(updatedUser).ConfigureAwait(false);
        
        return NoContent();
    }

    [HttpPatch("{userId:guid}")]
    public async Task<IActionResult> PatchUser(Guid userId, [FromBody] JsonPatchDocument<User> userPatch)
    {
        if (userId == Guid.Empty)
            return BadRequest();
        
        var user = await userService.GetUser(userId).ConfigureAwait(false);
        if (user is null)
            return NotFound();
        
        userPatch.ApplyTo(user);
        await userService.UpdateUser(user).ConfigureAwait(false);
        
        return NoContent();
    }

    private static User FromUpdateUserDto(Guid userId, UpdateUserDto updateUserDto)
    {
        var user = new User
        {
            UserId = userId,
            Email = updateUserDto.Email,
            FirstName = updateUserDto.FirstName,
            LastName = updateUserDto.LastName,
            ProfilePictureUrl = updateUserDto.ProfilePictureUrl
        };

        return user;
    }
}