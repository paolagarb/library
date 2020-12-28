using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    [Authorize]
    public class AssuntosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssuntosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Assuntos
        public async Task<IActionResult> Index()
        {
            var user = User.Identity.Name;

            var userId = (from c in _context.Users
                          where c.UserName == user
                          select c.Id).FirstOrDefault();
            var assuntos = (from c in _context.Assunto
                     where c.IdentityUserId == userId
                     select c).ToList();
            
            return View(assuntos);
        }

        // GET: Assuntos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assunto = await _context.Assunto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assunto == null)
            {
                return NotFound();
            }

            return View(assunto);
        }

        // GET: Assuntos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Assuntos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Assunto assunto)
        {
            var user = User.Identity.Name;

            var userId = (from c in _context.Users
                          where c.UserName == user
                          select c.Id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                assunto.IdentityUserId = userId;
                _context.Add(assunto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(assunto);
        }

        // GET: Assuntos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assunto = await _context.Assunto.FindAsync(id);
            if (assunto == null)
            {
                return NotFound();
            }
            return View(assunto);
        }

        // POST: Assuntos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Assunto assunto)
        {
            if (id != assunto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assunto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssuntoExists(assunto.Id))
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
            return View(assunto);
        }

        private bool AssuntoExists(int id)
        {
            return _context.Assunto.Any(e => e.Id == id);
        }
    }
}
