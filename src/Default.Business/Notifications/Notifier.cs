using Default.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Default.Business.Notifications
{
    /// <summary>
    /// Class to manage errors notifications
    /// </summary>
    public class Notifier : INotifier
    {
        private List<string> _notifications;

        public Notifier()
        {
            _notifications = new List<string>();
        }

        /// <summary>
        /// Handle notification
        /// </summary>
        /// <param name="message"></param>
        public void Handle(string message)
        {
            _notifications.Add(message);
        }

        /// <summary>
        /// Get notifications
        /// </summary>
        /// <returns></returns>
        public List<string> GetNotifications()
        {
            return _notifications;
        }

        /// <summary>
        /// Check if has notifications
        /// </summary>
        /// <returns>if has notifications or not</returns>
        public bool HasNotifications()
        {
            return _notifications.Any();
        }
    }
}
