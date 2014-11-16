﻿using System;
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


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadProductsBySuppliers(string sId)
        {
            int id = int.Parse(sId);
            var productList = (from suppProd in db.SupplierProducts.Include(s => db.Suppliers)
                                                               .Include(p => db.Products)
                               where suppProd.Supplier.ID == id
                               select suppProd.ProductId).ToList();


            var products = new List<string>();
            foreach (var productId in productList)
            {
                var name = (from prod in db.Products where prod.ID == productId select prod.Name).First();
                products.Add(name);
            }
            var phyData = products.Select(m => new SelectListItem()
            {
                Text = m,
                Value = m,
            });
            return Json(phyData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SeeDetails(int id=0)
        {
            var suppliers = (from suppProd in db.SupplierProducts.Include(s => db.Suppliers)
                                                                .Include(p => db.Products)
                             where suppProd.Product.ID == id
                             select suppProd.SupplierId).ToList();



            var supplierNames = new List<string>();
            var clientNames = new List<string>();
            var clientsSuppliers = new List<string>();
            foreach (var supplier in suppliers)
            {

                var clientId = (from cs in db.ClientSupplierMaps.Include(s => db.Suppliers)
                                           .Include(c => db.Clients)
                                       where cs.SupplierId == supplier && cs.ProductId == id
                                       select cs.ClientId).ToList();
                var supplierName = (from supp in db.Suppliers where supp.ID == supplier select supp.Name).First();
                supplierNames.Add(supplierName);
                foreach (var cId in clientId)
                {
                    var clientName = (from client in db.Clients where client.ID == cId select client.Name).First();
                    if (!clientNames.Contains(clientName))
                    {
                        clientNames.Add(clientName);
                    }
                    
                    var csName = clientName +"+"+ supplierName;
                    clientsSuppliers.Add(csName);
                }
            }


            //var clientsuppliermaps = (from csm in db.ClientSupplierMaps.Include(s => db.Suppliers)
            //                                                            .Include(c => db.Clients)
            //                                                            .Include(p=>db.Products) 
            //                                                    where csm.ProductId == id 
            //                                                    select csm).ToList();
            ViewBag.Suppliers = supplierNames;
            ViewBag.Clients = clientNames;
            ViewBag.ClientSuppliers = clientsSuppliers;
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}