namespace Domain.Entities
{
    public enum SeatStatus { Available, Booked, Sold }

    public class Seat
    {
        public Guid Id { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public int Row { get; set; }
        public int Column { get; set; }

        public SeatStatus Status { get; private set; } = SeatStatus.Available;

        public Guid BusScheduleId { get; set; }
        public BusSchedule? BusSchedule { get; set; }
        public Ticket? Ticket { get; set; }

        public Seat() { }

        public Seat(string seatNumber, int row, int column, Guid busScheduleId, SeatStatus status = SeatStatus.Available)
        {
            Id = Guid.NewGuid();
            SeatNumber = seatNumber;
            Row = row;
            Column = column;
            BusScheduleId = busScheduleId;
            Status = status; 
        }


        public bool CanBook() => Status == SeatStatus.Available;

        public void Book()
        {
            if (!CanBook())
                throw new InvalidOperationException($"Seat {SeatNumber} is not available.");
            Status = SeatStatus.Booked;
        }

        public void MakeSold()
        {
            if (Status != SeatStatus.Booked)
                throw new InvalidOperationException($"Seat {SeatNumber} must be booked before it can be sold.");
            Status = SeatStatus.Sold;
        }

        public void MakeAvailable() => Status = SeatStatus.Available;
    }
}
