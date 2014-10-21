using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SceptrDocumentation.Models;

namespace SceptrDocumentation.Controllers
{
    public class VerbController : Controller
    {
        private SceptrDocumentationDbContext db = new SceptrDocumentationDbContext();

        //
        // GET: /Verb/

        public ActionResult Index()
        {
            return View(db.Verbs.ToList());
        }

        //
        // GET: /Verb/Details/5

        public ActionResult Details(int id = 0)
        {
            Verb verb = db.Verbs.Find(id);
            if (verb == null)
            {
                return HttpNotFound();
            }
            return View(verb);
        }

        //
        // GET: /Verb/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Verb/Create

        [HttpPost]
        public ActionResult Create(Verb verb)
        {
            if (ModelState.IsValid)
            {
                db.Verbs.Add(verb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(verb);
        }

        //
        // GET: /Verb/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Verb verb = db.Verbs.Find(id);
            if (verb == null)
            {
                return HttpNotFound();
            }
            return View(verb);
        }

        //
        // POST: /Verb/Edit/5

        [HttpPost]
        public ActionResult Edit(Verb verb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(verb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(verb);
        }

        //
        // GET: /Verb/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Verb verb = db.Verbs.Find(id);
            if (verb == null)
            {
                return HttpNotFound();
            }
            return View(verb);
        }

        //
        // POST: /Verb/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Verb verb = db.Verbs.Find(id);
            db.Verbs.Remove(verb);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}