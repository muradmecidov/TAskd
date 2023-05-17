using Microsoft.AspNetCore.Mvc;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using WebFrontToBack.Areas.Admin.ViewModels;
using System.Diagnostics.Metrics;
using WebFrontToBack.Utilities.Extensions;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class RecentWorkController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RecentWorkController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<RecentWork> recentWorks = await _context.RecentWorks.ToListAsync();
            return View(recentWorks);
        }

       

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecentWorkVM recentWork)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!recentWork.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", $"{recentWork.Photo.FileName} must be image type");
                return View();
            }
            if (!recentWork.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", $"{recentWork.Photo.FileName} file must be size less than 200kb ");
                return View();

            }
            string root = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img");
            string fileName = await recentWork.Photo.SaveAsync(root);


           

            RecentWork recentwork = new RecentWork()
            {
                Title=recentWork.Title,
                Description=recentWork.Description,
                ImagePath=fileName
            };



            await _context.RecentWorks.AddAsync(recentwork);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        public async Task<IActionResult> Update(int id)
        {
            RecentWork recentWork = await _context.RecentWorks.FindAsync(id);
            if (recentWork == null)
            {
                return NotFound();
            }

            UpdateRecentWorkVM model = new UpdateRecentWorkVM()
            {
                Title = recentWork.Title,
                Description = recentWork.Description,
               
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateRecentWorkVM model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            RecentWork recentWork = await _context.RecentWorks.FindAsync(id);
            if (recentWork == null)
            {
                return NotFound();
            }

            if (model.Photo != null)
            {
                if (!model.Photo.CheckContentType("image/"))
                {
                    ModelState.AddModelError("Photo", $"{model.Photo.FileName} must be an image type");
                    return View(model);
                }
                if (!model.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", $"{model.Photo.FileName} file must be smaller than 200kb");
                    return View(model);
                }

                string root = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img");
                string existingImagePath = Path.Combine(root, recentWork.ImagePath);
                if (System.IO.File.Exists(existingImagePath))
                {
                    System.IO.File.Delete(existingImagePath);
                }

                string fileName = await model.Photo.SaveAsync(root);
                recentWork.ImagePath = fileName;
            }

            recentWork.Title = model.Title;
            recentWork.Description = model.Description;

            _context.RecentWorks.Update(recentWork);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




        public IActionResult Delete(int Id)
        {
            RecentWork? recentWork = _context.RecentWorks.Find(Id);
            if (recentWork == null)
            {
                return NotFound();
            }
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", recentWork.ImagePath);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.RecentWorks.Remove(recentWork);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
