using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibrarieModele;
using Repository_CodeFirst;

namespace ArtTherapy_MVC.Controllers
{
    public class PlanUtilizareController : Controller
    {
        private ArtTherapyCodeFirstContext db = new ArtTherapyCodeFirstContext();

        // GET: PlanUtilizare
        public async Task<ActionResult> Index()
        {
            return View(await db.PlanuriUtilizare.ToListAsync());
        }

        // GET: PlanUtilizare/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanUtilizare planUtilizare = await db.PlanuriUtilizare.FindAsync(id);
            if (planUtilizare == null)
            {
                return HttpNotFound();
            }
            return View(planUtilizare);
        }

        // GET: PlanUtilizare/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlanUtilizare/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nume,Descriere,LimitaLucrari,EmotiiExtinse,FeedbackDeblocabil,SesiuniTerapie,ChatSecurizat")] PlanUtilizare planUtilizare)
        {
            if (ModelState.IsValid)
            {
                db.PlanuriUtilizare.Add(planUtilizare);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(planUtilizare);
        }

        // GET: PlanUtilizare/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanUtilizare planUtilizare = await db.PlanuriUtilizare.FindAsync(id);
            if (planUtilizare == null)
            {
                return HttpNotFound();
            }
            return View(planUtilizare);
        }

        // POST: PlanUtilizare/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nume,Descriere,LimitaLucrari,EmotiiExtinse,FeedbackDeblocabil,SesiuniTerapie,ChatSecurizat")] PlanUtilizare planUtilizare)
        {
            if (ModelState.IsValid)
            {
                db.Entry(planUtilizare).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(planUtilizare);
        }

        // GET: PlanUtilizare/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanUtilizare planUtilizare = await db.PlanuriUtilizare.FindAsync(id);
            if (planUtilizare == null)
            {
                return HttpNotFound();
            }
            return View(planUtilizare);
        }

        // POST: PlanUtilizare/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PlanUtilizare planUtilizare = await db.PlanuriUtilizare.FindAsync(id);
            db.PlanuriUtilizare.Remove(planUtilizare);
            await db.SaveChangesAsync();
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
