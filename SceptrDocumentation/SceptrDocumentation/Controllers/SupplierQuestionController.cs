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
    public class SupplierQuestionController : Controller
    {
        private SceptrDocumentationDbContext db = new SceptrDocumentationDbContext();

        //
        // GET: /SupplierQuestion/

        public ActionResult Index()
        {
            var questionanswermappers = db.QuestionAnswerMappers.Include(q => q.Question).Include(q => q.Supplier);
            return View(questionanswermappers.ToList());
        }

        //
        // GET: /SupplierQuestion/Details/5

        public ActionResult Details(int id = 0)
        {
            QuestionAnswerMapper questionanswermapper = db.QuestionAnswerMappers.Find(id);
            if (questionanswermapper == null)
            {
                return HttpNotFound();
            }
            return View(questionanswermapper);
        }

        //
        // GET: /SupplierQuestion/Create

        public ActionResult Create()
        {
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "QuestionText");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "ID", "Name");
            return View();
        }

        //
        // POST: /SupplierQuestion/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuestionAnswerMapper questionanswermapper)
        {
            if (ModelState.IsValid)
            {
                db.QuestionAnswerMappers.Add(questionanswermapper);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "QuestionText", questionanswermapper.QuestionId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "ID", "Name", questionanswermapper.SupplierId);
            return View(questionanswermapper);
        }

        //
        // GET: /SupplierQuestion/Edit/5

        public ActionResult Edit(int quesId, int suppId)
        {
            QuestionAnswerMapper questionanswermapper = db.QuestionAnswerMappers.Find(quesId, suppId);
            Question question = db.Questions.Find(quesId);
            ViewBag.questionText = question.QuestionText;
            Supplier supplier = db.Suppliers.Find(suppId);
            ViewBag.supplierName = supplier.Name;
            if (questionanswermapper == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "QuestionText", questionanswermapper.QuestionId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", questionanswermapper.SupplierId);
            return View(questionanswermapper);
        }

        //
        // POST: /SupplierQuestion/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionAnswerMapper questionanswermapper)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questionanswermapper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "QuestionText", questionanswermapper.QuestionId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "ID", "Name", questionanswermapper.SupplierId);
            return View(questionanswermapper);
        }

        //
        // GET: /SupplierQuestion/Delete/5

        public ActionResult Delete(int quesId, int suppId)
        {
            QuestionAnswerMapper questionanswermapper = db.QuestionAnswerMappers.Find(quesId, suppId);
            Question question = db.Questions.Find(quesId);
            ViewBag.questionText = question.QuestionText;
            Supplier supplier = db.Suppliers.Find(suppId);
            ViewBag.supplierName = supplier.Name;
            if (questionanswermapper == null)
            {
                return HttpNotFound();
            }
            return View(questionanswermapper);
        }

        //
        // POST: /SupplierQuestion/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int quesId, int suppId)
        {
            QuestionAnswerMapper questionanswermapper = db.QuestionAnswerMappers.Find(quesId,suppId);
            db.QuestionAnswerMappers.Remove(questionanswermapper);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult QuestionForSupplier(int id = 0)
        {
            var supplierQuestions = db.QuestionAnswerMappers.Include(s => s.Supplier).Include(q => q.Question);
            var questions = from q in supplierQuestions where q.SupplierId == id select q;
            //foreach (var question in questions)
            //{
                
            //}
            ViewBag.Supplier = from s in db.Suppliers where s.ID == id select s.Name;
            return View(questions);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}