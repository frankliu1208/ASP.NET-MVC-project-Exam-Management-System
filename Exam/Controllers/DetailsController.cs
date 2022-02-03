using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Exam.Models;

namespace Exam.Controllers
{
    public class DetailsController : Controller
    {
        private ExamDBEntities db = new ExamDBEntities();

        // GET: Details
        public ActionResult Index()
        {
            var detail = db.Detail.Include(d => d.Answer).Include(d => d.Topic);
            return View(detail.ToList());
        }

        // GET: Details/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail detail = db.Detail.Find(id);
            if (detail == null)
            {
                return HttpNotFound();
            }
            return View(detail);
        }

        // GET: Details/Create
        public ActionResult Create()
        {
            ViewBag.AnswerID = new SelectList(db.Answer, "AnswerID", "AnswerID");
            ViewBag.TopicID = new SelectList(db.Topic, "TopicID", "TopicExplain");
            return View();
        }

        // POST: Details/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DetailID,AnswerID,TopicID,DetailAnswer")] Detail detail)
        {
            if (ModelState.IsValid)
            {
                db.Detail.Add(detail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AnswerID = new SelectList(db.Answer, "AnswerID", "AnswerID", detail.AnswerID);
            ViewBag.TopicID = new SelectList(db.Topic, "TopicID", "TopicExplain", detail.TopicID);
            return View(detail);
        }

        // GET: Details/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail detail = db.Detail.Find(id);
            if (detail == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnswerID = new SelectList(db.Answer, "AnswerID", "AnswerID", detail.AnswerID);
            ViewBag.TopicID = new SelectList(db.Topic, "TopicID", "TopicExplain", detail.TopicID);
            return View(detail);
        }

        // POST: Details/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DetailID,AnswerID,TopicID,DetailAnswer")] Detail detail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AnswerID = new SelectList(db.Answer, "AnswerID", "AnswerID", detail.AnswerID);
            ViewBag.TopicID = new SelectList(db.Topic, "TopicID", "TopicExplain", detail.TopicID);
            return View(detail);
        }

        // GET: Details/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail detail = db.Detail.Find(id);
            if (detail == null)
            {
                return HttpNotFound();
            }
            return View(detail);
        }

        // POST: Details/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail detail = db.Detail.Find(id);
            db.Detail.Remove(detail);
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
