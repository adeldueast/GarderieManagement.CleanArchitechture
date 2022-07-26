using GarderieManagementClean.API.Extensions;
using GarderieManagementClean.Domain.Entities;
using GarderieManagementClean.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        private readonly UserManager<ApplicationUser> _userManager;





        public PhotosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "owner,admin,employee")]
        [HttpPost(ApiRoutes.Photos.PostCouvertureEnfant)]
        public async Task<IActionResult> PostCouvertureEnfant([FromRoute] int enfantId)
        {
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());

            var enfant = await _context.Enfants
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
                IFormFile file = formCollection.Files.GetFile("image");
                Image image = Image.Load(file.OpenReadStream());

                Photo photo = new Photo()
                {
                    PhotoCouvertureDe = enfant,
                    FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName),
                    MimeType = file.ContentType
                };

                var lg = Directory.CreateDirectory($"C://images//lg//");
                var md = Directory.CreateDirectory($"C://images//md//");
                var sm = Directory.CreateDirectory($"C://images//sm//");

                image.Save($"C://images//lg//" + photo.FileName);

                image.Mutate(i =>
                i.Resize(new ResizeOptions()
                {
                    Mode = ResizeMode.Min,
                    Size = new Size() { Height = 720 }
                }));
                image.Save($"C://images//md//" + photo.FileName);


                image.Mutate(i =>
                i.Resize(new ResizeOptions()
                {
                    Mode = ResizeMode.Min,
                    Size = new Size() { Height = 320 }
                }));
                image.Save($"C://images//sm//" + photo.FileName);



                await _context.Photos.AddAsync(photo);
                await _context.SaveChangesAsync();

                return Ok(photo.Id);

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }



        [Authorize(Roles = "owner,admin,employee")]
        [HttpPost(ApiRoutes.Photos.PostGallerieEnfant)]
        public async Task<IActionResult> PostGallerieEnfant()
        {

            //Get logged in user
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());






            try
            {

                IFormCollection formCollection = await Request.ReadFormAsync();
                IReadOnlyList<IFormFile> files = formCollection.Files.GetFiles("image");


                var enfantsIds = formCollection["enfantIds"];

                //Check if ids are all valid ids
                ICollection<Enfant> enfants = new List<Enfant>();
                foreach (var enfantId in enfantsIds.ToString().Split(',').AsEnumerable())
                {
                    var enfant =
                        await _context.Enfants
                        .SingleOrDefaultAsync(e => e.Id == int.Parse(enfantId) && e.GarderieId == user.GarderieId);

                    if (enfant == null)
                    {
                        return NotFound($"Enfant '{enfantId}' does not exist");
                    }

                    enfants.Add(enfant);

                }

                foreach (var file in files)
                {
                    Image image = Image.Load(file.OpenReadStream());

                    Photo photo = new Photo()
                    {
                        Enfants = new List<Enfant>(enfants),
                        FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName),
                        MimeType = file.ContentType
                    };

                    var lg = Directory.CreateDirectory($"C://images//lg//");
                    var md = Directory.CreateDirectory($"C://images//md//");
                    var sm = Directory.CreateDirectory($"C://images//sm//");

                    image.Save($"{lg.FullName}" + photo.FileName);

                    image.Mutate(i =>
                    i.Resize(new ResizeOptions()
                    {
                        Mode = ResizeMode.Min,
                        Size = new Size() { Height = 720 }
                    }));
                    image.Save($"{md.FullName} " + photo.FileName);


                    image.Mutate(i =>
                    i.Resize(new ResizeOptions()
                    {
                        Mode = ResizeMode.Min,
                        Size = new Size() { Height = 320 }
                    }));
                    image.Save($"{sm.FullName}" + photo.FileName);

                    await _context.Photos.AddAsync(photo);


                }
                await _context.SaveChangesAsync();


                return Ok();


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


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

                //Return the file depending on what type of photo it is (special relation between photo and Child)
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
            var isAuthorizedd = photo.PhotoCouvertureDe != null
                ? userRoles.Contains("tutor") == true
                    ? photo.PhotoCouvertureDe.GarderieId == user.GarderieId && photo.PhotoCouvertureDe.Tutors.Select(te => te.ApplicationUser).Contains(user)
                    : photo.PhotoCouvertureDe.GarderieId == user.GarderieId
                : userRoles.Contains("tutor") == true
                    ? photo.Enfants.Any(e => e.GarderieId == user.GarderieId && e.Tutors.Select(te => te.ApplicationUser).Contains(user))
                    : photo.Enfants.All(e => e.GarderieId == user.GarderieId);

            if (!isAuthorizedd)
            {
                return NotFound($"Photo {id} does not exist");
            }

            var file = $"C:/images/{size}/{photo.FileName}";
            return new FileStreamResult(new FileStream(file, FileMode.Open), photo.MimeType);
        }



        [HttpGet(ApiRoutes.Photos.GetPhotosGallerieIdsOfAllEnfants)]
        [Authorize(Roles = "owner,admin,employee")]
        public async Task<IActionResult> getPhotoIdsOfAllEnfants()
        {
            //[FromBody] int[] enfantIds
            //Get logged in user
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());

            //returns all photo's ids except for the ones tht are profile photos
            var images = await _context.Photos
                .Where(photo => photo.Enfants.Count > 0 && photo.Enfants.All(enfant => enfant.GarderieId == user.GarderieId))
                .Select(p => new { p.Id, p.Description }).ToListAsync();

            return Ok(images);
        }




    }
}
