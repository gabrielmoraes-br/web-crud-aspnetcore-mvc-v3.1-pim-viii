using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCRUD_PIM_VIII.Data;
using WebCRUD_PIM_VIII.Models;
using WebCRUD_PIM_VIII.Models.ViewModels;

namespace WebCRUD_PIM_VIII.Controllers
{
    public class PessoasController : Controller
    {
        private readonly WebCRUDContext _context;

        public PessoasController(WebCRUDContext context)
        {
            _context = context;
        }

        // GET: Pessoas/Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pessoa.ToListAsync());
        }

        // GET: Pessoas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pessoas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pessoa pessoa)
        {
            PessoaDAO.Insira(pessoa);
            return RedirectToAction(nameof(Index));
        }

        // GET: Pessoas/Details
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

        // GET: Pessoas/Edit
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

        // POST: Pessoas/Edit
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

        // GET: Pessoas/Delete
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

        // POST: Pessoas/Delete
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

        // Método de busca que apresenta os datalhes do CPF pesquisado.
        public IActionResult Search(long cpf)
        {
            Pessoa pessoa = PessoaDAO.Consulte(cpf);

            if (pessoa.Id == 0)
            {
                return RedirectToAction(nameof(Error), new { message = "Pessoa não encontrada." });
            }

            return View("Details", pessoa);
        }

        //Método que redireciona o usuário para página de erro.
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
