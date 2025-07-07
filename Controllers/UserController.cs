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
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchUsers([FromQuery] string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            _logger.LogWarning("Search term was empty or null.");
            return BadRequest("Search term is required.");
        }

        try
        {
            _logger.LogInformation("Searching users with term: {SearchTerm}", searchTerm);

            var users = await _userService.SearchUsersAsync(searchTerm);

            _logger.LogInformation("Found {Count} users for search term: {SearchTerm}", users.Count, searchTerm);

            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching users with term: {SearchTerm}", searchTerm);
            return StatusCode(500, "An error occurred while searching users.");
        }
    }
    
    [HttpPut("{id:guid}/profile")]
    public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UpdateUserDto updateUserDto)
    {
        _logger.LogInformation("Received profile update for userId: {UserId}", id);

        try
        {
            var updatedUser = await _userService.UpdateProfileAsync(id, updateUserDto);
            _logger.LogInformation("User profile updated successfully for userId: {UserId}", id);
            return Ok(updatedUser);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Update failed for userId: {UserId} - {Message}", id, ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating profile for userId: {UserId}", id);
            return StatusCode(500, "An error occurred while updating the profile.");
        }
    }

    [HttpPatch("{id:guid}/online-status")]
    public async Task<IActionResult> UpdateOnlineStatus(Guid id, [FromQuery] bool isOnline)
    {
        _logger.LogInformation("Received request to update online status for userId: {UserId} to {IsOnline}", id, isOnline);

        try
        {
            var result = await _userService.UpdateOnlineStatusAsync(id, isOnline);

            if (!result)
            {
                _logger.LogWarning("User not found when trying to update online status. userId: {UserId}", id);
                return NotFound("User not found.");
            }

            _logger.LogInformation("Online status updated successfully for userId: {UserId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating online status for userId: {UserId}", id);
            return StatusCode(500, "An error occurred while updating online status.");
        }
    }

}