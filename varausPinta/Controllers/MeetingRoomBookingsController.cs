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

        //UUSI: Hae varaus ID:llä (Tarvitaan REST-yhteensopivuuteen)
        //Rajoite :int varmistaa, että tämä erottuu huoneen nimellä hausta
        [HttpGet("{id:int}")]
        public IActionResult GetBookingById(int id)
        {
            var booking = _bookingService.GetBookingById(id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        //Varauksen katselu huoneen nimellä
        [HttpGet("{roomName}")]
        public IActionResult GetBookings(string roomName)
        {
            var bookings = _bookingService.GetBookingsForRoom(roomName);
            return Ok(bookings);
        }

        //Varauksen luonti
        [HttpPost]
        public IActionResult CreateBooking(MeetingRoomBooking booking)
        {
            //Tarkistetaan mallin validointisäännöt (esim. [Required]-kentät)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdBooking = _bookingService.CreateBooking(booking);

            if (createdBooking == null)
            {
                return BadRequest("Varaus ei ole kelvollinen. Tarkista ajat ja päällekkäisyydet.");
            }

            //REST-PARANNUS: Palautetaan 201 Created ja Location-header
            return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, createdBooking);
        }

        //Varauksen peruutus
        [HttpDelete("{id}")]
        public IActionResult CancelBooking(int id)
        {
            var isDeleted = _bookingService.CancelBooking(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent(); //204 No Content on standardi onnistuneelle poistolle
        }
    }
}