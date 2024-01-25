using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TouristManagementService.DTOs;
using TouristManagementService.Services;

namespace TouristManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("get-tourists")]
        public async Task<IActionResult> GetTourists()
        {
            try
            {
                var tourists = await userService.GetAllTourists();
                return Ok(tourists);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("get-managers")]
        public async Task<IActionResult> GetManagers()
        {
            try
            {
                var managers = await userService.GetAllManagers();
                return Ok(managers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await userService.GetUserById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> UserRegister(UserRegisterDTO userRegisterDTO)
        {
            try
            {
                var user = await userService.Register(userRegisterDTO);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> UserLogin(UserLoginDTO userLoginDTO)
        {
            try
            {
                var user = await userService.Login(userLoginDTO);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
