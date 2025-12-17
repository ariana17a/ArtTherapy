using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using ArtTherapy_MVC.Models;
using LibrarieModele;
using Repository_CodeFirst;

namespace ArtTherapy_MVC.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly ArtTherapyCodeFirstContext db = new ArtTherapyCodeFirstContext();

        // helper pentru popularea dropdown-urilor
        private void PopulateDropDowns(FeedbackViewModel model)
        {
            model.Utilizatori = db.Utilizatori
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Nume
                })
                .ToList();

            model.LucrariArtistice = db.LucrariArtistice
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Titlu
                })
                .ToList();
        }

        // GET: Feedbacks
        public async Task<ActionResult> Index()
        {
            var feedbacks = await db.Feedbackuri
                .Include(f => f.Utilizator)
                .Include(f => f.LucrareArtistica)
                .ToListAsync();

            var model = feedbacks.Select(f => new FeedbackViewModel
            {
                Id = f.Id,
                Text = f.Text,
                UtilizatorId = f.UtilizatorId,
                LucrareArtisticaId = f.LucrareArtisticaId,
                NumeUtilizator = f.Utilizator != null ? f.Utilizator.Nume : string.Empty,
                TitluLucrare = f.LucrareArtistica != null ? f.LucrareArtistica.Titlu : string.Empty
            }).ToList();

            return View(model);
        }

        // GET: Feedbacks/Create
        public ActionResult Create()
        {
            var model = new FeedbackViewModel();
            PopulateDropDowns(model);
            return View(model);
        }

        // POST: Feedbacks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FeedbackViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropDowns(model);
                return View(model);
            }

            var feedback = new Feedback
            {
                Text = model.Text,
                UtilizatorId = model.UtilizatorId,
                LucrareArtisticaId = model.LucrareArtisticaId,
                CreatedAt = DateTime.Now
            };

            db.Feedbackuri.Add(feedback);
            await db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Feedback-ul a fost salvat cu succes.";

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}