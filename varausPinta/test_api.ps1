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