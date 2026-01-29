### ENSIMMÄINEN PROMPTI

Tehtävänämme on toteuttaa yksinkertainen kokoushuoneiden varausrajapinta REST arkkitehtuuria käyttäen. APIn tulee tarjota seuraavat toiminnot:

Varauksen luonti tietylle aikavälille, Varauksen peruutus sekä varauksen katselu jolla voidaan listata kaikki tietyn huoneen varaukset.

Toimintalogiikan kolme pointtia:
Varaukset eivät saa mennä päällekkäin, eli kaksi henkilöä ei voi varata samaa huonetta samaan aikaan.
Varaukset eivät voi sijoittua menneisyyteen
Aloitusajan täytyy olla ennen lopetusaikaa

Tekniset reunaehdot:
Käytössämme on ASP .net Core 10, eli suoritamme työn C# ohjelmointikielellä
Käytetään muistinvaraista tietokantaa.

### ENSIMMÄINEN VASTAUS

Se kuulostaa hyvältä harjoitustehtävältä! Toteutetaan siis yksinkertainen kokoushuoneiden varausrajapinta ASP.NET Core 10:llä ja muistinvaraisella tietokannalla. Käymme läpi tarvittavat osat: malli, palvelu, kontrolleri ja validointi.

1. Malli: MeetingRoomBooking
Luodaan malli, joka kuvaa yhtä varauskertaa:
csharp
Copy

public class MeetingRoomBooking
{
    public int Id { get; set; }
    public string RoomName { get; set; }
    public string BookedBy { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}


2. Muistinvarainen tietokanta
Käytetään List<MeetingRoomBooking>:ia muistinvaraisena tietokantana:
csharp
Copy

public static class InMemoryDatabase
{
    public static List<MeetingRoomBooking> Bookings = new List<MeetingRoomBooking>();
}


3. Validointilogiikka
Toteutetaan validointilogiikka, joka tarkistaa:

Varaukset eivät mene päällekkäin
Varaukset eivät sijoitu menneisyyteen
Aloitusajan täytyy olla ennen lopetusaikaa
csharp
Copy

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


4. Kontrolleri: MeetingRoomBookingsController
Luodaan API-kontrolleri, joka tarjoaa seuraavat toiminnot:

Varauksen luonti (POST /api/bookings)
Varauksen peruutus (DELETE /api/bookings/{id})
Varauksen katselu (GET /api/bookings/{roomName})
csharp
Copy

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


5. Testaus
Voit testata API:a esimerkiksi Postmanilla tai Swaggerilla. Esimerkkipyynnöt:


Varauksen luonti:
http
Copy

POST /api/bookings
{
    "roomName": "Huone 1",
    "bookedBy": "Ykä",
    "startTime": "2026-01-30T10:00:00",
    "endTime": "2026-01-30T11:00:00"
}



Varauksen peruutus:
http
Copy

DELETE /api/bookings/1



Varauksen katselu:
http
Copy

GET /api/bookings/Huone 1


Haluatko, että käymme läpi jotain osaa tarkemmin tai lisätään jotain toiminnallisuutta?


### TOINEN PROMPTI
<Aloitin uuden keskustelun tekoälyn kanssa, jotta tämä unohtaisi edelliset ohjeet täysin, eikä menisi sekaisin tekemistäni muutoksista>
Tässä on projektini lähdekoodin tiedostot <Lisäsin tiedostot tekoälylle>.
Tässä on tehtävänanto:
Tehtävänämme on toteuttaa yksinkertainen kokoushuoneiden varausrajapinta REST arkkitehtuuria käyttäen. APIn tulee tarjota seuraavat toiminnot:

Varauksen luonti tietylle aikavälille, Varauksen peruutus sekä varauksen katselu jolla voidaan listata kaikki tietyn huoneen varaukset.

Toimintalogiikan kolme pointtia:
Varaukset eivät saa mennä päällekkäin, eli kaksi henkilöä ei voi varata samaa huonetta samaan aikaan.
Varaukset eivät voi sijoittua menneisyyteen
Aloitusajan täytyy olla ennen lopetusaikaa

Tekniset reunaehdot:
Käytössämme on ASP .net Core 10, eli suoritamme työn C# ohjelmointikielellä
Käytetään muistinvaraista tietokantaa.

Anna palautetta lähdekoodista, ja etsi sieltä myös kehityskohteita.


### TOINEN VASTAUS

Teknisesti koodisi on erinomaisella tasolla. Se on säieturvallinen, arkkitehtuuriltaan puhdas ja noudattaa .NET-maailman parhaita käytäntöjä (DI, Singleton).

Jos haluamme hioa projektia, voimme tehdä myös syötteiden validoinnin (Input Validation) vielä yhdellä pienellä koodimuutoksella.


Koodihiominen: Syötteiden validointi
Tällä hetkellä, jos joku lähettää API:in varauksen ilman huoneen nimeä (roomName: null tai tyhjä), koodi saattaa kaatua tai sallia "nimetön huone" -varauksen.

Ammattilainen estää tämän Data Annotations -merkinnöillä MeetingRoomBooking-mallissa.

Päivitä MeetingRoomBooking.cs
Lisäämme [Required]-attribuutit. Näin ASP.NET Core tarkistaa automaattisesti, että tiedot ovat mukana, ja palauttaa 400 Bad Request jos ne puuttuvat – ennen kuin koodi edes menee kontrolleriin.

C#
using System.ComponentModel.DataAnnotations;

namespace varausPinta.Models
{
    public class MeetingRoomBooking
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Huoneen nimi on pakollinen tieto.")]
        public string RoomName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Varaajan nimi on pakollinen tieto.")]
        public string BookedBy { get; set; } = string.Empty;

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}