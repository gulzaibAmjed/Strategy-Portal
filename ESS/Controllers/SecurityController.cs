using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Egnitus.License;
using ESS.Models;
using Egnatium.License;
using System.Data.SqlClient;
using System.Configuration;


namespace ESS.Controllers
{
    public class SecurityController : Controller
    {
        //
        // GET: /Security/
        static string lString = ConfigurationManager.AppSettings["ConStr"];
        static string lUser = ConfigurationManager.AppSettings["DBUId"];
        static string lPwd = ConfigurationManager.AppSettings["DBPwd"];
        static string ConnectionString = lString + "uid=" + lUser + ";pwd=" + lPwd + ";Max Pool Size=180;";

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login()
        {
            string username = Request["username"].ToString();
            string password = Request["password"].ToString();
           string txtpwd = Egnitus.License.InterSecurity.MyPasswordHash(password);

            //STGY_SMAPEntities1 Db = new STGY_SMAPEntities1();
           STGY_SMAPEntities Db = new STGY_SMAPEntities();

            var data = Db.SEC_USER.SqlQuery("select * from sec_user where User_Name='" + username + "' and Pwd_Txt = '" + Egnatium.License.InterSecurity.MyPasswordHash(password) + "'").ToList();
            ViewBag.error = "";
            if (data.Count == 0)
            {
                ViewBag.error = "Wrong Credentials";
                //return View();
                return RedirectToAction("Index","Security");
            }
            else
            {
                Session.Add(username, "2");
                Session.Add("1", "1");
                return RedirectToAction("Index", "Home");
            }
           
            //return Json("true");
        }

        public ActionResult Logout()
        {
            Session.Remove("USER_ID");
            return RedirectToAction("Index");
        }

        public ActionResult ForgotPassword()
        {
            return View();
            //return RedirectToAction("ForgotPassword", "Security");
        }

        public ActionResult CancelPassword()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangePassword()
        {
            return View();
            //return RedirectToAction("ChangePassword","Home");
        }

        public ActionResult UpdatePassword()
        {
            try
            {
                string username = Request["username"].ToString();
                string password = Request["pwd1"].ToString();

                //if (username == "" || password == "")
                //{
                //    return RedirectToAction("Index");
                //}
                var name = Session.Keys.Get(0);
                STGY_SMAPEntities Db = new STGY_SMAPEntities();
                var data = Db.SEC_USER.SqlQuery("select * from sec_user where Pwd_Txt = '" + Egnatium.License.InterSecurity.MyPasswordHash(username) + "'").ToList();
                if (data.Count == 0)
                {
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    SqlConnection conn = new SqlConnection(ConnectionString);
                    conn.Open();
                    string sqlstr = "update sec_user set Pwd_Txt ='" + Egnatium.License.InterSecurity.MyPasswordHash(password) + "' where User_Name = '" + name + "'";
                    SqlCommand sqlcomm = new SqlCommand(sqlstr, conn);
                    int status = sqlcomm.ExecuteNonQuery();

                    //Db.sec_user.SqlQuery("update sec_user set Pwd_Txt ='" + Egnatium.License.InterSecurity.MyPasswordHash(password) + "' where User_Name = '" + name + "'").ToList();
                    return RedirectToAction("Index", "Home");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            

           // return RedirectToAction("index", "Home");
        }

    }
}
