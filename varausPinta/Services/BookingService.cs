using varausPinta.Models;

namespace varausPinta.Services
{
    public class BookingService
    {
        //Kapseloidaan data, lista on nyt privaatti ttei kukaan ulkopuolinen voi sorkkia sitä suoraan
        private readonly List<MeetingRoomBooking> _bookings = new();

        //ID-laskuri
        private int _nextId = 1;

        public List<MeetingRoomBooking> GetBookingsForRoom(string roomName)
        {
            return _bookings
                .Where(b => b.RoomName.Equals(roomName, StringComparison.OrdinalIgnoreCase)) //Lisätty kirjainkokoriippumattomuus
                .ToList();
        }

        public MeetingRoomBooking? CreateBooking(MeetingRoomBooking newBooking)
        {
            if (!IsValidBooking(newBooking))
            {
                return null;
            }

            //Generoidaan ID palvelimella
            newBooking.Id = _nextId++;

            _bookings.Add(newBooking);
            return newBooking;
        }

        //Perutaan varaus (palauttaa false, jos ei löydy)
        public bool CancelBooking(int id)
        {
            var booking = _bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null)
            {
                return false;
            }

            _bookings.Remove(booking);
            return true;
        }

        //Yksityinen apumetodi
        private bool IsValidBooking(MeetingRoomBooking newBooking)
        {
            if (newBooking.StartTime >= newBooking.EndTime) return false;

            //Käytetään UTC-aikaa vertailuissa
            if (newBooking.StartTime < DateTime.UtcNow) return false;

            foreach (var booking in _bookings)
            {
                //Ohitetaan eri huoneet
                if (!booking.RoomName.Equals(newBooking.RoomName, StringComparison.OrdinalIgnoreCase))
                    continue;

                //Tarkistetaan päällekkäisyys
                if (newBooking.StartTime < booking.EndTime && newBooking.EndTime > booking.StartTime)
                    return false;
            }
            return true;
        }
    }
}