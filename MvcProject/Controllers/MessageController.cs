using BusinessLayer.Concrete;
using DataAccessLayer.Entity_Framework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class MessageController : Controller
    {
        // GET: Message
        MessageManager cm = new MessageManager(new EfMessageDal());

        public ActionResult Inbox()
        {
            var messageList = cm.GetListInbox();
            return View(messageList);
        }
        public ActionResult Sendbox()
        {
            var messageList=cm.GetListSendbox();
            return View(messageList);
        }
        [HttpGet]
        public ActionResult NewMessage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewMessage(Message p)
        {
            return View();
        }
        [HttpGet]
        public ActionResult New1Message()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New1Message(Message message, string submitButton)
        {
            message.MessageDate = DateTime.Now;
            message.SenderMail = "admin@gmail.com"; // örnek sabit sender

            if (submitButton == "Taslaklara Kaydet")
            {
                message.IsDraft = true;
                cm.MessageyAdd(message);
                return RedirectToAction("DraftList"); // Taslak listesine yönlendirme
            }
            else if (submitButton == "Gönder")
            {
                message.IsDraft = false;
                cm.MessageyAdd(message);
                return RedirectToAction("Sendbox"); // Gönderilen kutusuna yönlendirme
            }

            return View();
        }
    }
}