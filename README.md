# FPS Matchmaker API
API za matchmaking FPS igrača, dizajniran za efikasan i skalabilan rad.

## Okruženje za build
Za uspešan build projekta, potrebno je sledeće:

### 1.Razvojno okruženje:

**Visual Studio 2022 (ili novija verzija) sa instaliranim .NET radnim okruženjem.**
### 2. .NET SDK:

**.NET 8.0 SDK (proveriti kompatibilnost sa priloženim fajlom FpsMatchmakerAPI.csproj).**
### 3.Operativni sistem:

**Windows 10/11 (preporučeno).
Alternativno, macOS ili Linux sa podrškom za .NET 8.0.**
### 4.Dodatne biblioteke:

**Sve potrebne biblioteke se automatski preuzimaju putem NuGet-a prilikom build procesa.**
## Kako se radi build
1.Otvorite projektni fajl *FpsMatchmakerAPI.csproj* u Visual Studio-u.
2.Osigurajte da je odabrana odgovarajuća verzija **.NET SDK-a u Tools > Options > .NET SDKs.**
3.Pritisnite *Ctrl + Shift + B* ili *kliknite na Build > Build Solution*.
Za build iz komandne linije:

```bash
dotnet build FpsMatchmakerAPI.csproj
```
## Kako se aplikacija pokreće
### Visual Studio
1.Kliknite na *Start Debugging (F5)* ili *Start Without Debugging (Ctrl+F5)*.
2.API će se pokrenuti na zadatoj adresi (npr. *https://localhost:8080*).
### Komandna linija
1.Navigirajte do direktorijuma gde se nalazi *.csproj* fajl.
2.Pokrenite aplikaciju komandom:
```bash
dotnet run --project FpsMatchmakerAPI.csproj
```
### Primer API poziva
Pokrenite API i posetite:
```bash
GET https://localhost:5001/api/matchmaking
```
## Korišćene tehnologije
-**C# i ASP.NET Core**
Glavni jezik i framework za razvoj ovog API-ja.
-**Entity Framework Core**
ORM za rad sa bazom podataka.
-**Swagger**
Dokumentacija API-ja i testiranje endpoint-a.
-**Newtonsoft.Json**
Obrada JSON podataka.
-**SQLite (opciono)**
Laka baza podataka za razvoj i testiranje.
    
