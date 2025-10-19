using Domain.Entities;

namespace Application.Contracts.Interfaces
{
    public interface ISeatRepository
    {
        Task<List<Seat>> GetSeatsByScheduleAsync(Guid busScheduleId);
        Task<Seat?> GetByIdAsync(Guid seatId);
        Task UpdateAsync(Seat seat);
    }
}
