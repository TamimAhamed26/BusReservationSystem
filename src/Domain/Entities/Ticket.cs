namespace Domain.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public Guid PassengerId { get; set; }
        public Guid SeatId { get; set; }
        public DateTime BookingDate { get; set; }

public string BoardingPoint { get; set; } = string.Empty; 
public string DroppingPoint { get; set; } = string.Empty; 

        public Passenger? Passenger { get; set; }
        public Seat? Seat { get; set; }
    }
}
