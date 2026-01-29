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


### NELJÄS PROMPTI
Kirjoita vielä skripti jolla saamme kokeiltua kaikkia APIn toimintoja, koska emme tiedä saammeko käyttää testauskirjastoja, niin kirjoita myös muutama pyyntö joilla voimme testata APIn tarkoituksellista toiminnallisuutta


### NELJÄS VASTAUS

Koska emme halua lisätä projektiin raskaita testauskirjastoja (kuten xUnit tai NUnit) tässä vaiheessa, paras tapa on luoda yksinkertainen PowerShell-skripti. Se on luettava, toimii Windowsissa suoraan ja sillä voidaan testata dynaamisesti (esim. laskea huomisen päivämäärä, jotta "menneisyys"-tarkistus ei estä testiä).

Tämä skripti toimii ikään kuin "köyhän miehen integraatiotesti". Se käy läpi onnistuneet tapaukset sekä tahalliset virhetilanteet (reunaehdot).

Vaihe 1: Selvitä API:n osoite
Tarkista Visual Studiosta, missä portissa sovelluksesi pyörii (esim. http://localhost:5123 tai https://localhost:7123). Löydät sen Properties/launchSettings.json -tiedostosta tai kun käynnistät sovelluksen selaimessa.

Vaihe 2: Luo testiskripti
Luo projektin juureen tiedosto nimeltä test_api.ps1 ja kopioi sinne alla oleva koodi. Muista muuttaa $baseUrl-muuttuja vastaamaan omaa porttiasi!

PowerShell
# Asetukset: MUUTA TÄMÄ VASTAAMAAN OMAA PORTTIASI
$baseUrl = "http://localhost:5276" 
$apiUrl = "$baseUrl/api/bookings"

# Apufunktio tulostukseen
function Print-Result ($testName, $status, $details = "") {
    if ($status -eq "PASS") {
        Write-Host "[PASS] $testName" -ForegroundColor Green
    } else {
        Write-Host "[FAIL] $testName" -ForegroundColor Red
        if ($details) { Write-Host "       $details" -ForegroundColor Gray }
    }
}

# Lasketaan ajat dynaamisesti, jotta testi toimii aina
$today = Get-Date
$tomorrowStart = $today.AddDays(1).ToString("yyyy-MM-ddTHH:00:00")
$tomorrowEnd = $today.AddDays(1).ToString("yyyy-MM-ddTHH:30:00")
$pastDate = $today.AddDays(-1).ToString("yyyy-MM-ddTHH:00:00")

Write-Host "--- ALOITETAAN API-TESTAUS ($baseUrl) ---" -ForegroundColor Cyan

# ---------------------------------------------------------
# TESTI 1: Luodaan validi varaus (Happy Path)
# ---------------------------------------------------------
$body = @{
    roomName = "Neukkari 1"
    bookedBy = "Testaaja Teppo"
    startTime = $tomorrowStart
    endTime = $tomorrowEnd
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod -Uri $apiUrl -Method Post -Body $body -ContentType "application/json" -ErrorAction Stop
    
    if ($response.id -gt 0 -and $response.roomName -eq "Neukkari 1") {
        Print-Result "Validin varauksen luonti" "PASS"
        $createdId = $response.id
    } else {
        Print-Result "Validin varauksen luonti" "FAIL" "Ei palauttanut ID:tä"
    }
} catch {
    $errorMsg = $_.Exception.Message
    
    # Yritetään lukea tarkempi syy vastauksen rungosta (jos sellainen on)
    if ($_.Exception.Response) {
        $stream = $_.Exception.Response.GetResponseStream()
        if ($stream) {
            $reader = New-Object System.IO.StreamReader($stream)
            $detailedError = $reader.ReadToEnd()
            $errorMsg = "$errorMsg -> SERVERIN VASTAUS: $detailedError"
        }
    }

    Print-Result "Validin varauksen luonti" "FAIL" $errorMsg
    exit # Lopetetaan tähän, koska ilman luontia muut testit eivät toimi
}
}

# ---------------------------------------------------------
# TESTI 2: Estä päällekkäinen varaus (Business Logic)
# ---------------------------------------------------------
# Yritetään varata sama huone samalle ajalle
try {
    Invoke-RestMethod -Uri $apiUrl -Method Post -Body $body -ContentType "application/json" -ErrorAction Stop
    Print-Result "Päällekkäisyyden esto" "FAIL" "API salli päällekkäisen varauksen (olisi pitänyt tulla 400)"
} catch {
    if ($_.Exception.Response.StatusCode -eq [System.Net.HttpStatusCode]::BadRequest) {
        Print-Result "Päällekkäisyyden esto" "PASS"
    } else {
        Print-Result "Päällekkäisyyden esto" "FAIL" "Väärä virhekoodi: $($_.Exception.Response.StatusCode)"
    }
}

# ---------------------------------------------------------
# TESTI 3: Estä menneisyys (Business Logic)
# ---------------------------------------------------------
$pastBody = @{
    roomName = "Neukkari 1"
    bookedBy = "Martti McFly"
    startTime = $pastDate
    endTime = $today.ToString("yyyy-MM-ddTHH:00:00")
} | ConvertTo-Json

try {
    Invoke-RestMethod -Uri $apiUrl -Method Post -Body $pastBody -ContentType "application/json" -ErrorAction Stop
    Print-Result "Menneisyyden esto" "FAIL" "API salli varauksen menneisyyteen"
} catch {
    if ($_.Exception.Response.StatusCode -eq [System.Net.HttpStatusCode]::BadRequest) {
        Print-Result "Menneisyyden esto" "PASS"
    } else {
        Print-Result "Menneisyyden esto" "FAIL"
    }
}

# ---------------------------------------------------------
# TESTI 4: Estä Start > End (Business Logic)
# ---------------------------------------------------------
$invertedBody = @{
    roomName = "Neukkari 1"
    bookedBy = "Sekoilija"
    startTime = $tomorrowEnd # Alkaa lopusta
    endTime = $tomorrowStart # Loppuu alkuun
} | ConvertTo-Json

try {
    Invoke-RestMethod -Uri $apiUrl -Method Post -Body $invertedBody -ContentType "application/json" -ErrorAction Stop
    Print-Result "Aikajärjestyksen tarkistus" "FAIL" "API salli alkamisajan olevan loppuajan jälkeen"
} catch {
    if ($_.Exception.Response.StatusCode -eq [System.Net.HttpStatusCode]::BadRequest) {
        Print-Result "Aikajärjestyksen tarkistus" "PASS"
    } else {
        Print-Result "Aikajärjestyksen tarkistus" "FAIL"
    }
}

# ---------------------------------------------------------
# TESTI 5: Hae luotu varaus ID:llä (REST toiminnallisuus)
# ---------------------------------------------------------
try {
    $getResponse = Invoke-RestMethod -Uri "$apiUrl/$createdId" -Method Get -ErrorAction Stop
    if ($getResponse.bookedBy -eq "Testaaja Teppo") {
        Print-Result "Varauksen haku ID:llä" "PASS"
    } else {
        Print-Result "Varauksen haku ID:llä" "FAIL" "Palautti väärää dataa"
    }
} catch {
    Print-Result "Varauksen haku ID:llä" "FAIL" $_.Exception.Message
}

# ---------------------------------------------------------
# TESTI 6: Poista varaus (REST toiminnallisuus)
# ---------------------------------------------------------
try {
    Invoke-RestMethod -Uri "$apiUrl/$createdId" -Method Delete -ErrorAction Stop
    # Varmistetaan poisto hakemalla se uudelleen (pitäisi tulla 404)
    try {
        Invoke-RestMethod -Uri "$apiUrl/$createdId" -Method Get -ErrorAction Stop
        Print-Result "Varauksen poisto" "FAIL" "Varaus löytyi vielä poiston jälkeen"
    } catch {
        if ($_.Exception.Response.StatusCode -eq [System.Net.HttpStatusCode]::NotFound) {
            Print-Result "Varauksen poisto" "PASS"
        } else {
            Print-Result "Varauksen poisto" "FAIL" "Poiston jälkeen tuli outo koodi: $($_.Exception.Response.StatusCode)"
        }
    }
} catch {
    Print-Result "Varauksen poisto" "FAIL" "DELETE-komento epäonnistui"
}

Write-Host "--- TESTAUS VALMIS ---" -ForegroundColor Cyan
Miten ajat skriptin?
Käynnistä API Visual Studiosta (vihreä nuoli "Start").

Avaa PowerShell (voit avata sen Visual Studion terminaalista: View -> Terminal).

Varmista, että olet samassa kansiossa kuin test_api.ps1.

Aja komento:

PowerShell
./test_api.ps1
Mitä tämä testaa?
Tämä skripti on erinomainen lisä palautukseen, koska se osoittaa:

Toiminnallisuuden: Create, Read, Delete toimivat.

Logiikan: Se todistaa, että päällekkäisyydet ja aikavirheet estetään (saat vihreät PASS-merkinnät virhetilanteista).

Dokumentaatio: Koodia lukemalla näkee heti, millaista JSON-dataa API odottaa.

### VIIDES PROMPTI
Skriptissä oli virhe koodilla
--- ALOITETAAN API-TESTAUS (http://localhost:5195) ---
[FAIL] Validin varauksen luonti
Etäpalvelin palautti virheen: (400) Virheellinen pyyntö. -> SERVERIN VASTAUS: {"type":"https://tools.ietf.org/html/rfc9110#section-15.5.1","title":"One or more validation errors occurred.","status":400,"errors":{"booking":["The booking field is required."],"$.endTime":["The JSON value could not be converted to System.DateTime. Path: $.endTime | LineNumber: 1 | BytePositionInLine: 37."]},"traceId":"00-b6c9fefe11e0d8750b97410603c829af-7d8ecb34694d5523-00"}

Skriptissä lienee jotain vialla, käy skripti läpi ja korjaa virheilmotuksen virhe.


### VIIDES VASTAUS

Virheviesti on hyvin selkeä: JSON-muunnos epäonnistui päivämäärän kohdalla.

SERVERIN VASTAUS: ... "$.endTime":["The JSON value could not be converted to System.DateTime..."]

Tämä johtuu siitä, että aiemmassa skriptissä käyttämäni päivämäärämuotoilu ToString("yyyy-MM-ddTHH:00:00") oli hieman riskialtis, sillä PowerShell ja .NET saattavat tulkita tuon T-kirjaimen väärin (esim. aikamuotoilijana eikä kirjaimena) riippuen koneesi lokaaliasetuksista.

Korjataan tämä käyttämällä varmaa ISO 8601 -standardimuotoilua ("s"), jonka .NET ymmärtää aina.

Korjaus: Päivitä testiskriptin aikamääritykset
Avaa test_api.ps1 ja korvaa rivit 17–20 (aikojen laskeminen) tällä varmemmalla versiolla:

PowerShell
# KORJATTU AIKAMÄÄRITYS (Käytetään "s"-muotoilua, joka on ISO 8601 -yhteensopiva)
$today = Get-Date
# Huom: ToString("s") tuottaa muodon "2026-01-30T12:00:00"
$tomorrowStart = $today.AddDays(1).ToString("s") 
$tomorrowEnd = $today.AddDays(1).AddMinutes(30).ToString("s")
$pastDate = $today.AddDays(-1).ToString("s")
$todayStr = $today.ToString("s") # Käytetään menneisyystestin loppuaikana
Sinun täytyy myös päivittää TESTI 3 käyttämään tuota $todayStr -muuttujaa, jotta senkin formaatti on oikein.

Tässä on koko korjattu skripti (kopioi tämä kokonaan tiedoston päälle, niin virheet häviävät):

PowerShell
# Asetukset: MUUTA TÄMÄ VASTAAMAAN OMAA PORTTIASI
$baseUrl = "http://localhost:5195" 
$apiUrl = "$baseUrl/api/bookings"

# Apufunktio tulostukseen
function Print-Result ($testName, $status, $details = "") {
    if ($status -eq "PASS") {
        Write-Host "[PASS] $testName" -ForegroundColor Green
    } else {
        Write-Host "[FAIL] $testName" -ForegroundColor Red
        if ($details) { Write-Host "       $details" -ForegroundColor Gray }
    }
}

Write-Host "--- ALOITETAAN API-TESTAUS ($baseUrl) ---" -ForegroundColor Cyan

# KORJATTU AIKAMÄÄRITYS (ISO 8601 -yhteensopiva)
$today = Get-Date
$tomorrowStart = $today.AddDays(1).ToString("s") 
$tomorrowEnd = $today.AddDays(1).AddMinutes(30).ToString("s")
$pastDate = $today.AddDays(-1).ToString("s")
$todayStr = $today.ToString("s") 

# ---------------------------------------------------------
# TESTI 1: Luodaan validi varaus (Happy Path)
# ---------------------------------------------------------
$body = @{
    roomName = "Neukkari 1"
    bookedBy = "Testaaja Teppo"
    startTime = $tomorrowStart
    endTime = $tomorrowEnd
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod -Uri $apiUrl -Method Post -Body $body -ContentType "application/json" -ErrorAction Stop
    
    if ($response.id -gt 0 -and $response.roomName -eq "Neukkari 1") {
        Print-Result "Validin varauksen luonti" "PASS"
        $createdId = $response.id
    } else {
        Print-Result "Validin varauksen luonti" "FAIL" "Ei palauttanut ID:tä"
    }
} catch {
    $errorMsg = $_.Exception.Message
    if ($_.Exception.Response) {
        $stream = $_.Exception.Response.GetResponseStream()
        if ($stream) {
            $reader = New-Object System.IO.StreamReader($stream)
            $detailedError = $reader.ReadToEnd()
            $errorMsg = "$errorMsg -> SERVERIN VASTAUS: $detailedError"
        }
    }
    Print-Result "Validin varauksen luonti" "FAIL" $errorMsg
    exit 
}

# ---------------------------------------------------------
# TESTI 2: Estä päällekkäinen varaus (Business Logic)
# ---------------------------------------------------------
try {
    Invoke-RestMethod -Uri $apiUrl -Method Post -Body $body -ContentType "application/json" -ErrorAction Stop
    Print-Result "Päällekkäisyyden esto" "FAIL" "API salli päällekkäisen varauksen"
} catch {
    if ($_.Exception.Response.StatusCode -eq [System.Net.HttpStatusCode]::BadRequest) {
        Print-Result "Päällekkäisyyden esto" "PASS"
    } else {
        Print-Result "Päällekkäisyyden esto" "FAIL" "Väärä virhekoodi: $($_.Exception.Response.StatusCode)"
    }
}

# ---------------------------------------------------------
# TESTI 3: Estä menneisyys (Business Logic)
# ---------------------------------------------------------
$pastBody = @{
    roomName = "Neukkari 1"
    bookedBy = "Martti McFly"
    startTime = $pastDate
    endTime = $todayStr
} | ConvertTo-Json

try {
    Invoke-RestMethod -Uri $apiUrl -Method Post -Body $pastBody -ContentType "application/json" -ErrorAction Stop
    Print-Result "Menneisyyden esto" "FAIL" "API salli varauksen menneisyyteen"
} catch {
    if ($_.Exception.Response.StatusCode -eq [System.Net.HttpStatusCode]::BadRequest) {
        Print-Result "Menneisyyden esto" "PASS"
    } else {
        Print-Result "Menneisyyden esto" "FAIL"
    }
}

# ---------------------------------------------------------
# TESTI 4: Estä Start > End (Business Logic)
# ---------------------------------------------------------
$invertedBody = @{
    roomName = "Neukkari 1"
    bookedBy = "Sekoilija"
    startTime = $tomorrowEnd 
    endTime = $tomorrowStart 
} | ConvertTo-Json

try {
    Invoke-RestMethod -Uri $apiUrl -Method Post -Body $invertedBody -ContentType "application/json" -ErrorAction Stop
    Print-Result "Aikajärjestyksen tarkistus" "FAIL" "API salli virheelliset ajat"
} catch {
    if ($_.Exception.Response.StatusCode -eq [System.Net.HttpStatusCode]::BadRequest) {
        Print-Result "Aikajärjestyksen tarkistus" "PASS"
    } else {
        Print-Result "Aikajärjestyksen tarkistus" "FAIL"
    }
}

# ---------------------------------------------------------
# TESTI 5: Hae luotu varaus ID:llä (REST toiminnallisuus)
# ---------------------------------------------------------
try {
    $getResponse = Invoke-RestMethod -Uri "$apiUrl/$createdId" -Method Get -ErrorAction Stop
    if ($getResponse.bookedBy -eq "Testaaja Teppo") {
        Print-Result "Varauksen haku ID:llä" "PASS"
    } else {
        Print-Result "Varauksen haku ID:llä" "FAIL" "Palautti väärää dataa"
    }
} catch {
    Print-Result "Varauksen haku ID:llä" "FAIL" $_.Exception.Message
}

# ---------------------------------------------------------
# TESTI 6: Poista varaus (REST toiminnallisuus)
# ---------------------------------------------------------
try {
    Invoke-RestMethod -Uri "$apiUrl/$createdId" -Method Delete -ErrorAction Stop
    
    try {
        Invoke-RestMethod -Uri "$apiUrl/$createdId" -Method Get -ErrorAction Stop
        Print-Result "Varauksen poisto" "FAIL" "Varaus löytyi vielä poiston jälkeen"
    } catch {
        if ($_.Exception.Response.StatusCode -eq [System.Net.HttpStatusCode]::NotFound) {
            Print-Result "Varauksen poisto" "PASS"
        } else {
            Print-Result "Varauksen poisto" "FAIL" "Odotettiin 404, saatiin: $($_.Exception.Response.StatusCode)"
        }
    }
} catch {
    Print-Result "Varauksen poisto" "FAIL" "DELETE-komento epäonnistui"
}

Write-Host "--- TESTAUS VALMIS ---" -ForegroundColor Cyan
Tallenna, ja aja ./test_api.ps1. Nyt pitäisi tulla vihreää riviä!