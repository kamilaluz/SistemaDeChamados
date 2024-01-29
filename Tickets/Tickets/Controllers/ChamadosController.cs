using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tickets.Models;
using Tickets.Services;

namespace Tickets.Controllers
{
    [Authorize]

    public class ChamadosController : Controller
    {
        private readonly ApplicationDbContext _context;



        public ChamadosController(ApplicationDbContext context)
        {
            _context = context;

        }

        //CHAMADOS
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Chamados.Include(c => c.Atendente).Include(c => c.Funcionario).Include(c => c.Status).OrderByDescending(c => c.Id);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chamado = await _context.Chamados
                .Include(c => c.Atendente)
                .Include(c => c.Funcionario)
                .Include(c => c.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chamado == null)
            {
                return NotFound();
            }

            var comentarios = await _context.Comentarios
            .Include(c => c.Autor)
            .Where(c => c.ChamadoId == id)
            .OrderByDescending(c => c.Id)
            .ToListAsync();

            var viewModel = new ChamadoComentarios
            {
                Chamado = chamado,
                ListaComentarios = comentarios
            };
            return View(viewModel);
        }

        public IActionResult Create()
        {
            ViewData["AtendenteId"] = new SelectList(_context.Usuarios
                .Include(u => u.Cargo)
                .Where(u => u.Cargo.Nome == "Atendente")
                .ToList(), "Id", "Nome");
            ViewData["FuncionarioId"] = new SelectList(_context.Usuarios
                .Include(u => u.Cargo)
                .Where(u => u.Cargo.Nome != "Atendente")
                .ToList(), "Id", "Nome");
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Nome");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string atendenteId, string funcionarioId, Chamado chamado)
        {
            var atendente = await _context.Usuarios
                .Include(c => c.Cargo)
                .FirstOrDefaultAsync(a => a.Id == atendenteId);

            var funcionario = await _context.Usuarios
                .Include(c => c.Cargo)
                .FirstOrDefaultAsync(f => f.Id == funcionarioId);

            var status = await _context.Status
                .FirstOrDefaultAsync(s => s.Nome == "Aguardando");


            var dataAtual = DateTime.UtcNow;

            Chamado novoChamado = new Chamado();
            novoChamado.Titulo = chamado.Titulo;
            novoChamado.Descricao = chamado.Descricao;
            novoChamado.Atendente = atendente;
            novoChamado.Funcionario = funcionario;
            novoChamado.Status = status;
            novoChamado.Data = dataAtual;

            _context.Add(novoChamado);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var chamado = await _context.Chamados
                .FindAsync(id);
            if (chamado == null)
            {
                return NotFound();
            }

            ViewData["FuncionarioId"] = new SelectList(_context.Usuarios
                .Include(u => u.Cargo)
                .Where(u => u.Cargo.Nome != "Atendente"), "Id", "Nome", chamado.FuncionarioId);
            ViewData["AtendenteId"] = new SelectList(_context.Usuarios
                .Include(u => u.Cargo)
                .Where(u => u.Cargo.Nome == "Atendente"), "Id", "Nome", chamado.AtendenteId);
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Nome", chamado.StatusId);


            return View(chamado);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int statusId, Chamado chamado)
        {
            var chamadoEditado = await _context.Chamados.FindAsync(chamado.Id);

            var status = await _context.Status
                .FirstOrDefaultAsync(f => f.Id == statusId);

            chamadoEditado.Id = chamado.Id;
            chamadoEditado.Titulo = chamado.Titulo;
            chamadoEditado.Descricao = chamado.Descricao;
            chamadoEditado.Status = status;
            chamadoEditado.FuncionarioId = chamado.FuncionarioId;
            chamadoEditado.AtendenteId = chamado.AtendenteId;

            _context.Update(chamadoEditado);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "chamado editado com sucesso.";

            return RedirectToAction("Details", new { id = chamado.Id });

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chamado = await _context.Chamados
                .Include(c => c.Atendente)
                .Include(c => c.Funcionario)
                .Include(c => c.Status)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (chamado == null)
            {
                return NotFound();
            }

            return View(chamado);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chamado = await _context.Chamados.FindAsync(id);
            if (chamado != null)
            {
                _context.Chamados.Remove(chamado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //COMENTARIOS

        public IActionResult AddComentario([FromQuery] int chamadoId)
        {


            ViewData["AutorId"] = new SelectList(_context.Usuarios, "Id", "Nome");
            ViewData["ChamadoId"] = chamadoId;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComentario(string autorId, Comentario comentario)
        {
            var autor = await _context.Usuarios
                .Include(c => c.Cargo)
                .FirstOrDefaultAsync(f => f.Id == autorId);

            var dataAtual = DateTime.UtcNow;

            Comentario novoComentario = new Comentario();
            novoComentario.Titulo = comentario.Titulo;
            novoComentario.Descricao = comentario.Descricao;
            novoComentario.Autor = autor;
            novoComentario.Data = dataAtual;
            novoComentario.ChamadoId = comentario.ChamadoId;

            _context.Comentarios.Add(novoComentario);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = comentario.ChamadoId });
        }

        public async Task<IActionResult> EditarComentario(int id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario == null)
            {
                return NotFound();
            }

            ViewData["AutorId"] = new SelectList(_context.Usuarios, "Id", "Nome", comentario.AutorId);
            return View(comentario);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarComentario(string autorId, Comentario comentario)
        {
            var comentarioEditado = await _context.Comentarios.FindAsync(comentario.Id);
            var autor = await _context.Usuarios
                .Include(c => c.Cargo)
                .FirstOrDefaultAsync(f => f.Id == autorId);


            comentarioEditado.Id = comentario.Id;
            comentarioEditado.Titulo = comentario.Titulo;
            comentarioEditado.Descricao = comentario.Descricao;
            comentarioEditado.Autor = autor;

            _context.Update(comentarioEditado);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Comentário editado com sucesso.";

            return RedirectToAction("Details", new { id = comentarioEditado.ChamadoId });

        }

        public async Task<IActionResult> ExcluirComentario(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentarios
                .Include(c => c.Autor)
                .Include(c => c.Chamado)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comentario == null)
            {
                return NotFound();
            }

            return View(comentario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirComentario(int id)
        {
            var excluir = await _context.Comentarios.FindAsync(id);
            if (excluir != null)
            {
                _context.Comentarios.Remove(excluir);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = excluir.ChamadoId });
        }


        private bool ComentarioExists(int id)
        {
            return _context.Comentarios.Any(e => e.Id == id);
        }



    }
}
