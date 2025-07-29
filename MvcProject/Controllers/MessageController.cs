using BusinessLayer.Concrete;
using BusinessLayer.ValidationsRules;
using DataAccessLayer.Abstract;
using DataAccessLayer.Entity_Framework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class MessageController : Controller
    {
        // Business katmanından mesaj yöneticisi
        MessageManager cm = new MessageManager(new EfMessageDal());
        MessageValidator messageValidator = new MessageValidator(); // FluentValidation sınıfı

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
        public ActionResult GetSendboxMessageDetails(int id)
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
        [ValidateInput(false)]
        public ActionResult NewMessage(Message p, string submitButton)
        {
            ValidationResult results = messageValidator.Validate(p);     // Doğrulama işlemi

            if (results.IsValid)
            {
                p.MessageDate = DateTime.Now;
                p.SenderMail = "admin@gmail.com";

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
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View(p);
        }



        // GET: Taslak mesaj düzenleme formu
        [HttpGet]
        public ActionResult EditDraft(int id)
        {
            var draftMessage = cm.GetByID(id);
            if (draftMessage == null)
                return HttpNotFound();

            return View(draftMessage);
        }

        // POST: Taslak mesaj güncelleme
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditDraft(Message p, string submitButton)
        {
            ValidationResult results = messageValidator.Validate(p);
            if (results.IsValid)
            {
                p.MessageDate = DateTime.Now;
                p.SenderMail = "admin@gmail.com";

                if (submitButton == "Taslakları Güncelle")
                {
                    p.IsDraft = true;
                    p.IsDeleted = false;
                    cm.MessageUpdate(p);
                    return RedirectToAction("Drafts");
                }
                else if (submitButton == "Gönder")
                {
                    p.IsDraft = false;
                    p.IsDeleted = false;
                    cm.MessageUpdate(p);
                    return RedirectToAction("Sendbox");
                }
            }
            else
            {
                foreach (var error in results.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            return View(p);
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
        // Spam olarak işaretleme
        [HttpPost]
        public ActionResult MarkAsNotSpam(int id)
        {
            cm.MarkAsNotSpam(id); // MessageManager'daki metot
            return RedirectToAction("SpamBox");
        }


        [HttpPost]
        public ActionResult MarkAsSpam(int id)
        {
            cm.MarkAsSpam(id);
            return RedirectToAction("SpamBox");
        }



        public ActionResult SpamBox()
        {
            var spamMessages = cm.GetSpamList();
            return View(spamMessages);
        }

    }
}
