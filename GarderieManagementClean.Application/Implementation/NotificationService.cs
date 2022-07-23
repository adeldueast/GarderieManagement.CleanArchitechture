using Contracts.Dtos.Response;
using GarderieManagementClean.Application.Interfaces.Repositories;
using GarderieManagementClean.Application.Interfaces.Services;
using GarderieManagementClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Implementation
{
    public class NotificationService : INotificationService
    {

        private readonly INotificationRepository _notificationService;

        // public IEnfantRepository _enfantRepository { get; }
        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationService = notificationRepository;
        }


        public Task createNotification(Notification notification)
        {
            return _notificationService.createNotification(notification);
        }

        public async Task deleteAllNotification(string userId)
        {
            await _notificationService.deleteAllNotification(userId);
        }

        public async Task deleteNotification(string userId, int notificationId)
        {
             await _notificationService.deleteNotification(userId, notificationId);
        }

        public async Task<IEnumerable<NotificationsResponse>> getAllNotification(string userId)
        {
            return await _notificationService.getAllNotification(userId);
        }

        public async Task markAllNotificationSeen(string userId)
        {
            await _notificationService.markAllNotificationSeen(userId);
        }

        public async Task markNotificationSeen(string userId, int notificationId)
        {
            await _notificationService.markNotificationSeen(userId, notificationId);
        }
    }
}
