namespace Domain.Entities
{
    public class Bus
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string BusName { get; set; } = string.Empty;
        public int TotalSeats { get; set; }

        public ICollection<BusSchedule> Schedules { get; set; } = new List<BusSchedule>();
    }
}
