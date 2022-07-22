using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        public bool Seen { get; set; } = false;

        public virtual List<ApplicationUser>  ApplicationUsers { get; set; }

        public DateTime CreatedAt { get; set; }

        public NotificationTypes NotificationType { get; set; }

        //When user clicks on notification, we can redirect him to journal or photo depending on the type of notif
        //public int DataId { get; set; }

        public string Message { get; set; }


    }

    public enum NotificationTypes
    {
        Photo,
        Journal
    }
}
