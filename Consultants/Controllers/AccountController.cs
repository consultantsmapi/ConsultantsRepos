using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Consultants.Models;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Bson;
using WebMatrix.WebData;
using System.Net.Mail;
using System.Web.Helpers;
using MongoDB.Bson.IO;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using MongoDB.Driver.GridFS;

namespace Consultants.Controllers
{
    public class AccountController : Controller
    {
        IMongoQuery usersQuery, consulQuery;
        OurDbContext Context = new OurDbContext();

        public ActionResult Index()
        {
            var collection = Context.Database.GetCollection<TextBox>("TextBox");
            return View();

        }

        [HttpPost]
        public ActionResult Index(double xPos, double yPos)
        {
            var collection = Context.Database.GetCollection<TextBox>("TextBox");

            return View();
        }
        public ActionResult UserRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserRegister(UserAccount _useraccount)
        {
            if (ModelState.IsValid)
            {
                var collection = Context.Database.GetCollection<UserAccount>("Users");
                usersQuery = Query<UserAccount>.Where(s => s.UserName == _useraccount.UserName);
                var model = collection.FindOne(usersQuery);

                if (model == null)
                {
                    string name = Request.Form["check"];

                    if (name == "true")
                    {
                        _useraccount.CheckBox = 1;
                    }

                    Context.Users.Insert(_useraccount);
                    ModelState.Clear();
                    TempData["Message"] = _useraccount.FirstName + " " + _useraccount.LastName + " נרשם בהצלחה";

                    return RedirectToAction("Login");
                }

                else
                {
                    TempData["Message"] = "שם משתמש תפוס";
                }
            }
            return View();
        }

        public ActionResult ConsultantsRegister()
        {

            string file = Server.MapPath("~/Content/Subjects.txt");
            string x = System.IO.File.ReadAllText(file, System.Text.Encoding.UTF8);
         
            List<SubjectConsultant> listofsubjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SubjectConsultant>>(System.IO.File.ReadAllText(file));
            ViewBag.SubjectList = listofsubjects;        
            return View();
        }

        [HttpPost]
        public ActionResult ConsultantsRegister(ConsultantsAccount _useraccount, HttpPostedFileBase file1, HttpPostedFileBase file2,String checkboxSelectCombo)
        {
          

                var collection = Context.Database.GetCollection<ConsultantsAccount>("Consultants");
                consulQuery = Query<ConsultantsAccount>.Where(s => s.UserName == _useraccount.UserName);
                var model = collection.FindOne(consulQuery);
        
         
            if (model == null)
                {
                    string name = Request.Form["check"];

                    if (name == "true")
                    {
                        _useraccount.CheckBox = 1;
                    }
                    if(file1!=null)
                    {

                            ObjectId fileID1 = ObjectId.GenerateNewId();
                            _useraccount.Documents1 = fileID1.ToString();
                          var options = new MongoGridFSCreateOptions
                        {
                              Id= fileID1,
                              ContentType = file1.ContentType
                        };
                        Context.Database.GridFS.Upload(file1.InputStream, file1.FileName, options);
                    }
                if (file2 != null)
                {

                    ObjectId fileID2 = ObjectId.GenerateNewId();
                    _useraccount.Documents2 = fileID2.ToString();
                    var options = new MongoGridFSCreateOptions
                    {
                        Id = fileID2,
                        ContentType = file2.ContentType
                    };
                    Context.Database.GridFS.Upload(file2.InputStream, file2.FileName, options);
                }
                Context.Consultants.Insert(_useraccount);
                    ModelState.Clear();
                    TempData["Message"] = _useraccount.FirstName + " " + _useraccount.LastName + " נרשם בהצלחה";
                    return RedirectToAction("Login");
                }

            
           

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
            
            var usersCollection = Context.Database.GetCollection<UserAccount>("Users");
            var consultantsCollection = Context.Database.GetCollection<ConsultantsAccount>("Consultants");
            usersQuery = Query<UserAccount>.Where(s => s.UserName == user.UserName && s.Password == user.Password);
            consulQuery = Query<ConsultantsAccount>.Where(s => s.UserName == user.UserName && s.Password == user.Password);
            var model1 = usersCollection.FindOne(usersQuery);
            var model2 = consultantsCollection.FindOne(consulQuery);
    
            if (model1 != null || model2 != null)
            {
                if (model1 != null)
                {
                    Session["UserName"] = model1.UserName.ToString();
                    return RedirectToAction("SearchConsultants");
                }

                if (model2 != null)
                {
                    Session["UserName"] = model2.UserName.ToString();
                    return RedirectToAction("SearchConsultants");
                }
            }

            else
            {
                TempData["Message"] = "שם משתמש או סיסמא שגויים";
            }

            return View();
        }

        public ActionResult ChangePassword()
        {
            if (Session["UserName"] != null)
            {
                return View();
            }
            else
                return RedirectToAction("Login");

        }

        [HttpPost]
        public ActionResult ChangePassword(UserAccount user)
        {
            if (Session["UserName"] != null)
            {
                IMongoUpdate passwordUpdate;
                var userCollection = Context.Database.GetCollection<UserAccount>("Users");
                usersQuery = Query<UserAccount>.Where(s => s.UserName == user.UserName && s.Password == user.Password && s.Password == user.ConfirmPassword);
                var model = userCollection.FindOne(usersQuery);

                if (model != null)
                {
                    string name = Request.Form["New Password"];
                    passwordUpdate = Update.Set("ConfirmPassword", name);
                    userCollection.Update(usersQuery, passwordUpdate);
                    passwordUpdate = Update.Set("Password", name);
                    userCollection.Update(usersQuery, passwordUpdate);
                    Session["UserName"] = model.UserName.ToString();
                    TempData["Message"] = "הסיסמא שונתה בהצלחה";

                    return RedirectToAction("SearchConsultants");
                }

                else
                {
                    var consultantsCollection = Context.Database.GetCollection<ConsultantsAccount>("Consultants");
                    usersQuery = Query<ConsultantsAccount>.Where(s => s.UserName == user.UserName && s.Password == user.Password && s.Password == user.ConfirmPassword);
                    var consultantsModel = consultantsCollection.FindOne(usersQuery);

                    if (consultantsModel != null)
                    {
                        string name = Request.Form["New Password"];
                        passwordUpdate = Update.Set("ConfirmPassword", name);
                        consultantsCollection.Update(usersQuery, passwordUpdate);
                        passwordUpdate = Update.Set("Password", name);
                        consultantsCollection.Update(usersQuery, passwordUpdate);
                        Session["UserName"] = consultantsModel.UserName.ToString();
                        TempData["Message"] = "הסיסמא שונתה בהצלחה";

                        return RedirectToAction("SearchConsultants");
                    }

                    else
                    {
                        ModelState.AddModelError("", "שם משתמש או סיסמא שגויים");
                        return View();
                    }
                }

            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        
        public ActionResult SearchConsultants()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login");
            }
                var usersCollection = Context.Database.GetCollection<UserAccount>("Users");
                var consultantsCollection = Context.Database.GetCollection<ConsultantsAccount>("Consultants");
                usersQuery = Query<UserAccount>.Where(s => s.UserName == Session["UserName"].ToString());
                consulQuery = Query<ConsultantsAccount>.Where(s => s.UserName == Session["UserName"].ToString());
                var model1 = usersCollection.FindOne(usersQuery);
                var model2 = consultantsCollection.FindOne(consulQuery);
                string type = null;
                if (model1 != null || model2 != null)
                {
                    if (model1 != null)
                    {
                        type = "user";
                    }

                    if (model2 != null)
                    {
                        type = "consultant";

                    }

                }
            if (TempData["Accounts"] != null)
            {
                ViewBag.my_cons = TempData["Accounts"];
            }
            else
            {
                ViewBag.type = type;
                var sizecons = 0;
                var consultants = new List<ConsultantsAccount>();
                var usersCursor = consultantsCollection.FindAll().ToList();
                foreach (var item in usersCursor)
                {
                    consultants.Add(item);
                    sizecons++;
                }
                ViewBag.my_cons = consultants;
            }
                return View();
        }

        [HttpPost]
        public ActionResult SearchConsultants(String _x)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login");
            }
            
                var collection = Context.Database.GetCollection<ConsultantsAccount>("Consultants");
            // Need to continue the query
                usersQuery = Query<ConsultantsAccount>.Where(s => s.UserName.Contains(_x) || s.YearOfExprience1.Contains(_x) || s.Street.Contains(_x) || s.ApartmentNumber.Contains(_x) || s.Birthday.Contains(_x) || s.City.Contains(_x));
                var usercursor = collection.Find(usersQuery);
                 var consultants = new List<ConsultantsAccount>();
               foreach (var item in usercursor)
            {
                consultants.Add(item);   
            }
            TempData["Accounts"] = consultants;
            return View();
            

        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(UserAccount user)
        {
            string userEmail;
            string emailBody = "<b>Your password is:</b><br/>";
            string emailSubject = "Password Recovery";
            var usersCollection = Context.Database.GetCollection<UserAccount>("Users");
            var consultantsCollection = Context.Database.GetCollection<ConsultantsAccount>("Consultants");
            usersQuery = Query<UserAccount>.Where(s => s.UserName == user.UserName);
            consulQuery = Query<ConsultantsAccount>.Where(s => s.UserName == user.UserName);
            var model1 = usersCollection.FindOne(usersQuery);
            var model2 = consultantsCollection.FindOne(consulQuery);

            if (model1 != null || model2 != null)
            {
                if (model1 != null)
                {
                    userEmail = model1.Email;
                    emailBody += model1.Password;
                    smtpRequest(userEmail, emailSubject, emailBody);

                    Session["UserName"] = model1.UserName.ToString();

                }

                if (model2 != null)
                {
                    userEmail = model2.Email;
                    emailBody += model2.Password;
                    smtpRequest(userEmail, emailSubject, emailBody);

                    Session["UserName"] = model2.UserName.ToString();

                }
            }

            else
            {
                TempData["Message"] = "משתמש לא נמצא";

            }
            return View();

        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }



        [HttpGet]
        public ActionResult SmartProfile()
        {
            if (Session["UserName"] != null)
            {
                var size = 0;
                var sizeLabel = 0;
                var sizePicture = 0;
                var consultantsCollection = Context.Database.GetCollection<ConsultantsAccount>("Consultants");
                var TextBoxCollection = Context.Database.GetCollection<TextBox>("TextBox");
                var LabelCollection = Context.Database.GetCollection<EditBox>("EditBox");
                var pictureCollection= Context.Database.GetCollection<Pictures>("Pictures");
                usersQuery = Query<ConsultantsAccount>.Where(s => s.UserName == (Session["UserName"]).ToString());
                var usersCursor = TextBoxCollection.Find(usersQuery);
                var usersCursorLabel = LabelCollection.Find(usersQuery);
                var usersCursorPictures = pictureCollection.Find(usersQuery);
                var Consultant = consultantsCollection.Find(usersQuery);
                var textBoxList = new List<TextBox>();
                var LabelList = new List<EditBox>();
                var pictureList = new List<Pictures>();

                foreach (var users in usersCursor)
                {
                    textBoxList.Add(users);
                    size++;
                }

                foreach (var item in usersCursorLabel)
                {
                    LabelList.Add(item);
                    sizeLabel++;
                }
                foreach (var item in usersCursorPictures)
                {
                    pictureList.Add(item);
                    sizePicture++;
                }

                ViewBag.Consultant = Consultant;
                ViewBag.textBoxList = (textBoxList);
                ViewBag.size = size;
                ViewBag.LabelList = (LabelList);
                ViewBag.sizeLabel = sizeLabel;
                ViewBag.pictureList = (pictureList);
                ViewBag.sizePicture = sizePicture;
                return View();
            }
            
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult SmartProfile(List<TextBox> _textbox, List<EditBox> _editbox,List<Pictures> _pictures, int editBoxCount, int textBoxCount,int picturesCount)
        {
            if (Session["UserName"] != null)
            {
                var consultantsCollection = Context.Database.GetCollection<ConsultantsAccount>("Consultants");
                usersQuery = Query<ConsultantsAccount>.Where(s => s.UserName == (Session["UserName"]).ToString());
                var picturesCollection = Context.Database.GetCollection<Pictures>("Pictures");
                var TextBoxCollection = Context.Database.GetCollection<TextBox>("TextBox");
                var LabelCollection = Context.Database.GetCollection<EditBox>("EditBox");
                var usersCursorLabel = LabelCollection.Remove(usersQuery);
                var usersCursorTextBox = TextBoxCollection.Remove(usersQuery);
                var usersCursorPictures = picturesCollection.Remove(usersQuery);
                var consultantsUsers = consultantsCollection.Find(usersQuery);
             
                for (int i = 0; i < textBoxCount; i++)
                {
                    _textbox[i].UserName = Session["UserName"].ToString();
                    Context.TextBox.Insert(_textbox[i]);

                }
                for (int i = 0; i < editBoxCount; i++)
                {
                    _editbox[i].UserName = Session["UserName"].ToString();
                    Context.EditBox.Insert(_editbox[i]);

                }
                for (int i = 0; i < picturesCount; i++)
                {
                    _pictures[i].UserName = Session["UserName"].ToString();
                    Context.Pictures.Insert(_pictures[i]);
                }
            
                return View(consultantsUsers);
            }
            else
            {

                return RedirectToAction("Login");

            }
        }

        private void smtpRequest(string userEmail, string emailSubject, string emailBody)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(userEmail);
            mail.From = new MailAddress("mapiconsultants@gmail.com");
            mail.Subject = emailSubject;
            mail.Body = emailBody;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("mapiconsultants@gmail.com", "1q2w3e4r@");
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(mail);    
                TempData["Message"] = "נשלח בהצלחה";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "שגיאה" + ex.Message;
            }
        }
    }
}