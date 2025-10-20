using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        /// <summary>
        /// Search available buses by route and journey date.
        /// </summary>
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableBuses(
            [FromQuery] string from,
            [FromQuery] string to,
            [FromQuery] DateTime journeyDate)
        {
            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
                return BadRequest("Both 'from' and 'to' must be provided.");

            var result = await _searchService.SearchAvailableBusesAsync(from, to, journeyDate);
            if (result == null || !result.Any())
                return NotFound("No buses found for the given criteria.");

            return Ok(result);
        }
    }
}
