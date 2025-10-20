namespace Application.Contracts.DTOs
{
    public class SeatPlanDto
    {
        public Guid BusScheduleId { get; set; }
        public string BusName { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string FromCity { get; set; } = string.Empty;
        public string ToCity { get; set; } = string.Empty;
        public DateTime JourneyDate { get; set; }

        public List<SeatDto> Seats { get; set; } = new();
    }
}
