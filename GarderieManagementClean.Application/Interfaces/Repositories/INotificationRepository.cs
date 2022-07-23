using Contracts.Dtos.Response;
using GarderieManagementClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {

        public Task<IEnumerable<NotificationsResponse>> getAllNotification(string userId);

        public Task createNotification(Notification notification);
        public Task deleteNotification(string userId, int notificationId);

        public Task deleteAllNotification(string userId);


        public Task markNotificationSeen(string userId, int notificationId);

        public Task markAllNotificationSeen(string userId);

    }
}
