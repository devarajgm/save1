using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using IntPractice.Models;

namespace IntPractice.Controllers
{
    public class StudentController : Controller
    {
        //
        // GET: /Student/

        public ActionResult Student()
        {
            ViewData["stuobj"] = getAllDetails();
            //Patient data = TempData["mydata"] as Patient;
            return View();
           
        }
        public string namevalidation(Student st)
        {
            string result = "";
            string str = st.Studentname;          
            try
            {
                if (str != null)
                {
                
                    for (int i = 0; i < str.Length; i++)
                    {
                        int ch = (int)str[i];
                        // Code to Allow Alphabets Back Spaces and Spaces.
                        if ((ch < 7 && ch > 0) || (ch < 32 && ch > 9) || (ch < 48 && ch > 32) || (ch < 97 && ch > 90) || (ch < 65 && ch > 47) || (ch < 127 && ch > 122))
                        {
                            result = "Enter Albhabets Only";
                        }
                        else
                        {
                            result = "Success";
                        }
                    }
                }               

                else
                {
                    result = "Enter the Value";
                }
            }
            catch (Exception e)
            {
               
            }
            return result;
            
            
        }
        public string mobileValidation(Student stud)
        {
            string str = stud.mobile;
            string result = "";
            if (str != null)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    int ch = (int)str[i];
                    // Code to Allow Alphabets Back Spaces and Spaces.
                    if (ch >= 48 && ch <= 57)                    
                    {
                     
                        result = "Success"; 
                        
                    }
                    else
                    {
                        result = "Enter Numbers Only";
                       
                    }
                }

            }

            return result;
        }
        public ActionResult save(Student student)
        {   
            if (namevalidation(student) == "Success" && mobileValidation(student)=="Success")
            {
                string connectionstring = "Data Source=WEB-SERVER\\SQLEXPRESS;Initial Catalog=Freshers;Persist Security Info=True;User ID=fresher;Password=fresher";
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_insertstuddetails", connection))
                    {
                        string part = student.imgurl;
                        string part1 = part.Substring(0, 4);
                        if (part1 == "data")
                        {
                            string pattern = @"data:image/(jpeg|jpg|gif);base64,";
                            string filename = System.Text.RegularExpressions.Regex.Replace(part, pattern, String.Empty);
                            byte[] image = Convert.FromBase64CharArray(filename.ToCharArray(), 0, filename.Length);
                            student.studimage = image;

                        }
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@Studentname", student.Studentname));
                        command.Parameters.Add(new SqlParameter("@Gender", student.Gender));
                        command.Parameters.Add(new SqlParameter("@Age", student.Age));
                        command.Parameters.Add(new SqlParameter("@Qualification", student.Qualification));
                        command.Parameters.Add(new SqlParameter("@Department", student.Department));
                        command.Parameters.Add(new SqlParameter("@States", student.States));
                        command.Parameters.Add(new SqlParameter("@Dob", student.Dob));
                        command.Parameters.Add(new SqlParameter("@Addresss", student.Addresss));
                        command.Parameters.Add(new SqlParameter("@studimage", student.studimage));
                        command.Parameters.Add(new SqlParameter("@email", student.email));
                        command.Parameters.Add(new SqlParameter("@mobile", student.mobile));
                        IDbDataParameter param = new SqlParameter("@@guid", DBNull.Value);
                        param.DbType = DbType.Int64;
                        param.Direction = ParameterDirection.Output;
                        command.Parameters.Add(param);
                        command.ExecuteNonQuery();
                        student.Studentid = Convert.ToInt16(param.Value);
                    }
                }
                return Json(new { success = "Saved Successfully" });
            }           
            else if (namevalidation(student) != "Success")
            {
                return Json(new { success = namevalidation(student) });
            }
            else if (mobileValidation(student) != "Success")
            {
                return Json(new { success = mobileValidation(student) });
            }
            else
            {
                return Json(new { success = "" });
            }
            
        }
        public string update(Student student)
        {

            string connectionstring = "Data Source=WEB-SERVER\\SQLEXPRESS;Initial Catalog=Freshers;Persist Security Info=True;User ID=fresher;Password=fresher";
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_updatestuddetails", connection))
                {
                    string part = student.imgurl;
                    string part1 = part.Substring(0, 4);
                    if (part1 == "data")
                    {
                        string pattern = @"data:image/(jpeg|jpg|gif);base64,";
                        string filename = System.Text.RegularExpressions.Regex.Replace(part, pattern, String.Empty);
                        byte[] image = Convert.FromBase64CharArray(filename.ToCharArray(), 0, filename.Length);
                        student.studimage = image;

                    }
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Studentid", student.Studentid));
                    command.Parameters.Add(new SqlParameter("@Studentname", student.Studentname));
                    command.Parameters.Add(new SqlParameter("@Gender", student.Gender));
                    command.Parameters.Add(new SqlParameter("@Age", student.Age));
                    command.Parameters.Add(new SqlParameter("@Qualification", student.Qualification));
                    command.Parameters.Add(new SqlParameter("@Department", student.Department));
                    command.Parameters.Add(new SqlParameter("@States", student.States));
                    command.Parameters.Add(new SqlParameter("@Dob", student.Dob));
                    command.Parameters.Add(new SqlParameter("@Addresss", student.Addresss));
                    command.Parameters.Add(new SqlParameter("@studimage", student.studimage));
                    command.Parameters.Add(new SqlParameter("@email", student.email));
                    command.Parameters.Add(new SqlParameter("@mobile", student.mobile));                    
                    command.ExecuteNonQuery();                    
                }
            }
            if (student.Studentid > 0)
            {
                return "Updated Successfully";
            }
            else
            {
                return "Failed to Update";
            }

        }

        public List<Student> getAllDetails()
        {
            Student student = null;
            List<Student> list = new List<Student>();
            string connection = "Data Source=WEB-SERVER\\sqlexpress;Initial Catalog=Freshers; User Id=fresher;Pwd=fresher; Connect Timeout=0";
            using (SqlConnection connec = new SqlConnection(connection))
            {
                connec.Open();
                using (SqlCommand command = new SqlCommand("select * from StudCounseling", connec))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        student = new Student();
                        byte[] image = (byte[])reader["studimage"];
                        string a1 = Convert.ToBase64String(image);
                        string a2 = String.Format("data:image/jpg;base64,{0}", a1);
                        student.imgurl = a2;
                        student.Studentid = Convert.ToInt64(reader["Studentid"]);
                        student.Studentname = Convert.ToString(reader["Studentname"]);
                        student.Gender = Convert.ToString(reader["Gender"]);
                        student.Age = Convert.ToInt64(reader["Age"]);
                        student.Qualification = Convert.ToString(reader["Qualification"]);
                        student.Department = Convert.ToString(reader["Department"]);
                        student.States = Convert.ToString(reader["States"]);
                        student.Dob = Convert.ToString(reader["Dob"]);
                        student.email = Convert.ToString(reader["email"]);
                        student.Addresss = Convert.ToString(reader["Addresss"]);
                        student.mobile = Convert.ToString(reader["mobile"]);
                        list.Add(student);

                    }

                }
                connec.Close();
            }
            return list;
        }
        public ActionResult dataForDataTable()
        {
            var ss = getAllDetails();
            var dd = from ff in ss 
                     select new[] 
          { ff.Studentid.ToString(),
            ff.Studentname,
            ff.Gender,
            ff.Dob,
            ff.Age.ToString(),
            ff.Qualification,                       
            ff.Department,            
            ff.States,
            ff.Addresss,
            ff.email,
            ff.mobile,
           };
            return Json(new { aaData = dd }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult dataTableFilter(string str)
        {
            var ss = getAllDetails();
            var dd = from ff in ss where ff.Gender.Contains(str)
                     select new[] 
          { ff.Studentid.ToString(),
            ff.Studentname,
            ff.Gender,
            ff.Dob,
            ff.Age.ToString(),
            ff.Qualification,                       
            ff.Department,            
            ff.States,
            ff.Addresss,
            ff.email,
            ff.mobile,
           };
            return Json(new { aaData = dd }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getAllDetails2()
        {
            Student student = null;
            List<Student> list = new List<Student>();
            string connection = "Data Source=WEB-SERVER\\sqlexpress;Initial Catalog=Freshers; User Id=fresher;Pwd=fresher; Connect Timeout=0";
            using (SqlConnection connec = new SqlConnection(connection))
            {
                connec.Open();
                using (SqlCommand command = new SqlCommand("select * from StudCounseling", connec))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        student = new Student();
                        byte[] image = (byte[])reader["studimage"];
                        string a1 = Convert.ToBase64String(image);
                        string a2 = String.Format("data:image/jpg;base64,{0}", a1);
                        student.imgurl = a2;
                        student.Studentid = Convert.ToInt64(reader["Studentid"]);
                        student.Studentname = Convert.ToString(reader["Studentname"]);
                        student.Gender = Convert.ToString(reader["Gender"]);
                        student.Age = Convert.ToInt64(reader["Age"]);
                        student.Qualification = Convert.ToString(reader["Qualification"]);
                        student.Department = Convert.ToString(reader["Department"]);
                        student.States = Convert.ToString(reader["States"]);
                        student.Dob = Convert.ToString(reader["Dob"]);
                        student.email = Convert.ToString(reader["email"]);
                        student.Addresss = Convert.ToString(reader["Addresss"]);
                        student.mobile = Convert.ToString(reader["mobile"]);
                        list.Add(student);

                    }

                }
                connec.Close();
            }
            var ss = from xx in list select new[] { xx.Studentid.ToString(), xx.Studentname, xx.Gender, xx.Dob, xx.Age.ToString(), xx.Qualification, xx.Department, xx.States, xx.Addresss, xx.email, xx.mobile };
            return Json(new { adata=ss }, JsonRequestBehavior.AllowGet);
        }
        public string getImage(Int64  id)
        {
            Student student = null;
            List<Student> list = new List<Student>();
            string connection = "Data Source=WEB-SERVER\\sqlexpress;Initial Catalog=Freshers; User Id=fresher;Pwd=fresher; Connect Timeout=0";
            using (SqlConnection connec = new SqlConnection(connection))
            {
                connec.Open();
                using (SqlCommand command = new SqlCommand("select studimage from StudCounseling where Studentid=@id", connec))
                {                   
                    command.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        student = new Student();
                        byte[] image = (byte[])reader["studimage"];
                        string a1 = Convert.ToBase64String(image);
                        string a2 = String.Format("data:image/jpg;base64,{0}", a1);
                        student.imgurl = a2;
                    }
                }
            }
            return student.imgurl;
        }
        //
        // GET: /Student/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Student/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Student/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Student/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Student/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Student/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Student/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
