using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sanyug_bugtracker.Hubs
{
    public class NotificationHub: Hub
    {
        public void NotifyAll(string title, string message, string alertType)
        {
            Clients.All.displayNotification(title, message, alertType);
        }
    }
}