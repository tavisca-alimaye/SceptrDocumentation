using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
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
            //supplierProducts = (from sp in supplierProducts select sp).Distinct();
            var suppliers = new SelectList(db.Suppliers.Select(n => n.Name).Distinct().ToList());
            //var suppliers = new SelectList(db.Suppliers.Select(n =>n.Name).Distinct().ToList());


            var products = new SelectList(db.Products.Select(n => n.Name).Distinct().ToList());
          //  var suppliers = db.Suppliers.Include(p => p.ID).Include(s => s.Name);

            var suppliersProducts = db.SupplierProducts.Include(p => p.Product).Include(s => s.Supplier);

            //var suppliersProducts = new SelectList(db.SupplierProducts.Select(n => n.Product.Name), db.SupplierProducts.Select(n => n.Supplier.Name));
            ViewBag.Products = products;
            ViewBag.Suppliers = suppliers;
            //ViewBag.SupplierProducts = suppliersProducts;
            return View(db.SupplierProducts.ToList());
        }
        public ActionResult SupplierProductEnabled()
        {
            //supplierProducts = (from sp in supplierProducts select sp).Distinct();
            var suppliers = new SelectList(db.Suppliers.Select(n => n.Name).Distinct().ToList());
            //var suppliers = new SelectList(db.Suppliers.Select(n =>n.Name).Distinct().ToList());


            var products = new SelectList(db.Products.Select(n => n.Name).Distinct().ToList());
            //  var suppliers = db.Suppliers.Include(p => p.ID).Include(s => s.Name);

            var suppliersProducts = db.SupplierProducts.Include(p => p.Product).Include(s => s.Supplier);

            //var suppliersProducts = new SelectList(db.SupplierProducts.Select(n => n.Product.Name), db.SupplierProducts.Select(n => n.Supplier.Name));
            ViewBag.Products = products;
            ViewBag.Suppliers = suppliers;
            //ViewBag.SupplierProducts = suppliersProducts;
            return View(db.SupplierProducts.ToList());
        }

        public ActionResult CreateNew()
        {
            var products = new SelectList(db.Products.Select(n => n.Name).Distinct().ToList());
            ViewBag.Products = products;
            return View();
        }

        [HttpPost]
        public ActionResult CreateNew(Supplier supplier,string[] Products)
        {
            if (ModelState.IsValid)
            {
                db.Suppliers.Add(supplier);
                db.SaveChanges();
                var supplierID = (from supp in db.Suppliers where supp.Name==supplier.Name select supp.ID).First();
                var questionsIds = from q in db.Questions select q.Id;
                if (Products != null)
                {
                    for (int i = 0; i < Products.Length; i++)
                    {
                        var product = Products[i];
                        var productID = (from prod in db.Products where prod.Name == product select prod.ID).First();
                        SupplierProduct supp_prod = new SupplierProduct();
                        supp_prod.SupplierId = supplierID;
                        supp_prod.ProductId = productID;
                        db.SupplierProducts.Add(supp_prod);
                        db.SaveChanges();
                    } 
                }

                foreach (var questionId in questionsIds.ToList())
                {
                    db.QuestionAnswerMappers.Add(new QuestionAnswerMapper() { QuestionId = questionId,SupplierId = supplierID,Answer = ""});
                    db.SaveChanges();
                }
                return RedirectToAction("SupplierProduct");
            }

            return View(supplier);
        }

        [HttpPost, ActionName("Update")]
        public ActionResult Update(string[] SupplierProducts)
        {
            var rows = from o in db.SupplierProducts select o;
            foreach (var row in rows)
            {
                db.SupplierProducts.Remove(row);
            }
            //db.SaveChanges();
            //var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext;
            //objCtx.ExecuteStoreCommand("TRUNCATE TABLE SupplierProduct"); 
            for (int i = 0; i < SupplierProducts.Length; i++)
            {
                var supplier_product = SupplierProducts[i];
                string pattern=@"(\w+)+(\w+)";
                MatchCollection mc = Regex.Matches(supplier_product, pattern);
                var supplier = mc[0];
                var product = mc[1];

                var supplierName = supplier.ToString();
                var productName = product.ToString();
                var supplierID = (from supp in db.Suppliers where supp.Name == supplierName select supp.ID).First();
                var productID = (from prod in db.Products where prod.Name == productName select prod.ID).First();

                SupplierProduct supp_prod = new SupplierProduct();
                supp_prod.SupplierId = supplierID;
                supp_prod.ProductId = productID;
                db.SupplierProducts.Add(supp_prod);
                db.SaveChanges();

            }
            return RedirectToAction("SupplierProduct");
        }


        public ActionResult QuestionsForSupplier(string supplierName)
        {
            var questionsForSupplier = from q in db.QuestionAnswerMappers.Include(q => q.Question)
                                                                        .Include(s => s.Supplier)
                                                                        .Include(v => v.Question.Verb)
                                                                    where q.Supplier.Name == supplierName
                                                                    select q;
            ViewBag.Categories = db.Verbs;
            ViewBag.SupplierName = supplierName;
            return View(questionsForSupplier.ToList());

        }

        public ActionResult EditQuestionsForSupplier(string supplierName)
        {
            var questionsForSupplier = from q in db.QuestionAnswerMappers.Include(q => q.Question)
                                                                        .Include(s => s.Supplier)
                                                                        .Include(v => v.Question.Verb)
                                       where q.Supplier.Name == supplierName
                                       select q;
            ViewBag.Categories = db.Verbs;
            ViewBag.SupplierName = supplierName;
            return View(questionsForSupplier.ToList());
        }

        [HttpPost, ActionName("SubmitNewAnswersForQuestion")]
        public ActionResult SubmitNewAnswersForQuestion(string[] answers,string[] questionIds,string supplierId)
        {
            int supplier = Int32.Parse(supplierId);
            var rows = from r in db.QuestionAnswerMappers where r.SupplierId == supplier select r;
            string supplierName = db.Suppliers.Find(supplier).Name;
            foreach (var row in rows)
            {
                db.QuestionAnswerMappers.Remove(row);
            }

            for (int i = 0; i < questionIds.Length; i++)
            {
                int question = Int32.Parse(questionIds[i]);
                db.QuestionAnswerMappers.Add(new QuestionAnswerMapper() { QuestionId = question, SupplierId = supplier, Answer = answers[i] });
            }
            db.SaveChanges();
            return RedirectToAction("QuestionsForSupplier", new { supplierName = supplierName});
        }
        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}