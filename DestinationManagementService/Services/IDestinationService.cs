using DestinationManagementService.DTOs;
using DestinationManagementService.Models;

namespace DestinationManagementService.Services
{
    public interface IDestinationService
    {
        Task<List<Destination>> GetAllDestinations();
        Task<Destination> GetDestinationById(string id);
        Task<Destination> CreateDestination(DestinationDTO destinationDTO);
    }
}
