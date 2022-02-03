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
    public class TopicsController : Controller
    {
        private ExamDBEntities db = new ExamDBEntities();

        // GET: Topics
        public ActionResult Index()
        {
            var topic = db.Topic.Include(t => t.Paper);
            return View(topic.ToList());
        }

        // GET: Topics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topic.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: Topics/Create/ 添加某试卷考题
        public ActionResult Create(int PaperID)
        {
            //ViewBag.PaperID = new SelectList(db.Paper, "PaperID", "PaperName");
            ViewBag.PaperID = PaperID; //试卷编号
            ViewBag.PaperName = db.Paper.Find(PaperID).PaperName; //试卷名称
            //定义考题类型下拉列表
            ViewBag.TopicTypes = new SelectList(new[] {
                new SelectListItem { Value = "1", Text = "单选" }
                ,new SelectListItem { Value = "2", Text = "判断" }
                ,new SelectListItem { Value = "3", Text = "问答" }
                }, "Value", "Text");
            return View();
        }
        // POST: Topics/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TopicID,TopicExplain,TopicScore,TopicType,TopicA,TopicB,TopicC,TopicD,TopicSort,TopicAnswer,PaperID")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Topic.Add(topic);
                db.SaveChanges();
                //return RedirectToAction("Index");
                //添加成功后返回对应的试卷详情页
                return RedirectToAction("Details", "Papers", new { id = topic.PaperID });
            }

            ViewBag.PaperID = new SelectList(db.Paper, "PaperID", "PaperName", topic.PaperID);
            return View(topic);
        }

        // GET: Topics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topic.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaperID = new SelectList(db.Paper, "PaperID", "PaperName", topic.PaperID);
            //定义考题类型下拉列表
            ViewBag.TopicTypes = new SelectList(new[] {
                new SelectListItem { Value = "1", Text = "单选" }
                ,new SelectListItem { Value = "2", Text = "判断" }
                ,new SelectListItem { Value = "3", Text = "问答" }
                }, "Value", "Text",topic.TopicType);

            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TopicID,TopicExplain,TopicScore,TopicType,TopicA,TopicB,TopicC,TopicD,TopicSort,TopicAnswer,PaperID")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Details", "Papers", new { id = topic.PaperID });
            }
            ViewBag.PaperID = new SelectList(db.Paper, "PaperID", "PaperName", topic.PaperID);
            return View(topic);
        }

        // GET: Topics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topic.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Topic topic = db.Topic.Find(id);
            db.Topic.Remove(topic);
            db.SaveChanges();
            //删除成功后返回对应的试卷详情页
            return RedirectToAction("Details", "Papers", new { id = topic.PaperID });
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
