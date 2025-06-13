using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KalkulosCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace KalkulosCore.Pages.Clientes
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly KuotasDbContext _context;

        public DetailsModel(KuotasDbContext context)
        {
            _context = context;
        }

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
    }
}