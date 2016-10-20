using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        public string connection = "Data source=Web-Server\\SQLEXPRESS;initial catalog=Freshers; uid=fresher;pwd=fresher;";
        SqlConnection con = null;
        SqlCommand cmd = null;
        SqlDataReader dr;

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to kick-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your quintessential app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your quintessential contact page.";

            return View();
        }

        public ActionResult save(Class1 stu)
        {
            using (con = new SqlConnection(connection))
            {
                con.Open();
                con.CreateCommand();
                using (cmd = new SqlCommand("sp_int_d", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@name", stu.Name));
                    cmd.Parameters.Add(new SqlParameter("@username", stu.Username));
                    cmd.Parameters.Add(new SqlParameter("@password", stu.Password));
                    cmd.Parameters.Add(new SqlParameter("@gender", stu.Gender));

                    cmd.ExecuteNonQuery();
                }
            }
            return Json(new { success = "save success" });
        }
        public string savee(Class1 ee, Class1 ad)
        {
            raj(ee);
            ram(ad);
            return "";
        }
        public string raj(Class1 ee)
        {
            using (con = new SqlConnection(connection))
            {
                con.Open();
                con.CreateCommand();
                using (cmd = new SqlCommand("sp_asp", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", ee.Username));
                    cmd.Parameters.Add(new SqlParameter("@password", ee.Password));
                    cmd.ExecuteNonQuery();
                }

            }
            return "";
        }
        public string ram(Class1 ad)
        {
            using (con = new SqlConnection(connection))
            {
                con.Open();
                con.CreateCommand();
                using (cmd = new SqlCommand("asp_sp", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@name", ad.Name));
                    cmd.Parameters.Add(new SqlParameter("gender", ad.Gender));
                    cmd.ExecuteNonQuery();

                }
            }
            return "";
        }
        public string save1(string a, string b, string c, string d)
        {
            Class2 obj = new Class2();
            obj.name = a;
            obj.username = b;
            obj.password = c;
            obj.city = d;

            using (con = new SqlConnection(connection))
            {
                con.Open();
                con.CreateCommand();
                using (cmd = new SqlCommand("remo_sp", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@name", obj.name));
                    cmd.Parameters.Add(new SqlParameter("@username", obj.username));
                    cmd.Parameters.Add(new SqlParameter("@password", obj.password));
                    cmd.Parameters.Add(new SqlParameter("@city", obj.city));
                    cmd.ExecuteNonQuery();
                }
            }
            return "";
        }
        public string update(string name, string user, string pass, string city)
        {
            Class2 m = new Class2();
            m.name = name;
            m.username = user;
            m.password = pass;
            m.city = city;
            using (con = new SqlConnection(connection))
            {
                con.Open();
                con.CreateCommand();
                using (cmd = new SqlCommand("upda_sp", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@name", m.name));
                    cmd.Parameters.Add(new SqlParameter("@username", m.username));
                    cmd.Parameters.Add(new SqlParameter("@password", m.password));
                    cmd.Parameters.Add(new SqlParameter("@city", m.city));
                    cmd.ExecuteNonQuery();
                }
            }
            return "";
        }

        public ActionResult grid()
        {
            Class2 obj = new Class2();
            var a = aaa();
            var result = from c in a select new[] { c.name, c.username, c.password, c.city };
            return Json(new { aaData = result }, JsonRequestBehavior.AllowGet);
        }
        public List<Class2> aaa()
        {
            using (con = new SqlConnection(connection))
            {
                con.Open();
                con.CreateCommand();
                List<Class2> d = new List<Class2>();
                using (cmd = new SqlCommand("auto_int", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Class2 obj = new Class2();
                        obj.name = Convert.ToString(dr["name"]);
                        obj.username = Convert.ToString(dr["username"]);
                        obj.password = Convert.ToString(dr["password"]);
                        obj.city = Convert.ToString(dr["city"]);
                        d.Add(obj);

                    }
                    
                }
                return d;
            }
        }
        public ActionResult auto(string searchresult)
        { 
        var autoname=from c in autoe(searchresult)select new{label=c.city};
            return this.Json(autoname,JsonRequestBehavior.AllowGet);
        }
        public List<Class2> autoe(string searchresult)
        {
            Class2 m = new Class2();
            List<Class2> list = new List<Class2>();
            using (cmd = new SqlCommand("sp_aute", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@city", searchresult));
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    m = new Class2();
                    m.city = Convert.ToString(dr["city"]);
                    list.Add(m);
                }
                return list;
            }
        }
    }
}
