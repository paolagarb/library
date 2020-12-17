using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class LivrosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LivrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Livros
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Livro.Include(l => l.Editora);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Livros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livro
                .Include(l => l.Editora)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        // GET: Livros/Create
        public IActionResult Create()
        {
            ViewData["LivroAssunto"] = new SelectList(_context.Assunto, "Id", "Nome");

            return View();
        }

        // POST: Livros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Edicao,Ano,Editora")] Livro livro, [Bind("Nome")] Assunto assunto, [Bind("Nome")] Autor autor )
        {
            var autorBd = (from c in _context.Autor
                          where c.Nome.Equals(autor.Nome)
                          select c).FirstOrDefault();

            Autor a1 = new Autor();

            if (autorBd == null)
            {
                a1.Nome = autor.Nome;
                _context.Autor.Add(a1);
                await _context.SaveChangesAsync();
            } else
            {
                a1 = autorBd;
            }

            int assuntoId = Convert.ToInt32(assunto.Nome);
            var assuntoSelecionado = (from c in _context.Assunto
                                     where c.Id.Equals(assuntoId)
                                     select c).FirstOrDefault();
            LivroAutor livroAutor = new LivroAutor();
            livroAutor.Autor = a1;
            livroAutor.Livro = livro;
            _context.LivroAutor.Add(livroAutor);
            await _context.SaveChangesAsync();

            LivroAssunto livroAssunto = new LivroAssunto();
            livroAssunto.Assunto = assuntoSelecionado;
            livroAssunto.Livro = livro;
            _context.LivroAssunto.Add(livroAssunto);
            await _context.SaveChangesAsync();

            livro.LivroAssunto.Add(livroAssunto);
            livro.LivroAutor.Add(livroAutor);

            if (ModelState.IsValid)
            {

                var editora = (from c in _context.Editora
                               where c.Nome.Equals(livro.Editora.Nome)
                               select c.Nome).FirstOrDefault();
                if (editora != null)
                {
                    var editoraId = (from c in _context.Editora
                                     where c.Nome.Equals(livro.Editora.Nome)
                                     select c.Id).FirstOrDefault();
                    livro.EditoraId = editoraId;
                    _context.Add(livro);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                } else
                {
                    _context.Editora.Add(new Editora
                    {
                        Nome = livro.Editora.Nome
                    });
                    await _context.SaveChangesAsync();
                    var editoraId = (from c in _context.Editora
                                   where c.Nome.Equals(livro.Editora.Nome)
                                   select c.Id).FirstOrDefault();
                    livro.EditoraId = editoraId;
                    _context.Add(livro);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["LivroAssunto"] = new SelectList(_context.Assunto, "Id", "Nome");
            return View(livro);
        }

        // GET: Livros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livro.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }
            ViewData["EditoraId"] = new SelectList(_context.Set<Editora>(), "Id", "Nome", livro.EditoraId);
            return View(livro);
        }

        // POST: Livros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Edicao,Ano,EditoraId")] Livro livro)
        {
            if (id != livro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(livro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivroExists(livro.Id))
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
            ViewData["EditoraId"] = new SelectList(_context.Set<Editora>(), "Id", "Nome", livro.EditoraId);
            return View(livro);
        }

        // GET: Livros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livro
                .Include(l => l.Editora)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        // POST: Livros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livro = await _context.Livro.FindAsync(id);
            _context.Livro.Remove(livro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LivroExists(int id)
        {
            return _context.Livro.Any(e => e.Id == id);
        }
    }
}
