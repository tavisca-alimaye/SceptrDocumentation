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
    public class SupplierController : Controller
    {
        private SceptrDocumentationDbContext db = new SceptrDocumentationDbContext();

        //
        // GET: /Supplier/

        public ActionResult Index()
        {
            return View(db.Suppliers.ToList());
        }

        //
        // GET: /Supplier/Details/5

        public ActionResult Details(int id = 0)
        {
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        //
        // GET: /Supplier/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Supplier/Create

        [HttpPost]
        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Suppliers.Add(supplier);
                db.SaveChanges();
                return RedirectToAction("SupplierProduct");
            }

            return View(supplier);
        }

        //
        // GET: /Supplier/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        //
        // POST: /Supplier/Edit/5

        [HttpPost]
        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        //
        // GET: /Supplier/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        //
        // POST: /Supplier/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            db.Suppliers.Remove(supplier);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SupplierProduct()
        {
            //var supplierProducts = db.SupplierProducts.Include(p => p.Product).Include(s => s.Supplier);
            //supplierProducts = (from sp in supplierProducts select sp).Distinct();
            var suppliers = new SelectList(db.Suppliers.Select(n => n.Name).Distinct().ToList());
            var products = new SelectList(db.Products.Select(n => n.Name).Distinct().ToList());
            ViewBag.Products = products;
            ViewBag.Suppliers = suppliers;
            return View(db.SupplierProducts.ToList());



        }





        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}