using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Create([Bind("Id,Titulo,Edicao,Ano,Editora")] Livro livro, [Bind("Nome")] Assunto assunto, List<string> listaAutores)
        {
            //var list = Request.Form["listaAutores"];


            var livroId = (from c in _context.Livro
                           where c.Titulo.Equals(livro.Titulo)
                           select c.Id).FirstOrDefault();
            var livroEdicao = (from c in _context.Livro
                               where c.Titulo.Equals(livro.Titulo)
                               select c.Edicao).FirstOrDefault();
            var livroEditora = (from c in _context.Livro
                                where c.Titulo.Equals(livro.Titulo)
                                select c.Editora).FirstOrDefault();

            if (livroId != 0 && livroEdicao == livro.Edicao && livroEditora == livro.Editora)
            {
                ViewData["JaExiste"] = "Esse livro já está cadastrado no sistema!";
                return View();
            }

            Autor autor1 = new Autor();
            List<Autor> listAutorLivro = new List<Autor>();

            foreach (var autorList in listaAutores)
            {
                var autorBd = (from c in _context.Autor
                               where c.Nome.Equals(autorList)
                               select c).FirstOrDefault();

                if (autorBd == null)
                {
                    autor1.Nome = autorList;
                    _context.Autor.Add(autor1);
                    await _context.SaveChangesAsync();
                    listAutorLivro.Add(autor1);
                }
                else
                {
                    autor1 = autorBd; 
                    listAutorLivro.Add(autor1);
                }
            }

            int assuntoId = Convert.ToInt32(assunto.Nome);
            var assuntoSelecionado = (from c in _context.Assunto
                                      where c.Id.Equals(assuntoId)
                                      select c).FirstOrDefault();

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

                    foreach (var autorList in listAutorLivro)
                    {
                        LivroAutor livroAutor = new LivroAutor();
                        livroAutor.Autor = autorList;
                        livroAutor.Livro = livro;
                        _context.LivroAutor.Add(livroAutor);
                        await _context.SaveChangesAsync();

                        livro.LivroAutor.Add(livroAutor);

                    }
                    LivroAssunto livroAssunto = new LivroAssunto();
                        livroAssunto.Assunto = assuntoSelecionado;
                        livroAssunto.Livro = livro;
                        _context.LivroAssunto.Add(livroAssunto);
                        await _context.SaveChangesAsync();

                        livro.LivroAssunto.Add(livroAssunto);
                        return RedirectToAction(nameof(Index));
                }
                else
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

                    foreach (var autorList in listAutorLivro)
                    {
                        LivroAutor livroAutor = new LivroAutor();
                        livroAutor.Autor = autorList;
                        livroAutor.Livro = livro;
                        _context.LivroAutor.Add(livroAutor);
                        await _context.SaveChangesAsync(); 
                        
                        livro.LivroAutor.Add(livroAutor);
                    }
                    LivroAssunto livroAssunto = new LivroAssunto();
                    livroAssunto.Assunto = assuntoSelecionado;
                    livroAssunto.Livro = livro;
                    _context.LivroAssunto.Add(livroAssunto);
                    await _context.SaveChangesAsync();

                    livro.LivroAssunto.Add(livroAssunto);
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
