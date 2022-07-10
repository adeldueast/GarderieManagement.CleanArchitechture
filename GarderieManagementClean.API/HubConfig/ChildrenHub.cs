using GarderieManagementClean.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public override Task OnConnectedAsync()
        {
            string garderie_group = Context.User.Claims.Single(x => x.Type == "GarderieId").Value;

            Groups.AddToGroupAsync(Context.ConnectionId, garderie_group);

            return base.OnConnectedAsync();

        }

        public override Task OnDisconnectedAsync(Exception stopCalled)
        {
          


            return base.OnDisconnectedAsync(stopCalled);
        }


    }
}
