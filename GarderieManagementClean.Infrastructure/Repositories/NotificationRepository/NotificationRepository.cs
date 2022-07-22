using Contracts.Dtos;
using Contracts.Dtos.Response;
using GarderieManagementClean.Application.Interfaces.Repositories;
using GarderieManagementClean.Domain.Entities;
using GarderieManagementClean.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Infrastructure.Repositories.NotificationRepository
{
    public class NotificationRepository : INotificationRepository
    {

        private readonly ApplicationDbContext _context;
        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task createNotification(Notification notification)
        {
            _context.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task deleteNotification(int notificationId)
        {

            var notif = _context.Notifications.SingleOrDefault(n => n.Id == notificationId);

            if (notif != null)
            {
                _context.Remove(notif);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<NotificationsResponse>> getAllNotification(string userId)
        {

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);

            var notifs = await _context.Notifications.Where(n => n.ApplicationUsers.Contains(user)).OrderBy(n => n.Seen == false).Select(n => new NotificationsResponse()
            {
                CreatedAt = n.CreatedAt,
                Id = n.Id,
                Message = n.Message,
                NotificationType = n.NotificationType,
                Seen = n.Seen
            }).ToListAsync();

            return notifs;
        }
    }
}
