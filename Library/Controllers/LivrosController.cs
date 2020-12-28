using Library.Data;
using Library.Models;
using Library.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    [Authorize]
    public class LivrosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LivrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("livros/index")]
        // GET: Livros
        public async Task<IActionResult> Index(int? busca, string search)
        {
            var user = User.Identity.Name;

            var userId = (from c in _context.Users
                          where c.UserName == user
                          select c.Id).FirstOrDefault();

            List<Livros> Livro = new List<Livros>();

            List<SelectListItem> itens = new List<SelectListItem>();
            SelectListItem item1 = new SelectListItem() { Text = "Livro", Value = "1", Selected = true };
            SelectListItem item2 = new SelectListItem() { Text = "Autor", Value = "2", Selected = false };
            SelectListItem item3 = new SelectListItem() { Text = "Editora", Value = "3", Selected = false };
            itens.Add(item1);
            itens.Add(item2);
            itens.Add(item3);

            ViewBag.Busca = itens;

            if (busca != null)
            {
                itens.Where(i => i.Value == busca.ToString()).First().Selected = true;
            }
            if (busca == 1 && (!String.IsNullOrEmpty(search)))
            {
                var id = (from c in _context.Livro
                          where c.Titulo.Contains(search) &&
                          c.IdentityUserId == userId
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

                    var foto = selecionarFoto(ids);

                    Livro.Add(new Livros
                    {
                        Id = ids,
                        Titulo = titulo,
                        Edicao = edicao,
                        Ano = ano,
                        Editora = editora,
                        Autores = autores,
                        Assuntos = assuntos,
                        Foto = foto
                    });
                }
                ViewBag.Livro = Livro;
            }
            else if (busca == 2 && (!String.IsNullOrEmpty(search)))
            {
                var id = (from c in _context.Livro
                          join livroAutor in _context.LivroAutor
                          on c.Id equals livroAutor.LivroId
                          join autor in _context.Autor
                          on livroAutor.AutorId equals autor.Id
                          where autor.Nome.Contains(search) &&
                          c.IdentityUserId == userId
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

                    var foto = selecionarFoto(ids);

                    Livro.Add(new Livros
                    {
                        Id = ids,
                        Titulo = titulo,
                        Edicao = edicao,
                        Ano = ano,
                        Editora = editora,
                        Autores = autores,
                        Assuntos = assuntos,
                        Foto = foto
                    });
                }
                ViewBag.Livro = Livro;
            }
            else if (busca == 3 && (!String.IsNullOrEmpty(search)))
            {
                var id = (from c in _context.Livro
                          join editora in _context.Editora
                          on c.EditoraId equals editora.Id
                          where editora.Nome.Contains(search) &&
                          c.IdentityUserId == userId
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

                    var foto = selecionarFoto(ids);

                    Livro.Add(new Livros
                    {
                        Id = ids,
                        Titulo = titulo,
                        Edicao = edicao,
                        Ano = ano,
                        Editora = editora,
                        Autores = autores,
                        Assuntos = assuntos,
                        Foto = foto
                    });
                }
                ViewBag.Livro = Livro;
            }
            else
            {
                var id = (from c in _context.Livro
                          where c.IdentityUserId == userId
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

                    var foto = selecionarFoto(ids);

                    Livro.Add(new Livros
                    {
                        Id = ids,
                        Titulo = titulo,
                        Edicao = edicao,
                        Ano = ano,
                        Editora = editora,
                        Autores = autores,
                        Assuntos = assuntos,
                        Foto = foto
                    });
                }
                ViewBag.Livro = Livro;
            }

            return View();
        }

        [Route("livros/detalhes")]
        // GET: Livros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            carregaLivro(id);
            return View();
        }

        [Route("livros/adicionar")]
        // GET: Livros/Create
        public IActionResult Create()
        {
            var user = User.Identity.Name;

            var userId = (from c in _context.Users
                          where c.UserName == user
                          select c.Id).FirstOrDefault();
            var assuntoLista = (from c in _context.Livro
                                join livroAssunto in _context.LivroAssunto
                                on c.Id equals livroAssunto.LivroId
                                join assunto in _context.Assunto
                                on livroAssunto.AssuntoId equals assunto.Id
                                where c.IdentityUserId == userId
                                select assunto).ToList();

            ViewData["LivroAssunto"] = new SelectList(assuntoLista, "Id", "Nome");
            return View();
        }

        // POST: Livros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Edicao,Ano,Editora")] Livro livro, [Bind("Nome")] Assunto assunto, List<string> listaAutores, List<string> listaAssuntos, IList<IFormFile> foto)
        {
            var user = User.Identity.Name;

            var userId = (from c in _context.Users
                          where c.UserName == user
                          select c.Id).FirstOrDefault();

            var livroId = (from c in _context.Livro
                           where c.Titulo.Equals(livro.Titulo) &&
                           c.IdentityUserId == userId
                           select c.Id).FirstOrDefault();
            var livroEdicao = (from c in _context.Livro
                               where c.Titulo.Equals(livro.Titulo) &&
                               c.IdentityUserId == userId
                               select c.Edicao).FirstOrDefault();
            var livroEditora = (from c in _context.Livro
                                where c.Titulo.Equals(livro.Titulo) &&
                                c.IdentityUserId == userId
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
                //var autorBd = (from autor in _context.Autor
                //               join livroAutor in _context.LivroAutor
                //               on autor.Id equals livroAutor.AutorId
                //               join livroX in _context.Livro
                //               on livroAutor.LivroId equals livroX.Id
                //               where livroX.IdentityUserId == userId
                //               select autor).FirstOrDefault();

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
                livro.IdentityUserId = userId;
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

                    IFormFile fotoCapa = foto.FirstOrDefault();
                    if (fotoCapa != null || fotoCapa.ContentType.ToLower().StartsWith("image/"))
                    {
                        MemoryStream ms = new MemoryStream();
                        fotoCapa.OpenReadStream().CopyTo(ms);
                        livro.Dados = ms.ToArray();
                        livro.ContentType = fotoCapa.ContentType;
                    }

                    _context.Update(livro);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["LivroAssunto"] = new SelectList(_context.Assunto, "Id", "Nome");
            return View(livro);
        }

        [Route("livros/editar")]
        // GET: Livros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            carregaLivro(id);
            return View();
        }

        [Route("livros/editar")]
        // POST: Livros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string titulo, List<string> autores, int edicao, int ano, string editora, List<int> assuntos, IList<IFormFile> foto)
        {
            var user = User.Identity.Name;

            var userId = (from c in _context.Users
                          where c.UserName == user
                          select c.Id).FirstOrDefault();

            try
            {
                var livro = (from c in _context.Livro
                             where c.Id == id
                             && c.IdentityUserId == userId
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

                IFormFile fotoCapa = foto.FirstOrDefault();
                if (fotoCapa != null || fotoCapa.ContentType.ToLower().StartsWith("image/"))
                {
                    MemoryStream ms = new MemoryStream();
                    fotoCapa.OpenReadStream().CopyTo(ms);

                    var dados = (from c in _context.Livro
                                 where c.Id == livro.Id
                                 select c.Dados).FirstOrDefault();

                    if (dados != ms.ToArray())
                    {
                        livro.Dados = ms.ToArray();
                        livro.ContentType = fotoCapa.ContentType;
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

                var livroAssunto = (from c in _context.LivroAssunto
                                    where c.Livro == livro
                                    select c).ToList();

                var livroAssuntoId = (from c in _context.LivroAssunto
                                      where c.Livro == livro
                                      select c.AssuntoId).ToList();
                int cont = 0;

                foreach (var assunto in assuntos)
                {
                    var assuntoAtual = (from c in _context.Assunto
                                        where c.Id == assunto
                                        select c).FirstOrDefault();

                    if (livroAssuntoId[cont] != assunto)
                    {
                        livroAssunto[cont].Assunto = assuntoAtual;

                        _context.LivroAssunto.Update(livroAssunto[cont]);
                        await _context.SaveChangesAsync();
                    }
                    cont++;
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("livros/deletar")]
        // GET: Livros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            carregaLivro(id);
            return View();
        }

        [Route("livros/deletar")]
        // POST: Livros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livro = (from c in _context.Livro
                         where c.Id == id
                         select c).FirstOrDefault();
            var livroAssunto = (from c in _context.LivroAssunto
                                where c.Livro == livro
                                select c).FirstOrDefault();
            var livroAutor = (from c in _context.LivroAutor
                              where c.Livro == livro
                              select c).FirstOrDefault();

            _context.Livro.Remove(livro);
            _context.LivroAssunto.Remove(livroAssunto);
            _context.LivroAutor.Remove(livroAutor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public void carregaLivro(int? id)
        {
            var user = User.Identity.Name;

            var userId = (from c in _context.Users
                          where c.UserName == user
                          select c.Id).FirstOrDefault();

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
            var foto = selecionarFoto(Convert.ToInt32(id));


            Livro.Add(new Livros
            {
                Id = Convert.ToInt32(id),
                Titulo = titulo,
                Edicao = edicao,
                Ano = ano,
                Editora = editora,
                Autores = autores,
                Assuntos = assuntos,
                Foto = foto
            });

            var assuntosId = (from assunto in _context.Assunto
                              join LivroAssunto in _context.LivroAssunto
                              on assunto.Id equals LivroAssunto.AssuntoId
                              join livro in _context.Livro
                              on LivroAssunto.LivroId equals livro.Id
                              where livro.Id == id
                              select assunto.Id).ToList();

            List<SelectList> listas = new List<SelectList>();
            for (int i = 0; i < assuntosId.Count; i++)
            {
                listas.Add(new SelectList(_context.Assunto, "Id", "Nome", assuntosId[i]));
            }
            ViewBag.Assunto = listas;
            ViewBag.Livro = Livro;
        }

        public FileStreamResult selecionarFoto(int id)
        {
            var fotoDados = (from c in _context.Livro
                             where c.Id == id
                             select c.Dados).FirstOrDefault();
            var fotoContentType = (from c in _context.Livro
                                   where c.Id == id
                                   select c.ContentType).FirstOrDefault();
            if (fotoDados != null)
            {
                MemoryStream ms = new MemoryStream(fotoDados);
                return new FileStreamResult(ms, fotoContentType);
            }
            else
            {
                return null;
            }
        }

        private bool LivroExists(int id)
        {
            return _context.Livro.Any(e => e.Id == id);
        }
    }
}
