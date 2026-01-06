# DOCUMENTA?IE PROIECT PPAW - ArtTherapy

**Student:** Apopii Ariana  
**Email:** apopiiariana17@gmail.com  
**An universitar:** 2024-2025  
**Disciplina:** PPAW (Proiectarea ?i Programarea Aplica?iilor Web)

---

## 1. PROIECTARE

### 1.1 Paradigme utilizate

Proiectul **ArtTherapy** utilizeaz? urm?toarele paradigme ?i tehnologii:

**MVC (Model-View-Controller)**
- Framework: ASP.NET MVC 5 pe .NET Framework 4.7.2
- Proiect: `ArtTherapy_MVC`
- Responsabilit??i:
  - **Model**: `LibrarieModele` (PlanUtilizare, Feedback)
  - **View**: Razor views (.cshtml) pentru prezentare
  - **Controller**: Logic? de procesare cereri (PlanUtilizareController, FeedbacksController)

**Web API REST**
- Framework: ASP.NET Web API 2
- Proiect: `ArtTherapy_API` (Visual Basic .NET)
- Endpoints:
  - `GET /api/PlanUtilizare` ? list? planuri (JSON)
  - `POST /api/PlanUtilizare` ? creare plan nou
  - `DELETE /api/PlanUtilizare/{id}` ? ?tergere plan

**ORM Code First**
- Tehnologie: Entity Framework 6.5.1
- Proiect: `Repository_CodeFirst`
- Context: `ArtTherapyCodeFirstContext`
- Avantaj: Control complet asupra modelelor, migra?ii automate

**ORM Database First**
- Tehnologie: Entity Framework 6.5.1 (EDMX)
- Proiect: `Repository_DBFirst`
- Avantaj: Generare automat? clase din baza de date existent?

**Dependency Injection**
- Implementare: `SimpleResolver` (custom, f?r? Unity/Autofac)
- Servicii injectate: `IPlanuriService`, `ICache`
- Pattern: Constructor Injection

**Service Layer Pattern**
- Interfe?e: `IPlanuriService`, `ICache`
- Implement?ri: `PlanuriService`, `MemoryCacheService`
- Responsabilitate: Logic? business ?i acces date

---

### 1.2 De ce au fost alese?

**MVC**
- Separare clar? responsabilit??i (prezentare vs logic?)
- Testabilitate superioar? fa?? de Web Forms
- Routing flexibil ?i SEO-friendly

**Web API**
- Expunere servicii REST pentru consum cross-platform
- JSON nativ (u?or de consumat din JavaScript/mobile)
- Separare front-end (MVC) de back-end (API)

**Code First**
- Control complet asupra modelelor (clasele C# dicteaz? schema DB)
- Migra?ii automate (ad?ugare coloane f?r? SQL manual)
- Folosit în MVC pentru modele simple create de dezvoltator

**Database First**
- Baza de date deja existent?
- Generare automat? clase din tabele
- Folosit în API pentru evitarea duplic?rii modelelor

**Dependency Injection**
- Loose coupling între Controller ?i Service
- Testabilitate (mock-uri u?oare pentru unit testing)
- Gestionare centralizat? instan?e (singleton cache)

**Service Layer**
- Separare logic? business de prezentare
- Refolosire cod (acela?i service din mai multe controllere)
- Izolare EF context (nu expui DbContext direct)

---

### 1.3 Arhitectura aplica?iei

```
???????????????????????????????????????????
?         CLIENT (Browser)                ?
???????????????????????????????????????????
               ? HTTP Request/Response
               ?
???????????????????????????????????????????
?      ArtTherapy_MVC (Presentation)      ?
?  ??????????????????????????????????     ?
?  ? Controllers                    ?     ?
?  ?  - PlanUtilizareController     ?     ?
?  ?  - FeedbacksController         ?     ?
?  ??????????????????????????????????     ?
?               ?                         ?
?  ??????????????????????????????????     ?
?  ? Views (Razor .cshtml)          ?     ?
?  ?  - Index, Create, Edit, Delete ?     ?
?  ??????????????????????????????????     ?
?               ?                         ?
?  ??????????????????????????????????     ?
?  ? Repository_CodeFirst           ?     ?
?  ?  - ArtTherapyCodeFirstContext  ?     ?
?  ??????????????????????????????????     ?
???????????????????????????????????????????
               ? SQL Queries
               ?
???????????????????????????????????????????
?       SQL Server Database               ?
?  - planuri_utilizare                    ?
?  - feedbacks                            ?
???????????????????????????????????????????

???????????????????????????????????????????
?    ArtTherapy_API (Service Layer)       ?
?  ??????????????????????????????????     ?
?  ? API Controllers (VB.NET)       ?     ?
?  ?  - PlanUtilizareController     ?     ?
?  ??????????????????????????????????     ?
?               ? DI (SimpleResolver)     ?
?  ??????????????????????????????????     ?
?  ? Services (Business Logic)      ?     ?
?  ?  - PlanuriService              ?     ?
?  ?  - MemoryCacheService          ?     ?
?  ??????????????????????????????????     ?
?               ?                         ?
?  ??????????????????????????????????     ?
?  ? Repository_DBFirst (EDMX)      ?     ?
?  ?  - ArtTherapyEntities          ?     ?
?  ??????????????????????????????????     ?
???????????????????????????????????????????
```

**Fluxul unei cereri GET cu cache:**
1. Client ? `GET /api/PlanUtilizare`
2. Controller ? `PlanuriService.GetAll()`
3. Service verific? cache:
   - **Cache HIT** ? return din memorie
   - **Cache MISS** ? query DB ? salveaz? în cache
4. Response: JSON cu lista planuri

---

## 2. IMPLEMENTARE

### 2.1 Business Layer

**PlanuriService** (logica principal?):

**GetAll() cu caching:**
```vb
Public Function GetAll() As IEnumerable(Of planuri_utilizare)
    ' 1. Verificare cache
    If _cache.IsSet(CACHE_KEY) Then
        Trace.WriteLine("CACHE HIT")
        Return _cache.Get(Of List(Of planuri_utilizare))(CACHE_KEY)
    End If
    
    ' 2. Query DB + salvare în cache
    Trace.WriteLine("CACHE MISS")
    Using ctx As New ArtTherapyEntities()
        Dim data = ctx.planuri_utilizare.ToList()
        _cache.Set(CACHE_KEY, data)
        Return data
    End Using
End Function
```

**Insert() cu invalidare cache:**
```vb
Public Function Insert(p As planuri_utilizare) As planuri_utilizare
    Using ctx As New ArtTherapyEntities()
        ctx.planuri_utilizare.Add(p)
        ctx.SaveChanges()
        
        ' Invalidare cache dup? modificare
        _cache.Remove(CACHE_KEY)
        
        Return p
    End Using
End Function
```

**Logic? implementat?:**
- **Caching**: Cache-first strategy cu invalidare la modific?ri
- **Logging**: Trace pentru debugging (CACHE HIT/MISS, errori)
- **Thread-safety**: `ConcurrentDictionary` pentru cache
- **TTL**: 10 minute per cache item

---

### 2.2 Libr?rii suplimentare

| Libr?rie | Versiune | Utilizare |
|----------|----------|-----------|
| EntityFramework | 6.5.1 | ORM (Code First + DB First) |
| Newtonsoft.Json | 13.0.4 | Serializare JSON în API |
| Bootstrap | 5.3.0 | Grid, navbar, componente UI |
| jQuery | 3.7.1 | AJAX, DOM manipulation |
| Microsoft.AspNet.WebApi.Cors | 5.3.0 | Cross-origin requests |

---

### 2.3 Sec?iuni de cod deosebite

**1. Dependency Injection custom**
```vb
Public Class SimpleResolver
    Implements IDependencyResolver
    
    Private Shared ReadOnly _cacheInstance As ICache = New MemoryCacheService()
    
    Public Function GetService(serviceType As Type) As Object
        If serviceType Is GetType(IPlanuriService) Then
            Return New PlanuriService(_cacheInstance)
        End If
        Return Nothing
    End Function
End Class
```

**2. Search + Sort în MVC**
```csharp
public async Task<ActionResult> Index(string search, string sort)
{
    var planuri = await db.PlanuriUtilizare.ToListAsync();
    
    // Filtrare
    if (!string.IsNullOrEmpty(search))
    {
        planuri = planuri.Where(p => 
            p.Nume.Contains(search) || 
            p.Descriere.Contains(search)
        ).ToList();
    }
    
    // Sortare
    if (sort == "nume")
        planuri = planuri.OrderBy(p => p.Nume).ToList();
    
    return View(planuri);
}
```

**3. Cache invalidation pattern**
- La fiecare `Insert()` sau `Delete()` ? `_cache.Remove(CACHE_KEY)`
- Garanteaz? consisten?? date

**4. UI responsive cu CSS Grid**
```css
.plans-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 18px;
}
@media (max-width: 900px) {
    .plans-grid { grid-template-columns: 1fr; }
}
```

---

## 3. UTILIZARE

### 3.1 Instalare pentru programator

**Cerin?e:**
- Visual Studio 2019/2022
- .NET Framework 4.7.2 SDK
- SQL Server 2017+ (LocalDB/Express)

**Pa?i:**
1. **Clone repository:**
   ```
   git clone https://github.com/ariana17a/ArtTherapy.git
   ```

2. **Deschide solution:**
   - `ArtTherapy.sln` în Visual Studio

3. **Restaurare NuGet:**
   - Right-click Solution ? Restore NuGet Packages

4. **Configurare connection string:**
   - `ArtTherapy_MVC/Web.config`:
   ```xml
   <add name="ArtTherapyCodeFirstContext"
        connectionString="Data Source=(localdb)\MSSQLLocalDB;
                         Initial Catalog=ArtTherapyDB;
                         Integrated Security=True" />
   ```

5. **Creare baz? de date (Code First):**
   - Package Manager Console:
   ```
   Enable-Migrations
   Add-Migration InitialCreate
   Update-Database
   ```

6. **Build + Run:**
   - F5 (MVC + API simultan)

---

### 3.2 Instalare pentru beneficiar

**Cerin?e:**
- Windows Server 2016+ / Windows 10/11
- IIS 10+
- .NET Framework 4.7.2 Runtime
- SQL Server 2017+ Express

**Pa?i:**
1. **Activare IIS** (Windows Features ? IIS + ASP.NET 4.8)

2. **Instalare SQL Server Express** (gratuit)

3. **Creare baz? de date** (SSMS):
   ```sql
   CREATE DATABASE ArtTherapyDB;
   USE ArtTherapyDB;
   
   CREATE TABLE planuri_utilizare (
       id INT IDENTITY(1,1) PRIMARY KEY,
       nume NVARCHAR(100) NOT NULL,
       descriere NVARCHAR(500),
       limita_lucrari INT,
       emotii_extinse BIT DEFAULT 0,
       feedback_deblocabil BIT DEFAULT 0,
       sesiuni_terapie BIT DEFAULT 0,
       chat_securizat BIT DEFAULT 0
   );
   ```

4. **Publicare din Visual Studio:**
   - Right-click pe `ArtTherapy_MVC` ? Publish ? Folder
   - Target: `C:\inetpub\wwwroot\ArtTherapy_MVC`

5. **Creare site IIS:**
   - IIS Manager ? Add Website
   - Port: 8080 (MVC), 8081 (API)

6. **Testare:**
   - `http://localhost:8080`

---

### 3.3 Mod de utilizare

**Pentru utilizator final:**

1. **Homepage** ? 3 carduri informative:
   - Terapie prin art?
   - Terapeu?i acredita?i
   - Planuri flexibile

2. **Pagina Planuri:**
   - Vezi carduri cu planuri (Free/Pro/Premium)
   - **Search:** Caut? dup? nume/descriere
   - **Sort:** Sorteaz? dup? nume sau limit? lucr?ri
   - Click **Alege planul** ? detalii plan

3. **Creare plan nou:**
   - Click **Creeaz? plan nou**
   - Formular: nume, descriere, checkboxuri (emo?ii, feedback, chat)
   - Submit ? planul apare în list?

4. **Editare/?tergere:**
   - Click **Edit** ? modific? date ? Save
   - Click **Delete** ? confirmare ? plan ?ters

---

## 4. CAPTURI ECRAN

*(Adaug? aici 2-3 capturi:)*
1. Homepage cu carduri
2. Pagina Planuri cu search/sort
3. API response JSON (`/api/PlanUtilizare`)

---

## 5. CONCLUZII

Proiectul **ArtTherapy** demonstreaz?:
- ? Implementare complet? CRUD pe 2 entit??i (MVC + API)
- ? Utilizare avansat? ORM (Code First + DB First)
- ? Service Layer cu caching ?i DI
- ? Logic? business specific? (invalidare cache, logging)
- ? UI responsive cu CSS Grid
- ? Search + Sort func?ional

Aplica?ia poate fi extins? cu:
- Autentificare utilizatori (ASP.NET Identity)
- Upload imagini lucr?ri artistice
- Sistem de pl??i (Stripe API)
- Dashboard analytics pentru terapeu?i

---

**Data:** [Completeaz? data]  
**Semn?tura:** [Completeaz?]
