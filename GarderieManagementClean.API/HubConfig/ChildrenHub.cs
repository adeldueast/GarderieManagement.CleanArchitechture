using GarderieManagementClean.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GarderieManagementClean.API.HubConfig
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChildrenHub : Hub
    {
        private readonly ApplicationDbContext _context;
        public ChildrenHub(ApplicationDbContext context)
        {
            _context = context;
        }
        public override async Task<Task> OnConnectedAsync()
        {
            string userId = Context.User.Claims.Single(x => x.Type == "Id").Value;
            string email = Context.User.Claims.Single(x => x.Type == ClaimTypes.Email).Value;
            string garderieId = Context.User.Claims.Single(x => x.Type == "GarderieId").Value;
            var user = await _context.Users.SingleAsync(x => x.Id == userId);
            user.isOnline = true;
            await _context.SaveChangesAsync();

            await Groups.AddToGroupAsync(Context.ConnectionId, garderieId);
            Debug.WriteLine($"User {email} joined group {garderieId}");

            await Clients.Group(garderieId).SendAsync("notifyUserStatusChanges", $"User {email} is online in garderie {garderieId}");
            return base.OnConnectedAsync();

        }

        public override async Task<Task> OnDisconnectedAsync(Exception stopCalled)
        {

            string userId = Context.User.Claims.Single(x => x.Type == "Id").Value;
            string email = Context.User.Claims.Single(x => x.Type == ClaimTypes.Email).Value;
            string garderieId = Context.User.Claims.Single(x => x.Type == "GarderieId").Value;
            var user = await _context.Users.SingleAsync(x => x.Id == userId);
            user.isOnline = false;
            await _context.SaveChangesAsync();

            Debug.WriteLine($"User {email} disconnected in garderie {garderieId}");
            await Clients.Group(garderieId).SendAsync("notifyUserStatusChanges", $"User {email} disconnected");

            return base.OnDisconnectedAsync(stopCalled);
        }



    }
}
