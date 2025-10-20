using Application.Contracts.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Get seat plan layout for a given bus schedule.
        /// </summary>
        [HttpGet("seatplan/{busScheduleId}")]
        public async Task<IActionResult> GetSeatPlan(Guid busScheduleId)
        {
            try
            {
                var result = await _bookingService.GetSeatPlanAsync(busScheduleId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Book a seat for a given schedule.
        /// </summary>
        [HttpPost("book")]
        public async Task<IActionResult> BookSeat([FromBody] BookSeatInputDto input)
        {
            if (input == null)
                return BadRequest("Booking details are required.");

            var result = await _bookingService.BookSeatAsync(input);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
