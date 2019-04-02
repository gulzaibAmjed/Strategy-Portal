using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ESS.Models;
using System.IO;
using System.Text;
using Egnitus;
using System.Data.Objects.SqlClient;
using System.Configuration;

namespace ESS.Controllers.STGYSTGY_SMAPEntities1
{
    public class HomeController : Controller
    {
        public static string connection = "";
        public static string projname = "";
        public static string tablename = "";
        public static string primarykeyattr = "";
        public static string primarykeyval = "";
        static string lString = ConfigurationManager.AppSettings["ConStr"];
        static string lUser = ConfigurationManager.AppSettings["DBUId"];
        static string lPwd = ConfigurationManager.AppSettings["DBPwd"];
        public static string ConnectionString = lString + "uid=" + lUser + ";pwd=" + lPwd + ";Max Pool Size=180;";

        public ActionResult Index()
        {
            if (Session.Keys.Count> 0)
            {
                ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
                STGY_SMAPEntities Db = new STGY_SMAPEntities();
                CLIENT_MASTER pm = new CLIENT_MASTER();
            
                //pm.Items = Db.CLIENT_MASTER.Select(x => new SelectListItem
                //{
                //    Value = x.Project_Name,
                //    Text = x.Project_Name
                //}).ToList();

                var model = new Clients();
                // get data from db.

                // Populate countries list.
                try
                {
                    model.Items = Db.CLIENT_MASTER.Select(x => new SelectListItem
                    {
                        //Value = x.Project_Name,
                        //Text = x.Project_Name
                    }).ToList();
                    var projects = Db.CLIENT_MASTER.SqlQuery("select * from CLIENT_MASTER").ToList();
                    ViewBag.count = projects.Count;
                    var users = Db.SEC_USER.SqlQuery("SELECT * FROM SEC_USER").ToList();
                    ViewBag.usercount = users.Count;
                    var devices = Db.DEVICE_MASTER.SqlQuery("SELECT * FROM DEVICE_MASTER").ToList();
                    ViewBag.deviceCount = devices.Count;
                }
                catch (Exception e)
                {

                }

                return View(model);

            }
            else
            {
                return RedirectToAction("Index", "Security");
            }
          
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Clients()
        {
            STGY_SMAPEntities Db = new STGY_SMAPEntities();

            var projects = Db.CLIENT_MASTER.SqlQuery("select * from CLIENT_MASTER").ToList();

            return View(projects);
        }
        [HttpGet]
        public ActionResult ClientsList()
        {
            STGY_SMAPEntities Db = new STGY_SMAPEntities();
            var projects = Db.CLIENT_MASTER.SqlQuery("select * from CLIENT_MASTER").ToList();
            return View(projects);
        }

        public ActionResult ClientsGrid()
        {
            STGY_SMAPEntities Db = new STGY_SMAPEntities();
            var projects = Db.CLIENT_MASTER.SqlQuery("select * from CLIENT_MASTER").ToList();
            return View(projects);
        }
        [HttpGet]
        public ActionResult EmailTemplate(string name)
        {
            if (name == null)
            {
                name = projname;
            }

            string connectionstring = "";
            string url = Request.Url.Segments.Last().ToString();
            STGY_SMAPEntities Db = new STGY_SMAPEntities();
            var projects = Db.CLIENT_MASTER.SqlQuery("select * from CLIENT_MASTER where Project_Name= '" + name + "' ").ToList();
            //connectionstring = projects[0].Connect_String.ToString();
            connection = connectionstring;
            SqlConnection conn = new SqlConnection(connectionstring);
            conn.Open();
            SqlDataAdapter lDataAdaptor = new SqlDataAdapter("select   ROW_NUMBER() OVER (ORDER BY EMAIL_TEMPLATE.TEMPLATE_ID ASC) AS No ,[LANGUAGE_NAME],TEMPLATE_CODE,MESSAGE_TITLE,MESSAGE_BODY from EMAIL_TEMPLATE,[SYS_LANGUAGE] where  [SYS_LANGUAGE].LANGUAGE_ID=EMAIL_TEMPLATE.LANGUAGE_ID", conn);
            DataTable dt = new DataTable();
            lDataAdaptor.Fill(dt);
            List<string[]> MyStringArrays = new List<string[]>();
            foreach (DataRow dr in dt.Rows)
            {
                MyStringArrays.Add(new string[] { dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), connectionstring,name});
            }
            ViewData["MattersTable"] = MyStringArrays;
            conn.Close();
            return View(MyStringArrays.AsEnumerable());
        }

        public ActionResult AddClients()
        {
            return View();
        }

        [HttpPost]

        public ActionResult CreateClients()
        {
            string query = string.Empty;
            string url = Request["URL"].ToString();
            string servername = Request["servername"].ToString();
            string database = Request["database"].ToString();
            string login = Request["login"].ToString();
            string password = Request["password"].ToString();

            TempData["servername"] = servername;
            TempData["database"] = database;

            string str = "Server=" + servername + ";Database=" + database + "; Persist Security Info=True; Connection Timeout=160;uid=" + login + ";pwd=" + password + ";Max Pool Size=180;";
            //string lConnectionString = lString + "uid=" + lUser + ";pwd=" + lPwd + ";Max Pool Size=180;";


            SqlConnection conn = new SqlConnection(str);
            connection = str;
            conn.Open();

            if (servername != "" && str != "")
            {
                try
                {
                    SqlCommand command = new SqlCommand("SMABP_AUTH_CODE", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.Add(new SqlParameter("", ""));
                    SqlDataReader ds = null;
                    ds = command.ExecuteReader();
                    if (ds.HasRows)
                    {
                        while (ds.Read())
                        {
                            TempData["companyname"] = ViewBag.companyname = ds.GetString(0);
                            TempData["code"] = ViewBag.code = ds.GetString(1);
                            ViewBag.status = "connected";
                            ViewBag.date = DateTime.Now;
                            //return View();
                        }
                    }

                }catch(Exception ex)
                {
                    throw ex;
                }
            }
            return View("AddClients");
            //return RedirectToAction("AddClients","Home");
        }

        
        public ActionResult SaveClient()
        {
            var userID = Session.Keys.Get(1);
            STGY_SMAPEntities Db = new STGY_SMAPEntities();
            CLIENT_MASTER objproject = new CLIENT_MASTER();
            objproject.CLIENT_NAME = TempData["companyname"].ToString();
            objproject.AUTH_CODE = TempData["code"].ToString();
            objproject.CREATE_DATE = DateTime.Now;
            objproject.CREATE_USER_ID = Convert.ToDecimal(userID);
            objproject.DATABASE_NAME = TempData["database"].ToString();
            objproject.SERVER_NAME = TempData["servername"].ToString();
            Db.CLIENT_MASTER.Add(objproject);
            Db.SaveChanges();
            //return View("Clients");
            return RedirectToAction("Clients", "Home");
        }

        public ActionResult EditProject(int id)
        {
            STGY_SMAPEntities Db = new STGY_SMAPEntities();
            var projects = Db.CLIENT_MASTER.SqlQuery("select * from CLIENT_MASTER where Project_Id='" + id + "'").ToList();
            CLIENT_MASTER objproj = new CLIENT_MASTER();
            foreach (var x in projects)
            {
                //objproj.Project_Id = x.Project_Id;
                //objproj.Project_Name = x.Project_Name;
                //objproj.Connect_String = x.Connect_String;
            }

            return View(objproj);
        }

        [HttpPost]
        public ActionResult EditProjectData()
        {
            string projectname = Request["Project_Name"].ToString();
            string connectionstring = Request["Connect_String"].ToString();
            string projectid = Request["Project_Id"].ToString();
            int pid = Int32.Parse(projectid);
            if (projectname != "" && connectionstring != "")
            {
                STGY_SMAPEntities Db = new STGY_SMAPEntities();
              //'  CLIENT_MASTER objproj = new CLIENT_MASTER();
                //objproj = ctx.Students.Where(s => s.StudentName == "New Student1").FirstOrDefault<Student>();
                //CLIENT_MASTER objproj = Db.CLIENT_MASTER.Where(x => x.Project_Id == pid).FirstOrDefault<CLIENT_MASTER>();
                //objproj.Project_Id = Int32.Parse(projectid);
                //objproj.Project_Name = projectname;
                //objproj.Connect_String = connectionstring;
                //Db.Entry(objproj).State = System.Data.EntityState.Modified;
                Db.SaveChanges();
            }
            return RedirectToAction("Project");
        }

        public ActionResult DeleteProject(int id)
        {
            STGY_SMAPEntities Db = new STGY_SMAPEntities();
            //CLIENT_MASTER objproj = Db.CLIENT_MASTER.Where(x => x.Project_Id == id).FirstOrDefault<CLIENT_MASTER>();
            //Db.CLIENT_MASTER.Remove(objproj);
            Db.SaveChanges();
            return RedirectToAction("Project");
        }

        public ActionResult EditTemplate(int id,string connectionstring,string name)
        {
            SqlConnection conn = new SqlConnection(connectionstring);
            connection = connectionstring;
            conn.Open();
            string langquery = "SELECT [LANGUAGE_ID],[LANGUAGE_NAME],[LANGUAGE_CODE] FROM [SYS_LANGUAGE]";
            SqlDataAdapter lDataAdaptor = new SqlDataAdapter("select * from EMAIL_TEMPLATE where TEMPLATE_CODE='" + id + "'", conn);
            DataTable dt = new DataTable();
            lDataAdaptor.Fill(dt);
            ViewData["projectname"] = name;
            ESS.Models.EmailTemplate objEmail = new Models.EmailTemplate();
            SqlDataAdapter lDataAdaptor1 = new SqlDataAdapter(langquery, conn);
            DataTable dt1 = new DataTable();
            lDataAdaptor1.Fill(dt1);
            var langtype = dt1;
          IList<SelectListItem> items = new List<SelectListItem>();
           

            //{
            //    new SelectListItem{Text = "California", Value = "B"},
            //    new SelectListItem{Text = "Alaska", Value = "B"},
            //    new SelectListItem{Text = "Illinois", Value = "B"},
            //    new SelectListItem{Text = "Texas", Value = "B"},
            //    new SelectListItem{Text = "Washington", Value = "B"}

            //};
           
        
            //ViewData["People"] = selectList;
            foreach(DataRow dr in dt.Rows)
            {
                objEmail.TEMPLATE_ID = Int32.Parse(dr[0].ToString());
                objEmail.TEMPLATE_CODE = Int32.Parse(dr[1].ToString());
                objEmail.LANGUAGE_ID = Int32.Parse(dr[2].ToString());
                objEmail.MESSAGE_TITLE = dr[3].ToString();
                //objEmail.RichText1 = dr[4].ToString().Trim();
                objEmail.PageContent = dr[4].ToString().Trim();
                ViewData["data"] = objEmail.PageContent;
                objEmail.MESSAGE_SIGNATURE = dr[5].ToString();
                objEmail.MESSAGE_SENDER = dr[6].ToString();
            }
            foreach (DataRow dr in dt1.Rows)
            {
                SelectListItem SLI = new SelectListItem();
                SLI.Text = dr[1].ToString();
                SLI.Value = dr[0].ToString();
                if (SLI.Value == objEmail.LANGUAGE_ID.ToString())
                {
                    SLI.Selected = true;
                }
                items.Add(SLI);
            }

            ViewData["Lang"] = items;
            return View(objEmail);
        }

        [HttpPost,ValidateInput(false)]
      
        public ActionResult UpdateEmailTemplate()
        {
            string jsonData = new StreamReader(Request.InputStream).ReadToEnd();
            StringBuilder s = new StringBuilder();
            
            string templateid = Request["TEMPLATE_ID"].ToString();
            string templatecode = Request["TEMPLATE_CODE"].ToString();
            string langid = Request["DDLLang"].ToString();
            string msgtitle = Request["MESSAGE_TITLE"].ToString();
            string msgbody = Request["PageContent"].ToString();
            string msgsignature = Request["MESSAGE_SIGNATURE"].ToString();
            string msgsender = Request["MESSAGE_SENDER"].ToString();
            string projectname = Request["Project_Id"].ToString();
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            string query = "update [EMAIL_TEMPLATE] SET";
            query += "[TEMPLATE_CODE] = '" + templatecode + "'";
            query += ",[LANGUAGE_ID] = '" + langid + "'";
            query += ",[MESSAGE_TITLE] = '" + msgtitle + "'";
            query += ",[MESSAGE_BODY] = '" + msgbody + "'";
            query += ",[MESSAGE_SIGNATURE] = '" + msgsignature + "'";
            query += ",[MESSAGE_SENDER] = '" + msgsender + "'";
            query += "where TEMPLATE_ID = '" + templateid + "'";
            SqlCommand sqlcomm = new SqlCommand(query, conn);
            int status = sqlcomm.ExecuteNonQuery();
            if (status == 0)
            {
                TempData["msg"] = "<script>alert('Record cannot be updated ');</script>";
            }
            else
            {
                TempData["msg"] = "<script>alert('Record updated succesfully');</script>";
            }
            return RedirectToAction("EmailTemplate", new { name = projectname });
        }

        public ActionResult UserList()
        {
            STGY_SMAPEntities Db = new STGY_SMAPEntities();
            var users = Db.SEC_USER.SqlQuery("SELECT * FROM SEC_USER").ToList();
            return View(users);
        }

        public ActionResult DeviceList()
        {
            STGY_SMAPEntities Db = new STGY_SMAPEntities();

            var model = new Clients();
            var data = new Clients();
           

            model.Items = Db.CLIENT_MASTER.Select(x => new SelectListItem
            {
                Value = SqlFunctions.StringConvert((double)x.CLIENT_ID),
                Text = x.CLIENT_NAME
            }).ToList();

            data.Items = Db.DEVICE_MASTER.Select(x => new SelectListItem
            {
                Value = SqlFunctions.StringConvert((double)x.CLIENT_ID),
                Text = x.DEVICE_NAME
            }).ToList();

            foreach (var item in model.Items)
            {
                model.client_ID = item.Text;
                model.client_Name = item.Value;
            }

            
            

            foreach (var item in data.Items)
	            {
                    data.client_ID = item.Text;
                    data.client_Name = item.Value;
	            }

            

            STGY_SMAPEntities Dbb = new STGY_SMAPEntities();
            var devices = Dbb.DEVICE_MASTER.SqlQuery("SELECT * FROM DEVICE_MASTER").ToList();
            return View(devices);
        }

        public ActionResult LoadSecurityTables(string name)
        {
            string connectionstring = "";
            string url = Request.Url.Segments.Last().ToString();
            STGY_SMAPEntities Db = new STGY_SMAPEntities();
            var projects = Db.CLIENT_MASTER.SqlQuery("select * from CLIENT_MASTER where Project_Name= '" + name + "' ").ToList();
            //connectionstring = projects[0].Connect_String.ToString();
            SqlConnection conn = new SqlConnection(connectionstring);
            conn.Open();
            SqlDataAdapter lDataAdaptor = new SqlDataAdapter("select * from EMAIL_TEMPLATE", conn);
            DataTable dt = new DataTable();
            lDataAdaptor.Fill(dt);
            SqlDataAdapter lDataAdaptor1 = new SqlDataAdapter("select * from sys.tables where name like 'SYS_%'", conn);
            DataTable dtsec = new DataTable();
            lDataAdaptor1.Fill(dtsec);
            ViewBag.Emailcount = dt.Rows.Count;
            ViewBag.seccount = dtsec.Rows.Count;
            SqlDataAdapter lDataAdaptor2 = new SqlDataAdapter("select * from sys.tables where name like 'LU_%'", conn);
            DataTable dtlu = new DataTable();
            lDataAdaptor2.Fill(dtlu);
            ViewBag.lucount = dtlu.Rows.Count;
            conn.Close();
            projname = name;
            connection = connectionstring;
            var model = new Clients();
            // get data from db.

            // Populate countries list.
            model.Items = Db.CLIENT_MASTER.Select(x => new SelectListItem
            {
                //Value = x.Project_Name,
                //Text = x.Project_Name
            }).ToList();
            return View(model);
           
        }

        public ActionResult SecurityObjectsList(string objtype)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            string query = "";
        
            if (objtype == "1")
            {
                query="select * from sys.tables where name like 'SYS_%'";
                @ViewBag.tablecaption = "System";
            }
            else
            {
                query = "select * from sys.tables where name like 'LU_%'";
                @ViewBag.tablecaption = "Look Up";
            }
            SqlDataAdapter lDataAdaptor = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            lDataAdaptor.Fill(dt);
            List<DataRow> list = dt.AsEnumerable().ToList();
            lDataAdaptor.Fill(dt);
            return View(list);
        
        }

        public ActionResult SecurityData(string name)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            string query = "select * from " + name + " ";
            SqlDataAdapter lDataAdaptor = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            lDataAdaptor.Fill(dt);
            List<DataRow> list = dt.AsEnumerable().ToList();
            conn.Close();
            //ViewData["MattersTable"] = dt.AsEnumerable().ToList();
            tablename = name;
            primarykeyattr = dt.Columns[0].Caption.ToString();
            return View(dt);
        }

        [HttpPost]
        public ActionResult GenerateView(string colval, string colh)
        {
            //string colval = Request["colval"].ToString();
            //string colh = Request["colh"].ToString();
            string[] values = colval.Split(',');
            for (int i = 0; i < values.Length-1; i++)
            {
                values[i] = values[i].Trim();
            }
            string[] colhead = colh.Split('\n');
            //string[] columnhead = new string[0];
            var columnhead = new List<string>();
           
            for (int i = 0; i < colhead.Length; i++)
            {
                if (colhead[i].Trim() != "" && colhead[i].Trim() != "Actions")
                    columnhead.Add(colhead[i].Trim());
            }
          
            var data = new List<HTMLTemplate>();
            string[] arrStrings = columnhead.ToArray();
            for (int i = 0; i < arrStrings.Length; i++)
            {
                HTMLTemplate newtemplate = new HTMLTemplate();
                newtemplate.FIELD_LABEL = arrStrings[i];
                newtemplate.FIELD_VALUE = values[i];
                data.Add(newtemplate);
            }
            primarykeyval = data[0].FIELD_VALUE.ToString();

            return View(data);
        }

        [HttpPost]

        public ActionResult UpdateFieldValue()
        {
            StringBuilder s = new StringBuilder();
            string query = "";
            foreach (string key in Request.Form.Keys)
            {
                s.AppendLine(key + ": " + Request.Form[key]);
              
                query += "" + key + "=";
                if (key.Contains("ID"))
                {
                    query += "" + Request.Form[key] + "";
                }
                else
                {
                    query += "'" + Request.Form[key] + "'";
                }
              
                query += ",";
              
            }
            query.TrimEnd(',');
            string qtr = query.TrimEnd(',');
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
          
            string sqlstr = "Update " + tablename + " SET " + qtr + " Where " + primarykeyattr + " = " + primarykeyval;
            SqlCommand sqlcomm = new SqlCommand(sqlstr, conn);
            int status = sqlcomm.ExecuteNonQuery();
            if (status == 0)
            {
                TempData["msg"] = "<script>alert('Record cannot be updated ');</script>";
            }
            else
            {
                TempData["msg"] = "<script>alert('Record updated succesfully');</script>";
            }
            

            return RedirectToAction("SecurityData", new { name = tablename });
        }

        public ActionResult AddTemplate()
        {
            SqlConnection conn = new SqlConnection(connection);
           // connection = connectionstring;
            conn.Open();
            string langquery = "SELECT [LANGUAGE_ID],[LANGUAGE_NAME],[LANGUAGE_CODE] FROM [SYS_LANGUAGE]";

            SqlDataAdapter lDataAdaptor1 = new SqlDataAdapter(langquery, conn);
            DataTable dt1 = new DataTable();
            lDataAdaptor1.Fill(dt1);
            var langtype = dt1;
            IList<SelectListItem> items = new List<SelectListItem>();
            int countselect = 0;

            foreach (DataRow dr in dt1.Rows)
            {
                SelectListItem SLI = new SelectListItem();
                SLI.Text = dr[1].ToString();
                SLI.Value = dr[0].ToString();
                countselect++;
                if (countselect == 1)
                {
                    SLI.Selected = true;
                }
             
                items.Add(SLI);
            }

            ViewData["Lang"] = items;
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult AddEmailTemplate()
        {
            string jsonData = new StreamReader(Request.InputStream).ReadToEnd();
            StringBuilder s = new StringBuilder();

            string templateid = Request["TEMPLATE_ID"].ToString();
            string templatecode = Request["TEMPLATE_CODE"].ToString();
            string langid = Request["DDLLang"].ToString();
            string msgtitle = Request["MESSAGE_TITLE"].ToString();
            string msgbody = Request["PageContent"].ToString();
            string msgsignature = Request["MESSAGE_SIGNATURE"].ToString();
            string msgsender = Request["MESSAGE_SENDER"].ToString();
            string projectname = Request["Project_Id"].ToString();
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            string query = "INSERT INTO [dbo].[EMAIL_TEMPLATE]([TEMPLATE_CODE],[LANGUAGE_ID],[MESSAGE_SENDER],[MESSAGE_TITLE],[MESSAGE_BODY],[MESSAGE_SIGNATURE]) VALUES ( ";
            query += "'" + templatecode + "'";
            query += ",'" + langid + "'";
            query += ",'" + msgsender + "'";
            query += ",'" + msgtitle + "'";
            query += ",'" + msgbody + "'";
            query += ",'" + msgsignature + "'";
            
            query += " )";
            SqlCommand sqlcomm = new SqlCommand(query, conn);
            int status = sqlcomm.ExecuteNonQuery();
            if (status == 0)
            {
                TempData["msg"] = "<script>alert('Record cannot be added ');</script>";
            }
            else
            {
                TempData["msg"] = "<script>alert('Record added succesfully');</script>";
            }
            return RedirectToAction("EmailTemplate", new { name = projectname });
            
        }
        public ActionResult DeleteTemplate(int id,string connectionstring,string name)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            string query = "delete from EMAIL_TEMPLATE where TEMPLATE_CODE='" + id + "'";
            SqlCommand sqlcomm = new SqlCommand(query, conn);
            int status = sqlcomm.ExecuteNonQuery();
            return RedirectToAction("EmailTemplate");


        }

        public ActionResult DecryptPassword()
        {

           
            return View();
        }

        public ActionResult GetDecrypt()
        {
           string test =  Egnitus.License.InterSecurity.MyPasswordHash("TEST");

            return View();

        }

    }




    
}
