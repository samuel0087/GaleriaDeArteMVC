using System.Diagnostics;
using galeria_arte_MVC.Data;
using galeria_arte_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace galeria_arte_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GaleriaDbContext _context;

        public HomeController(ILogger<HomeController> logger, GaleriaDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var obras = await _context.Obras
                .Take(6)
                .Include(o => o.Artista)
                .ToListAsync();

            var exposiciones = await _context.Exposiciones.ToListAsync();

            var HomeView = new HomeViewModel { Exposiciones = exposiciones, Obras = obras };

            return View(HomeView);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
