using GarderieManagementClean.API.Extensions;
using GarderieManagementClean.API.HubConfig;
using GarderieManagementClean.Application.Interfaces.Services;
using GarderieManagementClean.Domain.Entities;
using GarderieManagementClean.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GarderieManagementClean.API.Controllers.V1
{
    //[Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PhotosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly INotificationService _notificationService;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IHubContext<ChildrenHub> _hubContext;



        public PhotosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHubContext<ChildrenHub> hubContext, INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
            _notificationService = notificationService;






        }


        [Authorize(Roles = "owner,admin,employee")]
        [HttpPost(ApiRoutes.Photos.PostCouvertureEnfant)]
        public async Task<IActionResult> PostCouvertureEnfant([FromRoute] int enfantId)
        {
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());



            var enfant = await _context.Enfants
              .Include(e => e.PhotoCouverture)
              .SingleOrDefaultAsync(e =>
              e.Id == enfantId &&
              e.GarderieId == user.GarderieId);


            if (enfant == null)
            {
                return NotFound($"Enfant '{enfantId}' does not exist");
            }

            if (enfant.PhotoCouverture != null)
            {
                enfant.PhotoCouverture = null;
            }

            try
            {
                IFormCollection formCollection = await Request.ReadFormAsync();
                IFormFile file = formCollection.Files.GetFile("files");
                Image image = Image.Load(file.OpenReadStream());

                Photo photo = new Photo()
                {
                    PhotoCouvertureDe = enfant,
                    FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName),
                    MimeType = file.ContentType
                };


                SaveImages(image, photo);


                _context.Photos.Add(photo);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.Group(HttpContext.GetUserGarderieId()).SendAsync("childUpdate", $"gallerie was updated");
                return Ok(photo.Id);

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }


        //Todo Notify parents when new photo is added
        [Authorize(Roles = "owner,admin,employee")]
        [HttpPost(ApiRoutes.Photos.PostGallerieEnfant)]
        public async Task<IActionResult> PostGallerieEnfant([FromForm] PostGalleriePhotosRequest input)
        {

            //Get logged in user
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());



            try
            {


                //Get the files (photos)
                var files = input.Files;

                //Get the childrenIds that will be assigned to the photo
                var enfantsIds = input.EnfantsIds;





                //Check if childrenIds are all valid ids
                ICollection<Enfant> enfants = new List<Enfant>();
                foreach (var enfantId in enfantsIds)
                {
                    var enfant =
                        await _context.Enfants
                        .Include(e => e.Tutors)
                        .ThenInclude(te => te.ApplicationUser)
                        .SingleOrDefaultAsync(e => e.Id == enfantId && e.GarderieId == user.GarderieId);

                    if (enfant == null)
                    {
                        return NotFound($"Enfant '{enfantId}' does not exist");
                    }
                    //add tracked child to childList because we will need them after
                    enfants.Add(enfant);
                }


                //Foreach file(image), create a Photo Entity, save it on the machine, add photo in collection so we can save all changes in one (saveChangesAsync()) after the loop
                ICollection<Photo> photos = new List<Photo>();
                foreach (var file in files)
                {
                    Image image = Image.Load(file.OpenReadStream());
                    Photo photo = new Photo()
                    {
                        Description = input.Description,
                        Enfants = new List<Enfant>(enfants),
                        FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName),
                        MimeType = file.ContentType
                    };

                    SaveImages(image, photo);
                    photos.Add(photo);
                    _context.Photos.Add(photo);

                }
                await _context.SaveChangesAsync();


                HashSet<string> tutorsToNotify = new HashSet<string>();
                foreach (var photo in photos)
                {
                    foreach (var enfant in enfants)
                    {
                        //keep track of all parents that will be notified for new Photo of their child
                        enfant.Tutors.Select(te => te.ApplicationUser.Id).ToList().ForEach(tutorId => tutorsToNotify.Add(tutorId));

                        Notification notification = new Notification
                        {
                            CreatedAt = DateTime.Now,
                            ApplicationUsers = new List<ApplicationUser>(enfant.Tutors.Select(te => te.ApplicationUser)),
                            NotificationType = NotificationTypes.Photo,
                            DataId = photo.Id,
                            Message = $"New photo available for {enfant.Nom.Split(' ')[0]}",

                        };

                        _context.Notifications.Add(notification);

                    }
                }

                await _context.SaveChangesAsync();
                //notify all parents of a new image for their kids using signalR
                await _hubContext.Clients.Users(tutorsToNotify).SendAsync("newNotification", $"new notification avaible");
                await _hubContext.Clients.Group(HttpContext.GetUserGarderieId()).SendAsync("childUpdate", $"gallerie was updated");


                return Ok();


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }

        private static void SaveImages(Image image, Photo photo)
        {
            var systemPath = System.AppContext.BaseDirectory;
            System.IO.Directory.CreateDirectory($"{systemPath}lg");
            System.IO.Directory.CreateDirectory($"{systemPath}md");
            System.IO.Directory.CreateDirectory($"{systemPath}sm");

            var filePath = $"{systemPath}lg//" + photo.FileName;
            image.Save(filePath);

            //  image.Save($"C://images//lg//" + photo.FileName);


            image.Mutate(i =>
            i.Resize(new ResizeOptions()
            {
                Mode = ResizeMode.Min,
                Size = new Size() { Height = 720 }
            }));
            filePath = $"{systemPath}md//" + photo.FileName;
            image.Save(filePath);
            //  image.Save($"C://images//md//" + photo.FileName);


            image.Mutate(i =>
            i.Resize(new ResizeOptions()
            {
                Mode = ResizeMode.Min,
                Size = new Size() { Height = 320 }
            }));
            filePath = $"{systemPath}sm//" + photo.FileName;
            image.Save(filePath);
            // image.Save($"C://images//sm//" + photo.FileName);
        }

        [Authorize(Roles = "owner,admin,employee,tutor")]
        [HttpGet(ApiRoutes.Photos.Get)]
        public async Task<IActionResult> GetPhoto([FromRoute] string size, [FromRoute] int id)
        {
            try
            {

                //Get the user requesting the photo
                var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());

                var photo = await _context.Photos
                    .Include(p => p.PhotoCouvertureDe)
                    .ThenInclude(e => e.Tutors)
                    .Include(p => p.Enfants)
                    .ThenInclude(e => e.Tutors)
                    .ThenInclude(te => te.ApplicationUser)
                    .SingleOrDefaultAsync(p => p.Id == id);

                //Check if exist..
                if (photo == null)
                {
                    return NotFound($"Photo {id} does not exist");
                }

                //Return the file depending on what type of photo it is (special relation between photo and Child) and the type of user requesting(role) 
                return await returnFileIfAuthorized(photo, user, size, id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        private async Task<IActionResult> returnFileIfAuthorized(Photo photo, ApplicationUser user, string size, int id)
        {
            //Get the user's roles
            var userRoles = await _userManager.GetRolesAsync(user);

            //depending on what type of photo it is (Profile photo, or photo of gallerie of a child) and depending on the user requesting (educator or child's guardian(tutor))
            //If PhotoCouvertureDe != null, photo is of type profile picture (one to one relation with child)
            var isAuthorizedd = photo.PhotoCouvertureDe != null
                ? userRoles.Contains("tutor") == true
                    ? photo.PhotoCouvertureDe.GarderieId == user.GarderieId && photo.PhotoCouvertureDe.Tutors.Select(te => te.ApplicationUser).Contains(user)
                    : photo.PhotoCouvertureDe.GarderieId == user.GarderieId
                //Else, it is a photo of type gallerie photo (many-to-many) relation with childs, security check slightly different 
                : userRoles.Contains("tutor") == true
                    ? photo.Enfants.Any(e => e.GarderieId == user.GarderieId && e.Tutors.Select(te => te.ApplicationUser).Contains(user))
                    : photo.Enfants.All(e => e.GarderieId == user.GarderieId);

            if (!isAuthorizedd)
            {
                return NotFound($"Photo {id} does not exist");
            }

            var systemPath = System.AppContext.BaseDirectory;
            var filePath = $"{systemPath}sm//" + photo.FileName;
            var file = $"C:/images/{size}/{photo.FileName}";
            var bytes = System.IO.File.ReadAllBytes(filePath);

            return File(bytes, photo.MimeType);
            //return new FileStreamResult(new FileStream(file, FileMode.Open), photo.MimeType);
        }



        [HttpGet(ApiRoutes.Photos.GetAllGalleriePhotos)]
        [Authorize(Roles = "owner,admin,employee")]
        public async Task<IActionResult> getPhotoIdsOfAllEnfants()
        {
            //[FromBody] int[] enfantIds
            //Get logged in user
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());

            //returns all photo's ids except for the ones tht are profile photos
            var images = await _context.Photos
                .Where(photo => photo.Enfants.Count > 0 && photo.Enfants.All(enfant => enfant.GarderieId == user.GarderieId))
                .Select(p => new { p.Id, p.Description })
                .ToListAsync();

            return Ok(images);
        }


        [HttpGet(ApiRoutes.Photos.GetAllGaleriePhotoOfEnfantsOfTutor)]
        [Authorize(Roles = "tutor")]
        public async Task<IActionResult> getPhotoIdsOfAllEnfantsOfTutor()
        {
            //[FromBody] int[] enfantIds
            //Get logged in user
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());

            //returns all photo's ids except for the ones tht are profile photos
            var images = await _context.Photos
                .Where(photo => photo.Enfants.Count > 0 && photo.Enfants.Any(enfant => enfant.GarderieId == user.GarderieId && enfant.Tutors.Select(te => te.ApplicationUser).Contains(user)))
                .Select(p => new { p.Id, p.Description })
                .ToListAsync();

            return Ok(images);
        }

        [HttpGet(ApiRoutes.Photos.GetGalleriePhotosOfEnfant)]
        [Authorize(Roles = "owner,admin,employee,tutor")]
        public async Task<IActionResult> getPhotoIdsOfEnfant([FromRoute] int enfantId)
        {
            //[FromBody] int[] enfantIds
            //Get logged in user
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());

            //returns all photo's ids except for the ones tht are profile photos
            if (await _userManager.IsInRoleAsync(user, "tutor"))
            {
                var imagess = await _context.Photos
             .Where(photo => photo.Enfants.Count > 0 && photo.Enfants.Any(enfant => enfant.Id == enfantId && enfant.GarderieId == user.GarderieId && enfant.Tutors.Select(te => te.ApplicationUser).Any(t => t.Id == user.Id)))
             .Select(p => new { p.Id, p.Description }).ToListAsync();

                return Ok(imagess);
            }
            var images = await _context.Photos
                .Where(photo => photo.Enfants.Count > 0 && photo.Enfants.Any(enfant => enfant.Id == enfantId && enfant.GarderieId == user.GarderieId))
                .Select(p => new { p.Id, p.Description }).ToListAsync();

            return Ok(images);
        }



        [HttpGet(ApiRoutes.Photos.GetPhotoInformation)]
        [Authorize(Roles = "owner,admin,employee,tutor")]
        public async Task<IActionResult> getPhotoInformation([FromRoute] int photoId)
        {


            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());

            if (await _userManager.IsInRoleAsync(user, "tutor"))
            {
                var image = await _context.Photos
                    .Where(p => p.Id == photoId && p.Enfants.SelectMany(e => e.Tutors.Select(te => te.ApplicationUser)).Contains(user))
                    .Select(p => new { p.Id, p.Description })
                    .SingleOrDefaultAsync();

                if (image is null)
                {
                    return NotFound();
                }

                return Ok(image);


            }

            return NotFound();

        }


        //TOTO: Add delete Action
        public class PostGalleriePhotosRequest
        {


            public IReadOnlyList<IFormFile> Files { get; set; }

            public string Description { get; set; }
            public string Enfants { get; set; }
            public IEnumerable<int> EnfantsIds
            {
                get
                {
                    return this.Enfants.Split(',').Select(Int32.Parse).ToList();

                }
                set
                {

                }
            }




        }
    }
}
