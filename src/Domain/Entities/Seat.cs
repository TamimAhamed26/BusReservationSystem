namespace Domain.Entities
{
    public enum SeatStatus { Available, Booked, Sold }

    public class Seat
    {
        public Guid Id { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public SeatStatus Status { get; set; } = SeatStatus.Available;

        public Guid BusScheduleId { get; set; }
        public BusSchedule? BusSchedule { get; set; }


public int Row { get; set; } 
public int Column { get; set; } 
        public Ticket? Ticket { get; set; }
    }
}
