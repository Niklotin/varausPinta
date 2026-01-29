# Meeting Room Booking API

Yksinkertainen REST API kokoushuoneiden varaamiseen. Toteutettu C#:lla (.NET 10) k‰ytt‰en in-memory tietokantaa.

## Ominaisuudet

* **Luo varaus:** Est‰‰ p‰‰llekk‰iset varaukset ja menneisyyteen varaamisen.
* **Selaa varauksia:** Listaa varaukset huonekohtaisesti tai hae ID:ll‰.
* **Peru varaus:** Poista olemassa oleva varaus.
* **Tekninen laatu:** S‰ieturvallinen (Thread-safe) toteutus ja Dependency Injection.

## K‰ynnistys

Vaatimukset: .NET 10 SDK

1.  Kloonaa repositorio.
2.  Aja komento projektin juuressa:
    ```bash
    dotnet run
    ```
3.  API k‰ynnistyy oletuksena porttiin (ks. konsolin tuloste).

## Testaus

Projektin juuressa on mukana PowerShell-skripti `test_api.ps1`, joka ajaa automaattiset testit API:lle.

1.  Varmista, ett‰ sovellus on k‰ynniss‰.
2.  Tarkista `test_api.ps1` -tiedostosta, ett‰ `$baseUrl` vastaa sovelluksesi porttia.
3.  Aja skripti:
    ```powershell
    ./test_api.ps1
    ```

## Teknologiat

* C# / .NET 10
* ASP.NET Core Web API
* In-Memory Storage (Singleton Service)