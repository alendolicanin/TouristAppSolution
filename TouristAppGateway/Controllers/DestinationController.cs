using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using TouristAppGateway.DTOs;
using TouristAppGateway.Models;
using TouristAppGateway.Services;

namespace TouristAppGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly Urls urls;
        private readonly TokenService tokenService;

        public DestinationController(HttpClient httpClient, IOptions<Urls> config, TokenService tokenService)
        {
            this.httpClient = httpClient;
            urls = config.Value;
            this.tokenService = tokenService;
        }

        [HttpGet("get-destinations")]
        public async Task<IActionResult> GetAllDestinations()
        {
            try
            {
                var response = await httpClient.GetAsync(urls.Destinations + "/api/Destination/get-destinations");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var destinations = JsonConvert.DeserializeObject<List<Destination>>(content);

                return Ok(destinations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("destination/{id}")]
        public async Task<IActionResult> GetDestinationById(string id)
        {
            try
            {
                var response = await httpClient.GetAsync(urls.Destinations + "/api/Destination/destination/" + id);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var destination = JsonConvert.DeserializeObject<Destination>(content);

                return Ok(destination);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("add-destination"), Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateDestination(DestinationDTO destinationDTO)
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

                var destination = new Destination
                {
                    Name = destinationDTO.Name,
                    Description = destinationDTO.Description,
                    City = destinationDTO.City,
                    Country = destinationDTO.Country,
                    Landmarks = destinationDTO.Landmarks,
                    Rating = destinationDTO.Rating
                };

                var response = await httpClient.PostAsJsonAsync(urls.Destinations + "/api/Destination/add-destination", destination);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var destinationNew = JsonConvert.DeserializeObject<Destination>(content);

                return Ok(destinationNew);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
