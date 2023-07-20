using DemoWebAPI.Entities;
using DemoWebAPI.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TokenManager _tokenManager;

        public UserController(TokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        [HttpPost]
        public IActionResult Login(User u)
        {
            string token = _tokenManager.GenerateToken(u);
            return Ok(token);
        }
    }
}
