using Application.Contracts.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BusRepository : IBusRepository
    {
        private readonly AppDbContext _context;

        public BusRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BusSchedule>> SearchBusesAsync(string from, string to, DateTime journeyDate)
        {
            return await _context.BusSchedules
                .Include(s => s.Bus)
                .Include(s => s.Route)
                .Include(s => s.Seats)
                .Where(s =>
                    s.Route!.FromCity.ToLower() == from.ToLower() &&
                    s.Route.ToCity.ToLower() == to.ToLower() &&
                    s.JourneyDate.Date == journeyDate.Date)
                .ToListAsync();
        }
    }
}
