namespace Application.Contracts.DTOs
{
    public class SeatPlanDto
    {
        public Guid BusScheduleId { get; set; }
        public List<SeatDto> Seats { get; set; } = new List<SeatDto>();
    }
}
