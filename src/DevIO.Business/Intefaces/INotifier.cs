using System.Collections.Generic;
using DevIO.Business.Notifications;

namespace DevIO.Business.Intefaces
{
    public interface INotifier
    {
        void Handle(Notification notification);

        List<Notification> GetNotifications();
        
        bool HasNotifications();
    }
}