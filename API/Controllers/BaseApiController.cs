using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ServiceFilter(typeof(LogUserActivity))] // Action filter
[ApiController]
[Route("api/[controller]")] // Route will be /api/users
public class BaseApiController : ControllerBase
{
    
}