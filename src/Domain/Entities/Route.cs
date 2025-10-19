namespace Domain.Entities
{
    public class Route
    {
        public Guid Id { get; set; }
        public string FromCity { get; set; } = string.Empty;
        public string ToCity { get; set; } = string.Empty;

        public ICollection<BusSchedule> BusSchedules { get; set; } = new List<BusSchedule>();
    }
}
 