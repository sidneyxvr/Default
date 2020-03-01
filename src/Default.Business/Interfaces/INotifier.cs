using System.Collections.Generic;

namespace Default.Business.Interfaces
{
    public interface INotifier
    {
        bool HasNotifications();
        List<string> GetNotifications();
        void Handle(string message);
    }
}
