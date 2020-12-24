using System;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    public class AccountController : MainController
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            if (Email.ToLower() == "admin@admin.com" && Password=="admin")
            {
                var obj = new AccountVM();
                obj.IsAdmin = true;
                obj.ID = Guid.Empty;
                obj.IsVoted = false;
                obj.Email = Email;
                obj.Name = "Admin";
                Session["UserData"] = obj;
                return Json(new { IsAdmin = true, Code = 1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new AccountVM().LoginUser(Email, Password);
                if (result.Code == 1)
                {
                    result.Data.IsAdmin = false;
                    Session["UserData"] = result.Data;
                }
                return Json(new { IsAdmin = false, Code = result.Code }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(AccountVM obj)
        {
            obj.ID = Guid.NewGuid();
            Random random = new Random();
            obj.Password = random.Next(1, 250252025).ToString();
            var result = new AccountVM().AddUser(obj);
            if (result == 1)
            {
                try
                {
                    string body = "Your Password Is : " + obj.Password;
                    string HostMail = "smtp.gmail.com";
                    int HostMailPort = 587;
                    bool EnableSSL = true;
                    string SenderEmail = "senderemail";
                    string SenderPassword = "senderemailpass";
                    using (SmtpClient client = new SmtpClient())
                    {
                        client.Port = HostMailPort;
                        client.Host = HostMail;
                        client.EnableSsl = EnableSSL;
                        client.Timeout = 10000;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new System.Net.NetworkCredential(SenderEmail, SenderPassword);

                        MailMessage mail = new MailMessage(SenderEmail, obj.Email);
                        mail.Body = body;
                        mail.Subject = "Election Online Password";
                        mail.IsBodyHtml = false;
                        mail.BodyEncoding = UTF8Encoding.UTF8;
                        mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                        try
                        {
                            client.Send(mail);
                        }
                        catch (Exception)
                        {
                            DelUser(obj.ID);
                            result = 5;
                        }
                    }
                }
                catch (Exception)
                {
                    DelUser(obj.ID);
                    result = 5;
                }
                //SendMail
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        void DelUser(Guid ID)
        {
            bool result = new AccountVM().DelUser(ID);
        }
        public ActionResult Logout()
        {
            Session["UserData"] = null;
            return RedirectToAction("Index");
        }

    }
}