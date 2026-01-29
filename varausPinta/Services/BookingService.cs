using varausPinta.Models;

namespace varausPinta.Services
{
    public class BookingService
    {
        //Haetaan huoneen varaukset
        public List<MeetingRoomBooking> GetBookingsForRoom(string roomName)
        {
            return InMemoryDatabase.Bookings
                .Where(b => b.RoomName == roomName)
                .ToList();
        }

        //Luodaan varaus (palauttaa null, jos epäonnistuu)
        public MeetingRoomBooking? CreateBooking(MeetingRoomBooking newBooking)
        {
            if (!IsValidBooking(newBooking))
            {
                return null;
            }

            //Tästä puuttuu vielä ID generointi, mutta hoidetaan refaktorointi askel kerrallaan, tässä vaiheessa siirretään vastuita joten palataan asiaan.
            InMemoryDatabase.Bookings.Add(newBooking);
            return newBooking;
        }

        //Perutaan varaus (palauttaa false, jos ei löydy)
        public bool CancelBooking(int id)
        {
            var booking = InMemoryDatabase.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null)
            {
                return false;
            }

            InMemoryDatabase.Bookings.Remove(booking);
            return true;
        }

        //Yksityinen apumetodi
        private bool IsValidBooking(MeetingRoomBooking newBooking)
        {
            if (newBooking.StartTime >= newBooking.EndTime) return false;
            if (newBooking.StartTime < DateTime.Now) return false;

            foreach (var booking in InMemoryDatabase.Bookings)
            {
                if (booking.RoomName == newBooking.RoomName)
                {
                    // Tarkistetaan päällekkäisyys
                    if (newBooking.StartTime < booking.EndTime && newBooking.EndTime > booking.StartTime)
                        return false;
                }
            }
            return true;
        }
    }
}