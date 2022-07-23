using GarderieManagementClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos.Response
{
    public class NotificationsResponse
    {

        public int Id { get; set; }

        public bool Seen { get; set; } = false;


        public DateTime CreatedAt { get; set; }

        public NotificationTypes NotificationType { get; set; }

        //When user clicks on notification, we can redirect him to journal or photo depending on the type of notif
        public int DataId { get; set; }
 



        public string Message { get; set; }

    }
}
