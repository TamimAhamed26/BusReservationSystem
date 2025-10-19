namespace Application.Contracts.DTOs
{
    public class AvailableBusDto
    {
        public Guid BusScheduleId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string BusName { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public int SeatsLeft { get; set; }
        public decimal Price { get; set; }
        public string FromCity { get; set; } = string.Empty;
        public string ToCity { get; set; } = string.Empty;
        public DateTime JourneyDate { get; set; }
    }
}
