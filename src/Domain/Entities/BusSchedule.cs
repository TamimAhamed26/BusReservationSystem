namespace Domain.Entities
{
    public class BusSchedule
    {
        public Guid Id { get; set; }

        public Guid BusId { get; set; }
        public Guid RouteId { get; set; }

        public DateTime JourneyDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public decimal Price { get; set; }

        // Navigation Properties
        public Bus? Bus { get; set; }
        public Route? Route { get; set; }

        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
