using Domain.Entities;

namespace Application.Contracts.Interfaces
{
    public interface IBusRepository
    {
        Task<List<BusSchedule>> SearchBusesAsync(string from, string to, DateTime journeyDate);
    }
}
