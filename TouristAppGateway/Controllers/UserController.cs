using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TouristAppGateway.DTOs;
using TouristAppGateway.Models;
using TouristAppGateway.Services;

namespace TouristAppGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly Urls urls;
        private readonly TokenService tokenService;

        public UserController(HttpClient httpClient, IOptions<Urls> config, TokenService tokenService)
        {
            this.httpClient = httpClient;
            urls = config.Value;
            this.tokenService = tokenService;
        }

        [HttpGet("get-tourists")]
        public async Task<IActionResult> GetTourists()
        {
            var response = httpClient.GetAsync(urls.Users + "/api/User/get-tourists").Result;

            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var tourists = JsonConvert.DeserializeObject<List<User>>(content);

            return Ok(tourists);
        }

        [HttpGet("get-managers")]
        public async Task<IActionResult> GetManagers()
        {
            var response = httpClient.GetAsync(urls.Users + "/api/User/get-managers").Result;

            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var managers = JsonConvert.DeserializeObject<List<User>>(content);

            return Ok(managers);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var response = httpClient.GetAsync(urls.Users + "/api/User/user/" + id).Result;

            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<User>(content);

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> UserRegister(UserRegisterDTO userRegisterDTO)
        {
            var response = httpClient.PostAsJsonAsync(urls.Users + "/api/User/register", userRegisterDTO).Result;

            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<User>(content);

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> UserLogin(UserLoginDTO userLoginDTO)
        {
            var response = httpClient.PostAsJsonAsync(urls.Users + "/api/User/login", userLoginDTO).Result;

            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<User>(content);

            var token = tokenService.GenerateToken(user);

            return Ok(new { user, token });
        }
    }
}
