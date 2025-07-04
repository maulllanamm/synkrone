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
    private readonly ILogger<UserController> _logger;
    
    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id)
    {
        _logger.LogInformation("Getting user with id {id}", id);
        
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            _logger.LogInformation("User with id {id} not found", id);
            return NotFound();
        }

        return Ok(user);
    }
}