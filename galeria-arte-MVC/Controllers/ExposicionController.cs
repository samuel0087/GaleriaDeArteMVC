
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using galeria_arte_MVC.Models;
using galeria_arte_MVC.Data;

public class ExposicionController : Controller
{
    private readonly GaleriaDbContext _context;

    public ExposicionController(GaleriaDbContext context)
    {
        _context = context;
    }

    // GET: EXPOSICIONS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Exposiciones.ToListAsync());
    }

    // GET: EXPOSICIONS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var exposicion = await _context.Exposiciones
            .Include(e => e.ObrasExpuestas)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (exposicion == null)
        {
            return NotFound();
        }

        return View(exposicion);
    }

    // GET: EXPOSICIONS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: EXPOSICIONS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,FechaInicio,FechaFinal,ObrasExpuestas")] Exposicion exposicion)
    {
        if (ModelState.IsValid)
        {
            _context.Add(exposicion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(exposicion);
    }

    // GET: EXPOSICIONS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var exposicion = await _context.Exposiciones.FindAsync(id);
        if (exposicion == null)
        {
            return NotFound();
        }
        return View(exposicion);
    }

    // POST: EXPOSICIONS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre,FechaInicio,FechaFinal,ObrasExpuestas")] Exposicion exposicion)
    {
        if (id != exposicion.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(exposicion);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExposicionExists(exposicion.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(exposicion);
    }

    // GET: EXPOSICIONS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var exposicion = await _context.Exposiciones
            .FirstOrDefaultAsync(m => m.Id == id);
        if (exposicion == null)
        {
            return NotFound();
        }

        return View(exposicion);
    }

    // POST: EXPOSICIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var exposicion = await _context.Exposiciones.FindAsync(id);
        if (exposicion != null)
        {
            _context.Exposiciones.Remove(exposicion);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ExposicionExists(int? id)
    {
        return _context.Exposiciones.Any(e => e.Id == id);
    }

    [HttpGet]
    public async Task<IActionResult>SeleccionObra(int id)
    {
        

        var obras = await _context.Obras.ToListAsync();

        List<Guid> obrasIds = await _context.Exposiciones
            .Where(e => e.Id == id)
            .SelectMany(e => e.ObrasExpuestas)
            .Select(o => o.Id)
            .ToListAsync();

        ViewBag.ExposicionId = id;

        if(obrasIds != null)
          ViewBag.ObrasIncluidas = obrasIds;

        return View(obras);
    }

    [HttpPost]
    public async Task<IActionResult>SeleccionObra(int expoId, List<Guid> obraIds)
    {
        var expo = _context.Exposiciones
            .Include(e => e.ObrasExpuestas)
            .FirstOrDefault(e => e.Id == expoId);

        if(expo == null)
            return NotFound();

        foreach (var id in obraIds)
        {
            var obra = new Obra {Id = id};
            _context.Attach(obra);

            if(expo.ObrasExpuestas == null)
                expo.ObrasExpuestas = new List<Obra>();

            expo.ObrasExpuestas.Add(obra);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
