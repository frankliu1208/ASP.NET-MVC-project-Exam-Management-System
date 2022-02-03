using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//
using Exam.Models;
namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //Login by students
        public ActionResult LoginStudent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginStudent(string StuLoginName, string StuLoginPwd)
        {
            using (ExamDBEntities db = new ExamDBEntities())
            {
                Student student = db.Student.FirstOrDefault(t => t.StuLoginName == StuLoginName && t.StuLoginPwd == StuLoginPwd);
                if (student != null)
                {
                    Session["LoginID"] = student.StuID;
                    Session["LoginName"] = student.StuName;
                    Session["Type"] = "Student";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Username or password error!");
            }
            return View();
        }

        //Login by teacher
        public ActionResult LoginTeacher()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginTeacher(string TeacherLoginName, string TeacherLoginPwd)
        {
            using (ExamDBEntities db = new ExamDBEntities())
            {
                Teacher teacher = db.Teacher.FirstOrDefault(t => t.TeacherLoginName == TeacherLoginName && t.TeacherLoginPwd == TeacherLoginPwd);
                if (teacher != null)
                {
                    Session["LoginID"] = teacher.TeacherID;
                    Session["LoginName"] = teacher.TeacherName;
                    Session["Type"] = "Teacher";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Teacher's username or password error!");
            }
            return View();
        }

        //Log out
        public ActionResult Logout()
        {
            Session["LoginName"] = null;
            Session["Type"] = null;
            return RedirectToAction("Index");
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}