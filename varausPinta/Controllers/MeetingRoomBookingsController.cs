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

        public MeetingRoomBookingsController()
        {
            _bookingService = new BookingService();
        }

        // Varauksen luonti
        [HttpPost]
        public IActionResult CreateBooking(MeetingRoomBooking booking)
        {
            if (!_bookingService.IsValidBooking(booking))
                return BadRequest("Varaus ei ole kelvollinen. Tarkista ajat ja päällekkäisyydet.");

            InMemoryDatabase.Bookings.Add(booking);
            return Ok(booking);
        }

        // Varauksen peruutus
        [HttpDelete("{id}")]
        public IActionResult CancelBooking(int id)
        {
            var booking = InMemoryDatabase.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null)
                return NotFound();

            InMemoryDatabase.Bookings.Remove(booking);
            return Ok();
        }

        // Varauksen katselu
        [HttpGet("{roomName}")]
        public IActionResult GetBookings(string roomName)
        {
            var bookings = InMemoryDatabase.Bookings.Where(b => b.RoomName == roomName).ToList();
            return Ok(bookings);
        }
    }

}
