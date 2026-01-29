### ANALYYSI TEKOÄLYN TUOTTAMASTA RATKAISUSTA

##  Mitä tekoäly teki hyvin
	Tekoäly loi ensimmäisellä promptilla nopeasti hyvän perusrakenteen johon pystyin jatkamaan melko vaivattomasti omaa työtä

	Tekoälyn generoima koodi oli syntaktisesti ihan oikeaa C# kieltä ja kääntyi ilman virheitä
	Liiketoiminta säännöt kuten aikojen vertailu ja päällekäisyydet olivat pääpiirteittäin oikein toteutettu
	Koodi oli selkeää ja muuttujien nimet olivat  kuvaavia.

##  Mitä tekoäly teki huonosti
	Vaikka koodi toimikin pintapuolisesti, siinä oli merkittäviä arkkitehtuurisia ja teknisiä puutteita

	Riippuvuuksien injektointi oli rikki, tekoäly rekisteröi palvelun program.cs teidostossa, mutta loi sen silti manuaalisesti kontrollerin sisällä. Tämä rikkoo IoC periaatteita.
	APIen yksi tärkeimmistä ominaisuuksita eli säieturvallisuus puuttui, staattinen List<T> ei ole säieturvallinen, ja samanaikaiset pyynnöt olisivat voineet aiheuttaa datakorruptiota tai kaatumisen race conditioneiden takia
	ID hallinta puuttui tekoälyn generoimasta koodista
	Tietokanta oli julkinen staattinen  muuttuja jota kontrolleri manipuloi suoraan
	Koodi käytti DateTime.now funktiota, joka säilyttää palvelimen paikkallisen ajan, DateTime.UtcNow sijaan, joka in riskialtista palvelinympäristöissä

## Tärkeimmät parannukset perusteluineen
	Tein koodiin seuraavat refaktoroinnit
		
		Arkkitehturin korjaus
		- Poistin 'new BookingService()' kutsut ja 'InMemoryDatabase'-luokan
		-Siirsin datalogiikan Service tasolle ja otin palvelun käyttöön kontrollerin konstruktion kautta. Näin noudatetaan Separation of Concerns periaatetta
		
		Säietruvallisuus
		-Lisäsin BookingService luokkaan lock mekanismin, tämä varmistaa että kaksi tai useampi käyttäjää ei voi mukata varaustaulua täsmälleen samaan aikaan, mikä estää tuplavaraukset tai palvelun kaatumisen kuorman lisääntyessä

		Datan eheys ja ID-generointi
		-Toteutin palvelinpulen ID-generoinnin
		-Muutin tietovaraston yksityiseksi, jota sitä voi muokata vain hallitusti metodien kautta

		REST standardin nuodattaminen
		-Muutin CreateBooking metodin palauttamaan 201 created pelkän 200 OKn sijaan
		-Lisäsin Location headerin ja GetBookingByID-endpointin, jotta API olisi aidosti RESTfulia

		Validointi
		-Lisäsin [required] attribuutit tietomalliin ja ModelState tarkistuksen, jotta API hylkää puutteelliset pyynöt heti, eikä ota niitä ollenkaan käsittelyyn