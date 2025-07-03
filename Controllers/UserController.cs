using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using synkrone.Model.DTO;
using synkrone.Services.Interfaces;

namespace synkrone.Controllers;

[ApiController]
[Route("api/users")]
public class UserController: ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id)
    {
        var user = await _userService.GetUserAsync(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }
}