using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCRUD_PIM_VIII.Data;
using WebCRUD_PIM_VIII.Models;

namespace WebCRUD_PIM_VIII.Controllers
{
    public class TipoTelefonesController : Controller
    {
        private readonly WebCRUDContext _context;

        public TipoTelefonesController(WebCRUDContext context)
        {
            _context = context;
        }

        // GET: TipoTelefones
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoTelefone.ToListAsync());
        }

        // GET: TipoTelefones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTelefone = await _context.TipoTelefone
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoTelefone == null)
            {
                return NotFound();
            }

            return View(tipoTelefone);
        }

        // GET: TipoTelefones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoTelefones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tipo")] TipoTelefone tipoTelefone)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoTelefone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoTelefone);
        }

        // GET: TipoTelefones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTelefone = await _context.TipoTelefone.FindAsync(id);
            if (tipoTelefone == null)
            {
                return NotFound();
            }
            return View(tipoTelefone);
        }

        // POST: TipoTelefones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tipo")] TipoTelefone tipoTelefone)
        {
            if (id != tipoTelefone.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoTelefone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoTelefoneExists(tipoTelefone.Id))
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
            return View(tipoTelefone);
        }

        // GET: TipoTelefones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTelefone = await _context.TipoTelefone
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoTelefone == null)
            {
                return NotFound();
            }

            return View(tipoTelefone);
        }

        // POST: TipoTelefones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoTelefone = await _context.TipoTelefone.FindAsync(id);
            _context.TipoTelefone.Remove(tipoTelefone);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoTelefoneExists(int id)
        {
            return _context.TipoTelefone.Any(e => e.Id == id);
        }
    }
}
