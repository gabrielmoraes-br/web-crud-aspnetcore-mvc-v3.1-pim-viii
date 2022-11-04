using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySql.Data.MySqlClient;
using WebCRUD_PIM_VIII.Data;
using WebCRUD_PIM_VIII.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebCRUD_PIM_VIII.Controllers
{
    public class PessoasController : Controller
    {
        private readonly WebCRUDContext _context;

        public PessoasController(WebCRUDContext context)
        {
            _context = context;
        }

        // GET: Pessoas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pessoa.ToListAsync());
        }

        // GET: Pessoas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Pessoa não encontrada." });
            }

            var obj = await _context.Pessoa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Pessoa não encontrada." });
            }

            Pessoa pessoa = PessoaDAO.Consulte(obj.CPF);

            return View(pessoa);
        }

        // GET: Pessoas/Details/5
        public IActionResult Search(long cpf)
        {
            Pessoa pessoa = PessoaDAO.Consulte(cpf);

            if (pessoa.Id == 0)
            {
                return RedirectToAction(nameof(Error), new { message = "Pessoa não encontrada." });
            }

            return View("Details", pessoa);
        }

        // GET: Pessoas/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pessoa pessoa)
        {
            PessoaDAO.Insira(pessoa);
            return RedirectToAction(nameof(Index));
        }

        // GET: Pessoas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Pessoa não encontrada." });
            }

            var obj = await _context.Pessoa.FindAsync(id);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Pessoa não encontrada." });
            }

            Pessoa pessoa = PessoaDAO.Consulte(obj.CPF);

            return View(pessoa);
        }

        // POST: Pessoas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CPF,Endereco,Telefones")] Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Pessoa não encontrada." });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    PessoaDAO.Altere(pessoa);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PessoaExists(pessoa.Id))
                    {
                        return RedirectToAction(nameof(Error), new { message = "Pessoa não encontrada." });
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pessoa);
        }

        // GET: Pessoas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Pessoa não encontrada." });
            }

            var obj = await _context.Pessoa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Pessoa não encontrada." });
            }

            Pessoa pessoa = PessoaDAO.Consulte(obj.CPF);

            return View(pessoa);
        }

        // POST: Pessoas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pessoa = await _context.Pessoa.FindAsync(id);
            PessoaDAO.Exclua(PessoaDAO.Consulte(pessoa.CPF));
            return RedirectToAction(nameof(Index));
        }

        private bool PessoaExists(int id)
        {
            return _context.Pessoa.Any(e => e.Id == id);
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel 
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = message
            };
            return View(viewModel);
        }
    }
}
