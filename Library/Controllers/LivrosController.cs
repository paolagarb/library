using Library.Data;
using Library.Models;
using Library.Models.ViewModels;
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
            List<Livros> Livro = new List<Livros>();

            var id = (from c in _context.Livro
                      select c.Id).ToList();

            foreach (var ids in id)
            {
                var titulo = (from livro in _context.Livro
                              where livro.Id == ids
                              select livro.Titulo).FirstOrDefault();

                var edicao = (from livro in _context.Livro
                              where livro.Id == ids
                              select livro.Edicao).FirstOrDefault();

                var ano = (from livro in _context.Livro
                           where livro.Id == ids
                           select livro.Ano).FirstOrDefault();

                var editora = (from livro in _context.Livro
                               where livro.Id == ids
                               select livro.Editora.Nome).FirstOrDefault();

                var autores = (from autor in _context.Autor
                               join livroAutor in _context.LivroAutor
                               on autor.Id equals livroAutor.AutorId
                               join livro in _context.Livro
                               on livroAutor.LivroId equals livro.Id
                               where livro.Id == ids
                               select autor.Nome).ToList();

                var assuntos = (from assunto in _context.Assunto
                                join LivroAssunto in _context.LivroAssunto
                                on assunto.Id equals LivroAssunto.AssuntoId
                                join livro in _context.Livro
                                on LivroAssunto.LivroId equals livro.Id
                                where livro.Id == ids
                                select assunto.Nome).ToList();

                Livro.Add(new Livros
                {
                    Id = ids,
                    Titulo = titulo,
                    Edicao = edicao,
                    Ano = ano,
                    Editora = editora,
                    Autores = autores,
                    Assuntos = assuntos
                });
            }

            ViewBag.Livro = Livro;
            return View(Livro);
        }

        // GET: Livros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Livros> Livro = new List<Livros>();

            var titulo = (from livro in _context.Livro
                          where livro.Id == id
                          select livro.Titulo).FirstOrDefault();

            var edicao = (from livro in _context.Livro
                          where livro.Id == id
                          select livro.Edicao).FirstOrDefault();

            var ano = (from livro in _context.Livro
                       where livro.Id == id
                       select livro.Ano).FirstOrDefault();

            var editora = (from livro in _context.Livro
                           where livro.Id == id
                           select livro.Editora.Nome).FirstOrDefault();

            var autores = (from autor in _context.Autor
                           join livroAutor in _context.LivroAutor
                           on autor.Id equals livroAutor.AutorId
                           join livro in _context.Livro
                           on livroAutor.LivroId equals livro.Id
                           where livro.Id == id
                           select autor.Nome).ToList();

            var assuntos = (from assunto in _context.Assunto
                            join LivroAssunto in _context.LivroAssunto
                            on assunto.Id equals LivroAssunto.AssuntoId
                            join livro in _context.Livro
                            on LivroAssunto.LivroId equals livro.Id
                            where livro.Id == id
                            select assunto.Nome).ToList();

            Livro.Add(new Livros
            {
                Id = Convert.ToInt32(id),
                Titulo = titulo,
                Edicao = edicao,
                Ano = ano,
                Editora = editora,
                Autores = autores,
                Assuntos = assuntos
            });
            ViewBag.Livro = Livro;

            return View();
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
        public async Task<IActionResult> Create([Bind("Id,Titulo,Edicao,Ano,Editora")] Livro livro, [Bind("Nome")] Assunto assunto, List<string> listaAutores, List<string> listaAssuntos)
        {

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

            List<Autor> listAutorLivro = new List<Autor>();

            foreach (var autorList in listaAutores)
            {
                Autor autor1 = new Autor();
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
                    listAutorLivro.Add(autorBd);
                }
            }

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

                    foreach (var assuntos in listaAssuntos)
                    {
                        LivroAssunto livroAssunto = new LivroAssunto();
                        int assuntoId = Convert.ToInt32(assuntos);
                        var assuntoSelecionado = (from c in _context.Assunto
                                                  where c.Id.Equals(assuntoId)
                                                  select c).FirstOrDefault();
                        livroAssunto.Assunto = assuntoSelecionado;
                        livroAssunto.Livro = livro;
                        _context.LivroAssunto.Add(livroAssunto);
                        await _context.SaveChangesAsync();

                        livro.LivroAssunto.Add(livroAssunto);
                    }

                    _context.Update(livro);
                    await _context.SaveChangesAsync();

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

                    foreach (var assuntos in listaAssuntos)
                    {
                        LivroAssunto livroAssunto = new LivroAssunto();
                        int assuntoId = Convert.ToInt32(assuntos);
                        var assuntoSelecionado = (from c in _context.Assunto
                                                  where c.Id.Equals(assuntoId)
                                                  select c).FirstOrDefault();

                        livroAssunto.Assunto = assuntoSelecionado;
                        livroAssunto.Livro = livro;
                        _context.LivroAssunto.Add(livroAssunto);
                        await _context.SaveChangesAsync();

                        livro.LivroAssunto.Add(livroAssunto);
                    }

                    _context.Update(livro);
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

            List<Livros> Livro = new List<Livros>();

            var titulo = (from livro in _context.Livro
                          where livro.Id == id
                          select livro.Titulo).FirstOrDefault();

            var edicao = (from livro in _context.Livro
                          where livro.Id == id
                          select livro.Edicao).FirstOrDefault();

            var ano = (from livro in _context.Livro
                       where livro.Id == id
                       select livro.Ano).FirstOrDefault();

            var editora = (from livro in _context.Livro
                           where livro.Id == id
                           select livro.Editora.Nome).FirstOrDefault();

            var autores = (from autor in _context.Autor
                           join livroAutor in _context.LivroAutor
                           on autor.Id equals livroAutor.AutorId
                           join livro in _context.Livro
                           on livroAutor.LivroId equals livro.Id
                           where livro.Id == id
                           select autor.Nome).ToList();

            var assuntos = (from assunto in _context.Assunto
                            join LivroAssunto in _context.LivroAssunto
                            on assunto.Id equals LivroAssunto.AssuntoId
                            join livro in _context.Livro
                            on LivroAssunto.LivroId equals livro.Id
                            where livro.Id == id
                            select assunto.Nome).ToList();

            Livro.Add(new Livros
            {
                Id = Convert.ToInt32(id),
                Titulo = titulo,
                Edicao = edicao,
                Ano = ano,
                Editora = editora,
                Autores = autores,
                Assuntos = assuntos
            });

            var assuntosId = (from assunto in _context.Assunto
                              join LivroAssunto in _context.LivroAssunto
                              on assunto.Id equals LivroAssunto.AssuntoId
                              join livro in _context.Livro
                              on LivroAssunto.LivroId equals livro.Id
                              where livro.Id == id
                              select assunto.Id).ToList();

            List<SelectList> listas = new List<SelectList>();
            for (int i =0; i< assuntosId.Count; i++)
            {
                listas.Add(new SelectList(_context.Assunto, "Id", "Nome", assuntosId[i]));
            }
            ViewBag.Assunto = listas; //new SelectList(_context.Assunto, "Id", "Nome", assuntosId);
            ViewBag.Livro = Livro;
            return View(Livro);
        }

        // POST: Livros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Edicao,Ano,Editora,Autor,Assunto")] Livros livros)
        public async Task<IActionResult> Edit(int id, string titulo, List<string> autores, int edicao, int ano, string editora, List<int> assuntos)
        {

            try
            {
                var livro = (from c in _context.Livro
                             where c.Id == id
                             select c).FirstOrDefault();

                var tituloLivro = (from c in _context.Livro
                                   where c.Id == id
                                   select c.Titulo).FirstOrDefault();

                if (titulo != tituloLivro)
                {
                    livro.Titulo = titulo;
                }

                var edicaoLivro = (from c in _context.Livro
                                   where c.Id == id
                                   select c.Edicao).FirstOrDefault();

                if (edicao != edicaoLivro)
                {
                    livro.Edicao = edicao;
                }

                var anoLivro = (from c in _context.Livro
                                where c.Id == id
                                select c.Ano).FirstOrDefault();

                if (ano != anoLivro)
                {
                    livro.Ano = ano;
                }

                var editoraLivro = (from c in _context.Livro
                                    where c.Id == id
                                    select c.Editora.Nome).FirstOrDefault();

                if (editora != editoraLivro)
                {
                    var editoraNova = (from c in _context.Editora
                                       where c.Nome == editora
                                       select c).FirstOrDefault();

                    if (editoraNova == null)
                    {
                        Editora edit = new Editora();
                        edit.Nome = editora;
                        _context.Editora.Add(edit);
                        await _context.SaveChangesAsync();

                        livro.Editora = edit;
                    }
                    else
                    {
                        livro.Editora = editoraNova;
                    }
                }

                _context.Livro.Update(livro);
                await _context.SaveChangesAsync();

                var livroAutor = (from c in _context.LivroAutor
                                  where c.Livro == livro
                                  select c).ToList();
                int i = 0;

                foreach (var autor in autores)
                {
                    var autor1 = (from c in _context.Autor
                                  where c.Nome == autor
                                  select c).FirstOrDefault();

                    if (autor1 == null)
                    {
                        Autor autor2 = new Autor();
                        autor2.Nome = autor;
                        _context.Autor.Add(autor2);
                        await _context.SaveChangesAsync();

                        livroAutor[i].Autor = autor2;
                        _context.LivroAutor.Update(livroAutor[i]);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (livroAutor[i].Autor != autor1)
                        {
                            livroAutor[i].Autor = autor1;
                            _context.LivroAutor.Update(livroAutor[i]);
                            await _context.SaveChangesAsync();
                        }
                    }
                    i++;
                }


            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
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
