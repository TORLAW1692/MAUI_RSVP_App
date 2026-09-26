using Microsoft.AspNetCore.Mvc;
using RSVPAuthService.DataAccess;
using RSVPAuthService.Models;

namespace RSVPAuthService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [HttpGet]
    [BasicAuthentication]
    public IActionResult Get()
    {
        return Ok(new
        {
            authenticated = true
        });
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
        if (string.IsNullOrWhiteSpace(user.EmailAddress) ||
            string.IsNullOrWhiteSpace(user.Password))
        {
            return BadRequest("Email and password are required.");
        }

        UserData userData = new();

        bool created = userData.AddUser(user);

        if (!created)
            return Conflict("A user with that email already exists.");

        return Ok();
    }
}