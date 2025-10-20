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
            if (input.SeatIds == null || input.SeatIds.Count == 0)
                return new BookSeatResultDto { Success = false, Message = "No seat selected for booking." };

            var bookedSeats = new List<string>();
            Passenger? passenger = await _bookingRepository.FindPassengerByMobileAsync(input.PassengerMobile);

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

            try
            {
                foreach (var seatId in input.SeatIds)
                {
                    var seat = await _seatRepository.GetByIdAsync(seatId);
                    if (seat == null)
                        throw new InvalidOperationException($"Seat not found (ID: {seatId}).");

                    seat.Book();

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

                    bookedSeats.Add(seat.SeatNumber);
                }

                // Commit once for all bookings
                await _unitOfWork.SaveChangesAsync();

                if (input.ConfirmAndSell)
                {
                    // mark all as sold
                    foreach (var seatId in input.SeatIds)
                    {
                        var seat = await _seatRepository.GetByIdAsync(seatId);
                        seat?.MakeSold();
                        if (seat != null) await _seatRepository.UpdateAsync(seat);
                    }
                    await _unitOfWork.SaveChangesAsync();
                }

                return new BookSeatResultDto
                {
                    Success = true,
                    Message = $"Seats booked successfully: {string.Join(", ", bookedSeats)}",
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
