using Application.Contracts.DTOs;
using Application.Contracts.Interfaces;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly IBusRepository _busRepository;

        public SearchService(IBusRepository busRepository)
        {
            _busRepository = busRepository;
        }

        public async Task<List<AvailableBusDto>> SearchAvailableBusesAsync(string from, string to, DateTime journeyDate)
        {
            var schedules = await _busRepository.SearchBusesAsync(from, to, journeyDate);
            var result = new List<AvailableBusDto>();

            foreach (var schedule in schedules)
            {
                int totalSeats = schedule.Bus?.TotalSeats ?? 0;
                int bookedSeats = schedule.Seats.Count(s => s.Status != SeatStatus.Available);
                int seatsLeft = totalSeats - bookedSeats;

                result.Add(new AvailableBusDto
                {
                    BusScheduleId = schedule.Id,
                    CompanyName = schedule.Bus?.CompanyName ?? string.Empty,
                    BusName = schedule.Bus?.BusName ?? string.Empty,
                    FromCity = schedule.Route?.FromCity ?? string.Empty,
                    ToCity = schedule.Route?.ToCity ?? string.Empty,
                    JourneyDate = schedule.JourneyDate,
                    StartTime = schedule.StartTime,
                    ArrivalTime = schedule.ArrivalTime,
                    SeatsLeft = seatsLeft,
                    Price = schedule.Price
                });
            }

            return result;
        }
    }
}
