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
using System.IO;
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


        [HttpPost(ApiRoutes.Photos.CouvertureEnfant)]
        public async Task<IActionResult> PostEnfantPhotoCouverture([FromRoute] int enfantId)
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



        [HttpPost("Gallerie/{voyageId}")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> PostEnfantPhotoGallerie([FromRoute] int enfantId)
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

            try
            {

                IFormCollection formCollection = await Request.ReadFormAsync();
                IFormFile file = formCollection.Files.GetFile("image");

                Image image = Image.Load(file.OpenReadStream());

                Photo photo = new Photo()
                {
                    Enfant = enfant,
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

    }
}
