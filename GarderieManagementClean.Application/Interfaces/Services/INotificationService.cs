using Contracts.Dtos.Response;
using GarderieManagementClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Interfaces.Services
{
    public interface INotificationService
    {
        public Task<IEnumerable<NotificationsResponse>> getAllNotification(string userId);


        public Task createNotification(Notification  notification);

        public Task deleteNotification(int notificationId);


    }
}
