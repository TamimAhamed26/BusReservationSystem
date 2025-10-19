using Application.Contracts.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddTicketAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
        }

        public async Task AddPassengerAsync(Passenger passenger)
        {
            await _context.Passengers.AddAsync(passenger);
        }

        public async Task<Passenger?> FindPassengerByMobileAsync(string mobile)
        {
            return await _context.Passengers.FirstOrDefaultAsync(p => p.Mobile == mobile);
        }
    }
}
