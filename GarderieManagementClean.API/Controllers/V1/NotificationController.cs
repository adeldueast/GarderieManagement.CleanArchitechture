using GarderieManagementClean.API.Extensions;
using GarderieManagementClean.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GarderieManagementClean.API.Controllers.V1
{
    [Authorize]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        // GET: api/<NotificationController>

        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }


      
        [HttpGet(ApiRoutes.Notification.Get)]

        public async Task<IActionResult> Get()
        {
            var userId = HttpContext.GetUserId();

            var notifications = await _notificationService.getAllNotification(userId);

            return Ok(notifications);
        }


        //// DELETE api/<NotificationController>/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int notificationId)
        //{
        //    await _notificationService.deleteNotification(notificationId);
        //    return Ok();
        //}


        //// GET api/<NotificationController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<NotificationController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<NotificationController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}
    }
}
