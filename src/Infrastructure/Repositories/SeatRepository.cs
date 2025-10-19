using Application.Contracts.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly AppDbContext _context;

        public SeatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Seat>> GetSeatsByScheduleAsync(Guid busScheduleId)
        {
            return await _context.Seats
                .Include(s => s.BusSchedule)!.ThenInclude(b => b.Bus)
                .Include(s => s.BusSchedule)!.ThenInclude(b => b.Route)
                .Where(s => s.BusScheduleId == busScheduleId)
                .OrderBy(s => s.Row).ThenBy(s => s.Column)
                .ToListAsync();
        }

        public async Task<Seat?> GetByIdAsync(Guid seatId)
        {
            return await _context.Seats
                .Include(s => s.BusSchedule)!.ThenInclude(b => b.Bus)
                .Include(s => s.BusSchedule)!.ThenInclude(b => b.Route)
                .FirstOrDefaultAsync(s => s.Id == seatId);
        }

        public async Task UpdateAsync(Seat seat)
        {
            _context.Seats.Update(seat);
            await Task.CompletedTask;
        }
    }
}
