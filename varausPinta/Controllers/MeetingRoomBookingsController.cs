using Microsoft.AspNetCore.Mvc;
using varausPinta.Models;
using varausPinta.Services;

namespace varausPinta.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class MeetingRoomBookingsController : ControllerBase
    {
        private readonly BookingService _bookingService;

        //Injektoidaan riippuvuus, framework syöttää BookingServicen tähän
        public MeetingRoomBookingsController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public IActionResult CreateBooking(MeetingRoomBooking booking)
        {
            var createdBooking = _bookingService.CreateBooking(booking);

            if (createdBooking == null)
            {
                return BadRequest("Varaus ei ole kelvollinen. Tarkista ajat ja päällekkäisyydet.");
            }

            return Ok(createdBooking);
        }

        [HttpDelete("{id}")]
        public IActionResult CancelBooking(int id)
        {
            var isDeleted = _bookingService.CancelBooking(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("{roomName}")]
        public IActionResult GetBookings(string roomName)
        {
            var bookings = _bookingService.GetBookingsForRoom(roomName);
            return Ok(bookings);
        }
    }
}