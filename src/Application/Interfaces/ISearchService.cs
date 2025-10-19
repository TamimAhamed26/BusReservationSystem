using Application.Contracts.DTOs;

namespace Application.Interfaces
{
    public interface ISearchService
    {
        Task<List<AvailableBusDto>> SearchAvailableBusesAsync(string from, string to, DateTime journeyDate);
    }
}
