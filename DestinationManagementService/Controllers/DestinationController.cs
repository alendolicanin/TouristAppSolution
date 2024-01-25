using DestinationManagementService.DTOs;
using DestinationManagementService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DestinationManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController : ControllerBase
    {
        private readonly IDestinationService destinationService;

        public DestinationController(IDestinationService destinationService)
        {
            this.destinationService = destinationService;
        }

        [HttpGet("get-destinations")]
        public async Task<IActionResult> GetAllDestinations()
        {
            try
            {
                var destinations = await destinationService.GetAllDestinations();
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
                var destination = await destinationService.GetDestinationById(id);
                return Ok(destination);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("add-destination")]
        public async Task<IActionResult> CreateDestination(DestinationDTO destinationDTO)
        {
            try
            {
                var destination = await destinationService.CreateDestination(destinationDTO);
                return Ok(destination);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
