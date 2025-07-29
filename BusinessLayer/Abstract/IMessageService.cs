using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IMessageService
    {
        List<Message> GetListInbox();
        List<Message> GetListSendbox();
        void MessageyAdd(Message message);
        Message GetByID(int id);
        void MessageDelete(Message message);
        void MessageUpdate(Message message);
        List<Message> GetTrashList();
        void MessageMoveToTrash(int id);
        // Yeni spam metodları
        List<Message> GetSpamList();           // Spam mesajları listele
        void MarkAsNotSpam(int id);             // Spam değil olarak işaretle
        bool IsSpamMessage(Message message);    // Mesajın spam olup olmadığını kontrol et
    }
}
