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
    public class ClientSupplierController : Controller
    {
        private SceptrDocumentationDbContext db = new SceptrDocumentationDbContext();

        //
        // GET: /ClientSupplier/

        public ActionResult Index()
        {
            var clientsuppliermaps = db.ClientSupplierMaps.Include(c => c.Client).Include(c => c.Supplier).Include(c => c.Product);
            return View(clientsuppliermaps.ToList());
        }

        //
        // GET: /ClientSupplier/Details/5

        public ActionResult Details(int id = 0)
        {
            ClientSupplierMap clientsuppliermap = db.ClientSupplierMaps.Find(id);
            if (clientsuppliermap == null)
            {
                return HttpNotFound();
            }
            return View(clientsuppliermap);
        }

        //
        // GET: /ClientSupplier/Create

        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Clients, "ID", "Name");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "ID", "Name");
            ViewBag.ProductId = new SelectList(db.Products, "ID", "Name");
            return View();
        }

        //
        // POST: /ClientSupplier/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientSupplierMap clientsuppliermap)
        {
            if (ModelState.IsValid)
            {
                db.ClientSupplierMaps.Add(clientsuppliermap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clients, "ID", "Name", clientsuppliermap.ClientId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "ID", "Name", clientsuppliermap.SupplierId);
            ViewBag.ProductId = new SelectList(db.Products, "ID", "Name", clientsuppliermap.ProductId);
            return View(clientsuppliermap);
        }

        //
        // GET: /ClientSupplier/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ClientSupplierMap clientsuppliermap = db.ClientSupplierMaps.Find(id);
            if (clientsuppliermap == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ID", "Name", clientsuppliermap.ClientId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "ID", "Name", clientsuppliermap.SupplierId);
            ViewBag.ProductId = new SelectList(db.Products, "ID", "Name", clientsuppliermap.ProductId);
            return View(clientsuppliermap);
        }

        //
        // POST: /ClientSupplier/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientSupplierMap clientsuppliermap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientsuppliermap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ID", "Name", clientsuppliermap.ClientId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "ID", "Name", clientsuppliermap.SupplierId);
            ViewBag.ProductId = new SelectList(db.Products, "ID", "Name", clientsuppliermap.ProductId);
            return View(clientsuppliermap);
        }

        //
        // GET: /ClientSupplier/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ClientSupplierMap clientsuppliermap = db.ClientSupplierMaps.Find(id);
            if (clientsuppliermap == null)
            {
                return HttpNotFound();
            }
            return View(clientsuppliermap);
        }

        //
        // POST: /ClientSupplier/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientSupplierMap clientsuppliermap = db.ClientSupplierMaps.Find(id);
            db.ClientSupplierMaps.Remove(clientsuppliermap);
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