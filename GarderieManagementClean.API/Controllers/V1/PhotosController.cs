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
        private readonly RoleManager<IdentityRole> _roleManager;




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

                return Ok();

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }



        [Authorize(Roles = "owner,admin,employee")]
        [HttpPost(ApiRoutes.Photos.PostGallerieEnfant)]
        public async Task<IActionResult> PostGallerieEnfant([FromQuery] int[] enfantIds)
        {

            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());

            //Check if ids are all valid ids
            ICollection<Enfant> enfants = new List<Enfant>();
            foreach (var enfantId in enfantIds)
            {
                var enfant = await _context.Enfants
                    .SingleOrDefaultAsync(e => e.Id == enfantId && e.GarderieId == user.GarderieId);

                if (enfant == null)
                {
                    return NotFound($"Enfant '{enfantId}' does not exist");
                }

                enfants.Add(enfant);

            }




            try
            {

                IFormCollection formCollection = await Request.ReadFormAsync();
                IFormFile file = formCollection.Files.GetFile("image");

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


                var photo = await _context.Photos.SingleOrDefaultAsync(p => p.Id == id);

                if (photo == null)
                {
                    return NotFound($"Photo {id} does not exist");
                }

                //Get the user's roles
                var userRoles = await _userManager.GetRolesAsync(user);

                // is a photo couverture of enfant
                if (photo.PhotoCouvertureDe != null)
                {
                    if (userRoles.Contains("tutor"))
                    {
                        //is a tutor, add security
                        //security checks  (same garderie && user and request photo's are linked)
                        var isAuthorized = photo.PhotoCouvertureDe.GarderieId == user.GarderieId && photo.PhotoCouvertureDe.Tutors.Select(te => te.ApplicationUser).ToList().Contains(user);
                        if (!isAuthorized)
                        {
                            return NotFound($"Photo {id} does not exist");
                        }
                        byte[] bytes;
                        bytes = System.IO.File.ReadAllBytes($"C://images/{size}/{photo.FileName}");
                        return File(bytes, photo.MimeType);
                    }
                    else
                    {
                        //is an educator, add security
                        //security checks  (same garderie)
                        if (!(photo.PhotoCouvertureDe.GarderieId == user.GarderieId))
                        {
                            return NotFound($"Photo {id} does not exist");
                        }
                        byte[] bytes;
                        bytes = System.IO.File.ReadAllBytes($"C://images/{size}/{photo.FileName}");
                        return File(bytes, photo.MimeType);
                    }


                }

                // is a photo couverture of user
                // is a photo of gallerie of enfant 
                if (photo.Enfants != null && photo.Enfants.Count > 0)
                {
                    if (userRoles.Contains("tutor"))
                    {
                        //is a tutor, add security
                        //security checks  (same garderie && user and request photo's are linked)

                        var isAuthorized = photo.Enfants.All(e => e.Id == user.GarderieId && e.Tutors.Select(te => te.ApplicationUser).Contains(user));
                        if (!isAuthorized)
                        {
                            return NotFound($"Photo {id} does not exist");
                        }


                        byte[] bytes;
                        bytes = System.IO.File.ReadAllBytes($"C://images/{size}/{photo.FileName}");
                        return File(bytes, photo.MimeType);
                    }
                    else
                    {
                        //is an educator, add security
                        //security checks  (same garderie)
                        if (!(photo.PhotoCouvertureDe.GarderieId == user.GarderieId))
                        {
                            return NotFound($"Photo {id} does not exist");
                        }
                        byte[] bytes;
                        bytes = System.IO.File.ReadAllBytes($"C://images/{size}/{photo.FileName}");
                        return File(bytes, photo.MimeType);
                    }
                }



                return NotFound();


            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }




        }






    }
}
