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
    public class AnswersController : Controller
    {
        private ExamDBEntities db = new ExamDBEntities();

        // GET: Answers
        public ActionResult Index()
        {
            var answer = db.Answer.Include(a => a.Paper).Include(a => a.Student).Include(a => a.Teacher);
            return View(answer.ToList());
        }

        // GET: Answers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answer.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // GET: Answers/Create
        public ActionResult Create()
        {
            ViewBag.PaperID = new SelectList(db.Paper, "PaperID", "PaperName");
            ViewBag.StuID = new SelectList(db.Student, "StuID", "StuName");
            ViewBag.TeacherID = new SelectList(db.Teacher, "TeacherID", "TeacherName");
            return View();
        }

        // POST: Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AnswerID,PaperID,StuID,TeacherID,AnswerScore,AnswerTime,SubmitTime,BatchTime,AnswerState")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                db.Answer.Add(answer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PaperID = new SelectList(db.Paper, "PaperID", "PaperName", answer.PaperID);
            ViewBag.StuID = new SelectList(db.Student, "StuID", "StuName", answer.StuID);
            ViewBag.TeacherID = new SelectList(db.Teacher, "TeacherID", "TeacherName", answer.TeacherID);
            return View(answer);
        }

        // GET: Answers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answer.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaperID = new SelectList(db.Paper, "PaperID", "PaperName", answer.PaperID);
            ViewBag.StuID = new SelectList(db.Student, "StuID", "StuName", answer.StuID);
            ViewBag.TeacherID = new SelectList(db.Teacher, "TeacherID", "TeacherName", answer.TeacherID);
            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AnswerID,PaperID,StuID,TeacherID,AnswerScore,AnswerTime,SubmitTime,BatchTime,AnswerState")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(answer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PaperID = new SelectList(db.Paper, "PaperID", "PaperName", answer.PaperID);
            ViewBag.StuID = new SelectList(db.Student, "StuID", "StuName", answer.StuID);
            ViewBag.TeacherID = new SelectList(db.Teacher, "TeacherID", "TeacherName", answer.TeacherID);
            return View(answer);
        }

        // GET: Answers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answer.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Answer answer = db.Answer.Find(id);
            db.Answer.Remove(answer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region 老师审卷
        //老师审卷
        // GET: Answers/MyAnswer
        public ActionResult TeAnswer()
        {
            return View(db.Answer.Include("Student").ToList());
        }
        // GET: Answers/MyAnswerDetail/5
        //答题试卷
        public ActionResult TeAnswerDetail(int id)
        {
            //Answer answer = db.Answer.Find(id);
            Answer answer = db.Answer.Include("Paper").Include("Student").First(a => a.AnswerID == id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            ViewBag.answer = answer;
            //显示全部答题信息
            ViewBag.Details = db.Detail.Include("Topic").Include("Answer")
                .Where(a => a.AnswerID == id).ToList();
            return View(answer);
        }
        //提交审核：生成分数，设置答题状态为2
        public ActionResult Verify(int id)
        {
            Answer answer = db.Answer.Find(id);
            //计算分数
            foreach (Detail item in answer.Detail)
            {
                if (item.DetailAnswer == item.Topic.TopicAnswer)
                {
                    answer.AnswerScore += item.Topic.TopicScore;
                }
            }
            //重置分数和状态
            answer.AnswerState = 2;
            answer.BatchTime = DateTime.Now;
            db.Entry(answer).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("TeAnswer");
        }
        #endregion


        #region 学生答题方法
        //开始答题方法
        public ActionResult BeginAnswer(int PaperID,int? AnswerID)
        {
            if (AnswerID==null)//开始新考试
            {
                Answer answer = new Answer();
                answer.PaperID = PaperID;
                answer.StuID = (int)Session["LoginID"];//当前登录学生ID
                answer.TeacherID = 1;//设置默认审卷老师
                answer.AnswerScore = 0;
                answer.AnswerTime = DateTime.Now;//记录开始答题时间
                answer.AnswerState = 0;//开始答题状态
                db.Answer.Add(answer);//添加新答题试卷
                db.SaveChanges();
                return RedirectToAction("AnswerDetail", new { id = answer.AnswerID }); //打开答题试卷
            }
            else
            {//继续答题
                return RedirectToAction("AnswerDetail", new { id = AnswerID }); //打开答题试卷
            }
        }
  
        // GET: Answers/AnswerDetail/5
        //答题试卷详情
        public ActionResult AnswerDetail(int id)
        {
            //获取答题试卷;
            Answer answer = db.Answer.Include("Paper").Include("Student").First(a => a.AnswerID == id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            ViewBag.answer = answer;
            return View(answer);
        }
 
        //答题试卷显示当前考题，返回分部视图
        public ActionResult _Topic(int aid, int? sort)
        {
            int cursort = sort == null ? 1 : (int)sort; //如果没有考题序号，则显示第一题
            Answer answer = db.Answer.Include("Paper").Include("Student").First(a => a.AnswerID == aid);
            if (answer == null)
            {
                return HttpNotFound();
            }
            ViewBag.answer = answer;
            //获取题目信息集合,获取当前题目
            Topic topic = answer.Paper.Topic.ToList()[cursort - 1];
            ViewBag.topic = topic;
            ViewBag.sort = cursort;//当前答题序号
            ViewBag.count = answer.Paper.Topic.ToList().Count;//答题数
            //获取题目对应答题信息
            if (db.Detail.Where(d => d.AnswerID == aid && d.TopicID == topic.TopicID).Count() == 0)
            {
                //新建该题答案
                Detail detail = new Detail();
                detail.AnswerID = answer.AnswerID;
                detail.TopicID = topic.TopicID;
                detail.DetailAnswer = "";
                db.Detail.Add(detail);
                db.SaveChanges();
            }
            Detail curdetail = db.Detail.Include("Topic").Include("Answer").First(d => d.AnswerID == aid && d.TopicID == topic.TopicID);
            return PartialView(curdetail);
        }

        //答题显示所有考题，返回分部视图
        public ActionResult _AllDetailStu(int aid)
        {
            //显示全部答题信息
            ViewBag.Details = db.Detail.Include("Topic").Include("Answer")
                .Where(a => a.AnswerID == aid).ToList();
            return PartialView();
        }

        //答题提交每题答案
        public ActionResult SubmitAnswer(Detail detail, int sort, int count)
        {
            //保存答案
            db.Entry(detail).State = EntityState.Modified;
            db.SaveChanges();
            //获取下一题序号
            if (sort < count)
            {//显示下一题
                return RedirectToAction("_Topic", new { aid = detail.AnswerID, sort = sort + 1 });
            }
            else
            {  //答完最后一题，显示全部答题信息
                return RedirectToAction("_AllDetailStu", new { aid = detail.AnswerID });
            }
        }
 
        //交卷：设置答题状态为1
        public ActionResult Hand(int id)
        {
            Answer answer = db.Answer.Find(id);
            answer.AnswerState = 1;
            answer.AnswerTime = DateTime.Now;
            db.Entry(answer).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("MyAnswer");//跳转到我的试卷页面
        }
        #endregion

        #region 学生试卷
        //学生试卷列表
        // GET: Answers/MyAnswer
        public ActionResult MyAnswer()
        {
            int sid = (int)Session["LoginID"]; //查询当前登录学生的试卷
            var answers = db.Answer.Include("Student").Where(a => a.StuID == sid).ToList();
            return View(answers);
        }
        // GET: Answers/MyAnswerDetail/5
        //学生试卷答题详情
        public ActionResult MyAnswerDetail(int id)
        {
            //Answer answer = db.Answer.Find(id);
            Answer answer = db.Answer.Include("Paper").Include("Student").First(a => a.AnswerID == id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            ViewBag.answer = answer;
            //全部答题信息
            ViewBag.Details = db.Detail.Include("Topic").Include("Answer").Where(a => a.AnswerID == id).ToList();
            return View(answer);
        }
        #endregion


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
