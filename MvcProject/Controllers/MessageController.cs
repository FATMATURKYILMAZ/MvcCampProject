using BusinessLayer.Concrete;
using DataAccessLayer.Entity_Framework;
using EntityLayer.Concrete;
using System;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class MessageController : Controller
    {
        // Business katmanından mesaj yöneticisi
        MessageManager cm = new MessageManager(new EfMessageDal());

        // Gelen kutusu mesajları
        public ActionResult Inbox()
        {
            var messageList = cm.GetListInbox();
            return View(messageList);
        }

        // Gönderilen kutusu mesajları
        public ActionResult Sendbox()
        {
            var messageList = cm.GetListSendbox();
            return View(messageList);
        }
        public ActionResult GetInboxMessageDetails(int id)
        {
            var values = cm.GetByID(id);
            return View(values);
        }

        // Yeni mesaj oluşturma sayfası (GET)
        [HttpGet]
        public ActionResult NewMessage()
        {
            return View();
        }

        // Yeni mesaj gönderme işlemi (POST)
        // submitType parametresi ile "taslak" veya "gönder" işlemi belirlenir
        [HttpPost]
        [ValidateInput(false)]  // HTML içeriğini engellememek için
        public ActionResult NewMessage(Message p, string submitButton)
        {
            p.MessageDate = DateTime.Now;
            p.SenderMail = "admin@gmail.com";  // Sabit gönderici (isteğe bağlı dinamik yapılabilir)

            if (submitButton == "Taslaklara Kaydet")
            {
                p.IsDraft = true;
                p.IsDeleted = false;
                cm.MessageyAdd(p);
                return RedirectToAction("Drafts");
            }
            else if (submitButton == "Gönder")
            {
                p.IsDraft = false;
                p.IsDeleted = false;
                cm.MessageyAdd(p);
                return RedirectToAction("Sendbox");
            }
            else if (submitButton == "İptal Et")
            {
                p.IsDraft = false;
                p.IsDeleted = true;
                cm.MessageyAdd(p);
                return RedirectToAction("Trash");
            }

            return View(p); // Diğer durumlarda formu geri döndür
        }



        // Taslak mesaj düzenleme sayfası (GET)
        [HttpGet]
        public ActionResult EditDraft(int id)
        {
            var draftMessage = cm.GetByID(id);
            return View(draftMessage);
        }

        // Taslak mesajlar listesi
        public ActionResult Drafts()
        {
            var draftMessages = cm.GetDraftMessages();
            return View(draftMessages);
        }
        public ActionResult Trash()
        {
            var trashMessages = cm.GetTrashList();
            return View(trashMessages);
        }
        public ActionResult MoveToTrash(int id)
        {
            cm.MessageMoveToTrash(id);
            return RedirectToAction("Inbox");
        }
    }
}
