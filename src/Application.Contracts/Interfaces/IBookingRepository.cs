using Domain.Entities;

namespace Application.Contracts.Interfaces
{
    public interface IBookingRepository
    {
        Task AddTicketAsync(Ticket ticket);
        Task AddPassengerAsync(Passenger passenger);
        Task<Passenger?> FindPassengerByMobileAsync(string mobile);
    }
}
