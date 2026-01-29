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



### KOLMAS PROMPTI
Projektin toteutus on tällä hetkellä ns "pragmaattinen REST", mitkä asiat korjaamalla saamme APIn noudattamaan täydellisesti REST arkkitehtuuria


### KOLMAS VASTAUS
Tämä on erinomainen kysymys.

Miksi se on REST?
Resurssipohjaisuus: Käsittelet "resursseja" (varauksia) URL-osoitteessa /api/bookings.

HTTP-verbit: Käytät oikeita verbejä oikeisiin asioihin:

GET tietojen hakuun.

POST uuden luomiseen.

DELETE poistamiseen.

Stateless (Tilaton): Jokainen pyyntö sisältää kaiken tarvittavan tiedon, eikä palvelin muista edellistä HTTP-pyyntöä (vaikka data tallentuukin).

JSON: Kommunikaatio tapahtuu standardissa JSON-muodossa.

Miten teemme siitä "Senior-tason" REST APIn?
Nykyisessä koodissa on yksi kauneusvirhe, joka liittyy HTTP-statuskoodeihin.

Kun luot uuden varauksen (CreateBooking), palautat tällä hetkellä koodin 200 OK. REST-standardin mukaan uuden resurssin luomisen pitäisi palauttaa 201 Created. Lisäksi vastauksen tulisi sisältää Location-header, joka kertoo mistä uusi varaus löytyy.

Korjataan tämä? Se vaatii kaksi pientä muutosta:

Tarvitsemme metodin yhden varauksen hakemiseen ID:llä (jotta voimme viitata siihen).

Muutamme POST-metodin palautusarvon.

Vaihe 1: Lisää "Hae yksi" -metodi Serviceen ja Kontrolleriin
Lisää ensin tämä BookingService.cs -tiedostoon:

C#
// Lisää tämä BookingService-luokkaan
public MeetingRoomBooking? GetBookingById(int id)
{
    lock (_lock)
    {
        return _bookings.FirstOrDefault(b => b.Id == id);
    }
}
Vaihe 2: Päivitä Kontrolleri (Tärkeä REST-korjaus)
Muokataan MeetingRoomBookingsController.cs. Lisätään sinne GetById-endpoint ja päivitetään CreateBooking palauttamaan CreatedAtAction.

C#
// Lisää tämä uusi endpoint kontrolleriin
[HttpGet("{id:int}")] // Huom: lisätty :int erottamaan se roomName-hausta
public IActionResult GetBookingById(int id)
{
    var booking = _bookingService.GetBookingById(id);
    if (booking == null) return NotFound();
    return Ok(booking);
}

// PÄIVITÄ CreateBooking-metodi tällaiseksi:
[HttpPost]
public IActionResult CreateBooking(MeetingRoomBooking booking)
{
    // ... validointi pysyy samana ...
    if (!ModelState.IsValid) return BadRequest(ModelState); // Hyödyntää aiempaa [Required] muutosta

    var createdBooking = _bookingService.CreateBooking(booking);
    
    if (createdBooking == null)
    {
        return BadRequest("Varaus ei ole kelvollinen. Tarkista ajat ja päällekkäisyydet.");
    }

    // REST-KORJAUS: Palautetaan 201 Created ja Location-header
    // Tämä kertoo: "Onnistui, ja uusi tieto löytyy osoitteesta /api/bookings/{id}"
    return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, createdBooking);
}
Miksi tämä kannattaa tehdä?
Tämä on juuri sellainen yksityiskohta, josta senior-kehittäjät nyökyttelevät hyväksyvästi.

201 Created on täsmällisempi kuin 200 OK.

Location-header auttaa asiakassovellusta (client) tietämään heti, mikä uuden varauksen ID on ja mistä se löytyy.

GetBookingById on muutenkin hyödyllinen olla olemassa.
