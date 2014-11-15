using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjektB.Web.Models;

namespace ProjektB.Web.Controllers
{
    public class FitnessProvidersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FitnessProviders
        public ActionResult Index()
        {
            var fitnessProviders = db.FitnessProviders.Include(f => f.ApplicationUser);
            return View(fitnessProviders.ToList());
        }

        // GET: FitnessProviders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FitnessProvider fitnessProvider = db.FitnessProviders.Find(id);
            if (fitnessProvider == null)
            {
                return HttpNotFound();
            }
            return View(fitnessProvider);
        }

        // GET: FitnessProviders/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: FitnessProviders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ConnectionDetails,Type,ApplicationUserId")] FitnessProvider fitnessProvider)
        {
            if (ModelState.IsValid)
            {
                db.FitnessProviders.Add(fitnessProvider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", fitnessProvider.ApplicationUserId);
            return View(fitnessProvider);
        }

        // GET: FitnessProviders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FitnessProvider fitnessProvider = db.FitnessProviders.Find(id);
            if (fitnessProvider == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", fitnessProvider.ApplicationUserId);
            return View(fitnessProvider);
        }

        // POST: FitnessProviders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ConnectionDetails,Type,ApplicationUserId")] FitnessProvider fitnessProvider)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fitnessProvider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", fitnessProvider.ApplicationUserId);
            return View(fitnessProvider);
        }

        // GET: FitnessProviders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FitnessProvider fitnessProvider = db.FitnessProviders.Find(id);
            if (fitnessProvider == null)
            {
                return HttpNotFound();
            }
            return View(fitnessProvider);
        }

        // POST: FitnessProviders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FitnessProvider fitnessProvider = db.FitnessProviders.Find(id);
            db.FitnessProviders.Remove(fitnessProvider);
            db.SaveChanges();
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
