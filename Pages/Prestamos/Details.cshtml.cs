using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KalkulosCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace KalkulosCore.Pages.Prestamos
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly KuotasDbContext _context;

        public DetailsModel(KuotasDbContext context)
        {
            _context = context;
        }

        public Prestamo Prestamo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Esta consulta trae el Préstamo e incluye los datos relacionados que necesitamos
            var prestamo = await _context.Prestamos
                .Include(p => p.IdclienteNavigation)  // Incluye los datos del Cliente
                .Include(p => p.IdacreedorNavigation) // Incluye los datos del Acreedor
                .Include(p => p.Cuota)               // Incluye TODAS las cuotas de este préstamo
                .FirstOrDefaultAsync(m => m.Operacion == id);

            if (prestamo == null)
            {
                return NotFound();
            }

            Prestamo = prestamo;
            return Page();
        }
    }
}