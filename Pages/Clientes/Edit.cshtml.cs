using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KalkulosCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace KalkulosCore.Pages.Clientes
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly KuotasDbContext _context;

        public EditModel(KuotasDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cliente Cliente { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FirstOrDefaultAsync(m => m.Idcliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            Cliente = cliente;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Aquí se puede añadir lógica para comprobar si el registro
                // fue eliminado por otro usuario mientras lo editábamos.
                // Por ahora, lo mantenemos simple.
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}