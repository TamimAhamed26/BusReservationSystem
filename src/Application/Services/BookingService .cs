using Application.Contracts.DTOs;
using Application.Contracts.Interfaces;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IBusRepository _busRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(
            ISeatRepository seatRepository,
            IBookingRepository bookingRepository,
            IBusRepository busRepository,
            IUnitOfWork unitOfWork)
        {
            _seatRepository = seatRepository;
            _bookingRepository = bookingRepository;
            _busRepository = busRepository;
            _unitOfWork = unitOfWork;
        }

        // ---- Get Seat Plan ----
        public async Task<SeatPlanDto> GetSeatPlanAsync(Guid busScheduleId)
        {
            var seats = await _seatRepository.GetSeatsByScheduleAsync(busScheduleId);
            if (seats == null || seats.Count == 0)
                throw new KeyNotFoundException("No seats found for this schedule.");

            var schedule = seats.First().BusSchedule!;
            var dto = new SeatPlanDto
            {
                BusScheduleId = schedule.Id,
                BusName = schedule.Bus?.BusName ?? string.Empty,
                CompanyName = schedule.Bus?.CompanyName ?? string.Empty,
                FromCity = schedule.Route?.FromCity ?? string.Empty,
                ToCity = schedule.Route?.ToCity ?? string.Empty,
                JourneyDate = schedule.JourneyDate,
                Seats = seats.Select(s => new SeatDto
                {
                    SeatId = s.Id,
                    SeatNumber = s.SeatNumber,
                    Row = s.Row,
                    Column = s.Column,
                    Status = s.Status.ToString()
                }).ToList()
            };
            return dto;
        }

        // ---- Book Seat ----
        public async Task<BookSeatResultDto> BookSeatAsync(BookSeatInputDto input)
        {
            // 1. Validate seat
            var seat = await _seatRepository.GetByIdAsync(input.SeatId);
            if (seat == null)
                return new BookSeatResultDto { Success = false, Message = "Seat not found." };

            try
            {
                // 2. Use domain logic to book
                seat.Book();

                // 3. Find or create passenger
                var passenger = await _bookingRepository.FindPassengerByMobileAsync(input.PassengerMobile);
                if (passenger == null)
                {
                    passenger = new Passenger
                    {
                        Id = Guid.NewGuid(),
                        Name = input.PassengerName,
                        Mobile = input.PassengerMobile
                    };
                    await _bookingRepository.AddPassengerAsync(passenger);
                }

                // 4. Create ticket
                var ticket = new Ticket
                {
                    Id = Guid.NewGuid(),
                    PassengerId = passenger.Id,
                    SeatId = seat.Id,
                    BookingDate = DateTime.UtcNow,
                    BoardingPoint = input.BoardingPoint,
                    DroppingPoint = input.DroppingPoint
                };
                seat.Ticket = ticket;

                await _bookingRepository.AddTicketAsync(ticket);
                await _seatRepository.UpdateAsync(seat);

                // 5. Commit transaction
                await _unitOfWork.SaveChangesAsync();

                // 6. Optionally mark sold (e.g., confirmed payment)
                seat.MakeSold();
                await _seatRepository.UpdateAsync(seat);
                await _unitOfWork.SaveChangesAsync();

                return new BookSeatResultDto
                {
                    Success = true,
                    Message = $"Seat {seat.SeatNumber} booked successfully!",
                    TicketId = ticket.Id,
                    SeatNumber = seat.SeatNumber,
                    PassengerName = passenger.Name
                };
            }
            catch (InvalidOperationException ex)
            {
                return new BookSeatResultDto { Success = false, Message = ex.Message };
            }
        }
    }
}
