namespace Application.Contracts.DTOs
{
    public class BookSeatInputDto
    {
        public Guid BusScheduleId { get; set; }
        public List<Guid> SeatIds { get; set; } = new List<Guid>();

        // passenger info (for simplicity single passenger per seat in this DTO)
        public string PassengerName { get; set; } = string.Empty;
        public string PassengerMobile { get; set; } = string.Empty;

        public string BoardingPoint { get; set; } = string.Empty;
        public string DroppingPoint { get; set; } = string.Empty;

        // whether to mark final (Sold) or keep as Booked
        public bool ConfirmAndSell { get; set; } = true;
    }
}
