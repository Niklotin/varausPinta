using varausPinta.Models;

namespace varausPinta.Services
{
    public class BookingService
    {
        //Kapseloidaan data, lista on nyt privaatti ttei kukaan ulkopuolinen voi sorkkia sitä suoraan
        private readonly List<MeetingRoomBooking> _bookings = new();

        //ID-laskuri
        private int _nextId = 1;

        //Lisätään säielukko ettei API kyykähdä jos sille tehdään samanaikaisia muokkauksia
        private readonly object _lock = new();

        public List<MeetingRoomBooking> GetBookingsForRoom(string roomName)
        {
            //Lukitaan myös lukuhetkellä jottei lista muutu kesken lukemisen
            lock (_lock)
            {
                return _bookings
                    .Where(b => b.RoomName.Equals(roomName, StringComparison.OrdinalIgnoreCase))
                    .ToList(); // .ToList() luo uuden kopion, joten lukitus voidaan vapauttaa heti palautuksen jälkeen
            }
        }

        public MeetingRoomBooking? CreateBooking(MeetingRoomBooking newBooking)
        {
            //Lukitaan koko "tarkista ja tallenna" operaatio
            lock (_lock)
            {
                if (!IsValidBooking(newBooking))
                {
                    return null;
                }

                newBooking.Id = _nextId++;
                _bookings.Add(newBooking);

                return newBooking;
            }
        }

        //Perutaan varaus (palauttaa false, jos ei löydy)
        public bool CancelBooking(int id)
        {
            lock (_lock)
            {
                var booking = _bookings.FirstOrDefault(b => b.Id == id);
                if (booking == null)
                {
                    return false;
                }

                _bookings.Remove(booking);
                return true;
            }
        }

        //Yksityinen apumetodi.
        private bool IsValidBooking(MeetingRoomBooking newBooking)
        {
            if (newBooking.StartTime >= newBooking.EndTime) return false;
            if (newBooking.StartTime < DateTime.UtcNow) return false;

            foreach (var booking in _bookings)
            {
                if (!booking.RoomName.Equals(newBooking.RoomName, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (newBooking.StartTime < booking.EndTime && newBooking.EndTime > booking.StartTime)
                    return false;
            }
            return true;
        }
    }
}