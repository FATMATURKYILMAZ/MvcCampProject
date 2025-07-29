using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class MessageManager : IMessageService
    {
        IMessageDal _messageDal;

        public MessageManager(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }

        public List<Message> GetDraftMessages()
        {
            return _messageDal.List(x => x.IsDraft == true);
        }

        public Message GetByID(int id)
        {
            return _messageDal.Get(x => x.MessageID == id);
        }

        public List<Message> GetListInbox()
        {
            return _messageDal.List(x => x.ReceiverMail == "admin@gmail.com" && x.IsSpam == false);
        }

        public List<Message> GetListSendbox()
        {
            return _messageDal.List(x => x.SenderMail == "admin@gmail.com");
        }

        public void MessageUpdate(Message message)
        {
            _messageDal.Update(message);
        }

        public void MessageyAdd(Message message)
        {
            // Mesaj spam mı kontrol et
            message.IsSpam = IsSpamMessage(message);
            _messageDal.Insert(message);
        }

        public void MessageDelete(Message message)
        {
            throw new NotImplementedException();
        }

        public List<Message> GetTrashList()
        {
            return _messageDal.List(x => x.IsDeleted == true && x.ReceiverMail == "admin@gmail.com");
        }

        public void MessageMoveToTrash(int id)
        {
            var message = _messageDal.Get(x => x.MessageID == id);
            if (message != null)
            {
                message.IsDeleted = true;
                _messageDal.Update(message);
            }
        }

        public bool IsSpamMessage(Message message)
        {
            var spamKeywords = new List<string> { "para kazan", "bedava", "tıkla", "reklam", "kampanya", "ürün" };
            string contentToCheck = (message.Subject + " " + message.MessageContent).ToLower();

            foreach (var keyword in spamKeywords)
            {
                if (contentToCheck.Contains(keyword))
                    return true;
            }
            return false;
        }

        public List<Message> GetSpamList()
        {
            return _messageDal.List(x => x.IsSpam == true && x.ReceiverMail == "admin@gmail.com");
        }

        public void MarkAsNotSpam(int id)
        {
            var message = _messageDal.Get(x => x.MessageID == id);
            if (message != null)
            {
                message.IsSpam = false;
                MessageUpdate(message);
            }
        }

        public void MarkAsSpam(int id)
        {
            var message = _messageDal.Get(x => x.MessageID == id);
            if (message != null)
            {
                message.IsSpam = true;
                MessageUpdate(message);
            }
        }


    }
}
