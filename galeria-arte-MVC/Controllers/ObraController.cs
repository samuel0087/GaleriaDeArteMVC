using galeria_arte_MVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace galeria_arte_MVC.Controllers
{
    public class ObraController : Controller
    {
        private readonly GaleriaDbContext _context;

        public ObraController(GaleriaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(string Id)
        {
            var obra = await _context.Obras
                .Include(o => o.Artista)
                .FirstOrDefaultAsync(o => o.Id.ToString() == Id);
            return View(obra);
        }
    }
}
