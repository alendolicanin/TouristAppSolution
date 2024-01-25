using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using TouristAppGateway.Models;
using TouristAppGateway.Services;

namespace TouristAppGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly Urls urls;
        private readonly TokenService tokenService;

        public RecommendationController(HttpClient httpClient, IOptions<Urls> config, TokenService tokenService)
        {
            this.httpClient = httpClient;
            urls = config.Value;
            this.tokenService = tokenService;
        }

        [HttpGet("city"), Authorize(Roles = "Tourist")]
        public async Task<IActionResult> GetDestinationsByCity(string city)
        {
            try
            {
                var tokenHeader = Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(tokenHeader) || !tokenHeader.ToString().StartsWith("Bearer "))
                {
                    return Unauthorized(new { message = "Invalid or missing JWT token" });
                }

                var token = tokenHeader.ToString().Substring("Bearer ".Length).Trim();

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync(urls.Recommendations + "/recommendation/city/" + city);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var recommendations = JsonConvert.DeserializeObject<List<Destination>>(content);

                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("landmarks"), Authorize(Roles = "Tourist")]
        public async Task<IActionResult> GetDestinationsByLandmarks(string landmark)
        {
            try
            {
                var tokenHeader = Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(tokenHeader) || !tokenHeader.ToString().StartsWith("Bearer "))
                {
                    return Unauthorized(new { message = "Invalid or missing JWT token" });
                }

                var token = tokenHeader.ToString().Substring("Bearer ".Length).Trim();

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync(urls.Recommendations + $"/recommendation/landmarks?landmark={landmark}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var recommendations = JsonConvert.DeserializeObject<List<Destination>>(content);

                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
