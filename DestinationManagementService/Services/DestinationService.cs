using DestinationManagementService.DTOs;
using DestinationManagementService.Models;
using DestinationManagementService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DestinationManagementService.Services
{
    public class DestinationService : IDestinationService
    {
        private readonly IMessageBroker messageBroker;
        private readonly IMongoCollection<Destination> destinationCollection;

        public DestinationService(IMessageBroker messageBroker, IOptions<MongoDBSettings> mongoDBSettings)
        {
            this.messageBroker = messageBroker;
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            destinationCollection = database.GetCollection<Destination>(mongoDBSettings.Value.CollectionName);
        }

        public async Task<Destination> CreateDestination(DestinationDTO destinationDTO)
        {
            var destination = new Destination
            {
                Name = destinationDTO.Name,
                Description = destinationDTO.Description,
                City = destinationDTO.City,
                Country = destinationDTO.Country,
                Landmarks = destinationDTO.Landmarks,
                Rating = destinationDTO.Rating
            };

            await InsertIntoMongoDB(destination);

            messageBroker.Publish(destination);

            return destination;
        }

        public async Task<List<Destination>> GetAllDestinations()
        {
            var destinationsFromMongoDB = await GetAllDestinationsFromMongoDB();
            return destinationsFromMongoDB;
        }

        public async Task<Destination> GetDestinationById(string id)
        {
            var destinationFromMongoDB = await GetDestinationByIdFromMongoDB(id);
            return destinationFromMongoDB;
        }

        public async Task InsertIntoMongoDB(Destination destination)
        {
            await destinationCollection.InsertOneAsync(destination);
        }

        public async Task<List<Destination>> GetAllDestinationsFromMongoDB()
        {
            var destinations = await destinationCollection.Find(_ => true).ToListAsync();
            return destinations;
        }

        public async Task<Destination> GetDestinationByIdFromMongoDB(string id)
        {
            var destination = await destinationCollection.Find(d => d.Id == id).FirstOrDefaultAsync();
            return destination;
        }
    }
}
