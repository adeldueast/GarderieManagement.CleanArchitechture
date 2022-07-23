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

        public async Task<IEnumerable<NotificationsResponse>> getAllNotification(string userId)
        {

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);

            var notifs = await _context.Notifications.Where(n => n.ApplicationUsers.Contains(user))
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationsResponse()
                {
                    CreatedAt = n.CreatedAt,
                    Id = n.Id,
                    Message = n.Message,
                    NotificationType = n.NotificationType,
                    DataId = n.DataId,
                    Seen = n.Seen
                }).ToListAsync();

            return notifs;
        }
        public async Task createNotification(Notification notification)
        {
            _context.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task deleteAllNotification(string userId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);

            var notifs = await _context.Notifications.Where(n => n.ApplicationUsers.Contains(user)).ToListAsync();

            _context.RemoveRange(notifs);
            await _context.SaveChangesAsync();

        }

        public async Task deleteNotification(string userId, int notificationId)
        {

            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            var notif = await _context.Notifications.SingleOrDefaultAsync(n => n.Id == notificationId && n.ApplicationUsers.Contains(user));
            if (notif != null)
            {
                _context.Remove(notif);
                await _context.SaveChangesAsync();
            }
        }

        public async Task markAllNotificationSeen(string userId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);

            var notifs = await _context.Notifications.Where(n => n.ApplicationUsers.Contains(user)).ToListAsync();
            foreach (var notif in notifs)
            {
                notif.Seen = true;

            }
            await _context.SaveChangesAsync();


        }

        public async Task markNotificationSeen(string userId, int notificationId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);

            var notif = await _context.Notifications.SingleOrDefaultAsync(n => n.Id == notificationId && n.ApplicationUsers.Contains(user));

            notif.Seen = true;
            await _context.SaveChangesAsync();
        }
    }
}
