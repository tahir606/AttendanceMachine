using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tulpep.NotificationWindow;

namespace Attendance001.helper
{
    class NotificationHelper
    {

        static PopupNotifier popup = new PopupNotifier();

        NotificationHelper()
        {

        }

        public static void CreateNotification(string content)
        {            
            popup.Image = Properties.Resources.info;
            popup.TitleText = "Notification";
            popup.ContentText = content;
            popup.Popup();
        }

    }
}
