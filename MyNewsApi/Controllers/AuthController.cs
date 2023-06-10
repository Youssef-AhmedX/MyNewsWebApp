using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyNewsApi.Dtos;
using MyNewsApi.Services;

namespace MyNewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Some properties are not valid");


            var result = await _userService.LoginUserAsync(model);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
