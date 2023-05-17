using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;
using WebFrontToBack.ViewModel;

namespace WebFrontToBack.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM() { 
            Sliders= await _appDbContext.Sliders.ToListAsync(),
            Categories= await _appDbContext.Categories.Where(c=>!c.IsDeleted).ToListAsync(),
            Services= await _appDbContext.Services
                .Include(s => s.Category)
                .Include(s => s.ServiceImages)
                .OrderByDescending(s => s.Id)
                .Where(s=>!s.IsDeleted)
                .Take(8)
                .ToListAsync(),
            RecentWorks = await _appDbContext.RecentWorks.ToListAsync()
            };
            return View(homeVM);
        }
        //public async Task<IActionResult> Index()
        //{
        //    List<RecentWork> recentWorks = await _appDbContext.RecentWorks.ToListAsync();

        //    return View(recentWorks);
        //}
    }
}
