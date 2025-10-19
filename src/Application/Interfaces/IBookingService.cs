using Application.Contracts.DTOs;

namespace Application.Interfaces
{
    public interface IBookingService
    {
        Task<SeatPlanDto> GetSeatPlanAsync(Guid busScheduleId);
        Task<BookSeatResultDto> BookSeatAsync(BookSeatInputDto input);
    }
}
