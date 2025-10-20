using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Seed
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            
            // Check if data already exists
            if (context.Buses.Any())
            {
                return; // DB has been seeded
            }

            // --- Seed Data ---

            // 1. Routes
            var routeDhakaRaj = new Route { Id = Guid.NewGuid(), FromCity = "Dhaka", ToCity = "Rajshahi" };
            var routeDhakaChi = new Route { Id = Guid.NewGuid(), FromCity = "Dhaka", ToCity = "Chittagong" };

            // 2. Buses
            var busNational = new Bus { Id = Guid.NewGuid(), CompanyName = "National Travels", BusName = "99-DHA-CHA (NON AC)", TotalSeats = 40 };
            var busHanif = new Bus { Id = Guid.NewGuid(), CompanyName = "Hanif Enterprise", BusName = "68-RAJ-CHAPA (NON AC)", TotalSeats = 40 };

            // 3. Schedules (using a date from the UI)
            var journeyDate = new DateTime(2025, 10, 23);

            var scheduleNational = new BusSchedule
            {
                Id = Guid.NewGuid(),
                Bus = busNational,
                Route = routeDhakaRaj,
                JourneyDate = journeyDate,
                StartTime = new TimeSpan(6, 0, 0), // 6:00 AM
                ArrivalTime = new TimeSpan(13, 30, 0), // 1:30 PM
                Price = 700
            };

            var scheduleHanif = new BusSchedule
            {
                Id = Guid.NewGuid(),
                Bus = busHanif,
                Route = routeDhakaRaj,
                JourneyDate = journeyDate,
                StartTime = new TimeSpan(6, 0, 0), // 06:00 AM
                ArrivalTime = new TimeSpan(13, 0, 0), // Assumed arrival
                Price = 700
            };
            
            // 4. Seats for National Travels
            var seatsNational = GenerateSeatsForSchedule(scheduleNational.Id, 40);

            // 5. Seats for Hanif Enterprise
            var seatsHanif = GenerateSeatsForSchedule(scheduleHanif.Id, 40);

            // --- Add to Context ---
            context.Routes.AddRange(routeDhakaRaj, routeDhakaChi);
            context.Buses.AddRange(busNational, busHanif);
            context.BusSchedules.AddRange(scheduleNational, scheduleHanif);
            context.Seats.AddRange(seatsNational);
            context.Seats.AddRange(seatsHanif);

            context.SaveChanges();
        }

      
        /// Helper method to generate a standard 40-seat (10x4) layout.
       
        private static List<Seat> GenerateSeatsForSchedule(Guid scheduleId, int totalSeats)
        {
            var seats = new List<Seat>();
            for (int i = 0; i < totalSeats; i++)
            {
                int row = i / 4;
                int col = i % 4;
                string seatNumber = $"{(char)('A' + row)}{col + 1}";

                seats.Add(new Seat(seatNumber, row + 1, col + 1, scheduleId, SeatStatus.Available));
            }
            return seats;
        }

    }
}