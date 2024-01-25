namespace TouristAppGateway.DTOs
{
    public class DestinationDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<string> Landmarks { get; set; }
        public double Rating { get; set; }
    }
}
