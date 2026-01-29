using varausPinta.Models;

namespace varausPinta.Services
{
    public class BookingService
    {
        public bool IsValidBooking(MeetingRoomBooking newBooking)
        {
            // 1. Aloitusajan täytyy olla ennen lopetusaikaa
            if (newBooking.StartTime >= newBooking.EndTime)
                return false;

            // 2. Varaukset eivät saa sijoittua menneisyyteen
            if (newBooking.StartTime < DateTime.Now)
                return false;

            // 3. Varaukset eivät saa mennä päällekkäin
            foreach (var booking in InMemoryDatabase.Bookings)
            {
                if (booking.RoomName == newBooking.RoomName)
                {
                    if (newBooking.StartTime < booking.EndTime && newBooking.EndTime > booking.StartTime)
                        return false;
                }
            }

            return true;
        }

    }
}
