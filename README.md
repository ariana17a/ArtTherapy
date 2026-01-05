# ArtTherapy – Aplicație Web ASP.NET MVC + Web API

## Descriere
ArtTherapy este o aplicație web realizată în ASP.NET MVC și ASP.NET Web API, care permite gestionarea planurilor de utilizare pentru o platformă de terapie prin artă.

---

## Tehnologii utilizate
- ASP.NET MVC (.NET Framework)
- ASP.NET Web API
- Entity Framework (DB First)
- SQL Server
- JavaScript (fetch API)
- Dependency Injection (implementare custom – SimpleResolver)
- Git & GitHub

---

## Structura soluției
- **ArtTherapy_API**
  - Web API pentru gestionarea planurilor de utilizare
  - CRUD: GET, POST, DELETE
  - Dependency Injection pentru servicii
- **ArtTherapy_MVC**
  - Interfață web MVC
  - Consumă API-ul prin fetch (pagina ApiCrud)
- **Repository_DBFirst**
  - Acces la baza de date (Entity Framework DB First)

---

## Instalare și rulare

### Cerințe
- Visual Studio 2022
- SQL Server / SQL Express
- .NET Framework

### Pași de rulare
1. Clonează repository-ul: git clone https://github.com/ariana17a/ArtTherapy.git
2. Deschide fișierul `ArtTherapy_MVC.sln` în Visual Studio
3. Configurează **Multiple Startup Projects**:
- ArtTherapy_API → Start
- ArtTherapy_MVC → Start
4. Rulează soluția cu **IIS Express**

---

## URL-uri utile
- API:
https://localhost:44311/api/PlanUtilizare

- Frontend MVC (consum API):
https://localhost:44302/PlanUtilizare/ApiCrud


---

## Funcționalități implementate
- Listare planuri (GET)
- Adăugare plan (POST)
- Ștergere plan (DELETE)
- Consum API din MVC
- Dependency Injection pentru servicii
- Logging de bază și tratare erori

---

## Autor
- Nume: Ariana Apopii
- Grupa: 7311b
- An universitar: 2025–2026
