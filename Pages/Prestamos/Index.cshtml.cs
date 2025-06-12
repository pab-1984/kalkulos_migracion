using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KalkulosCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace KalkulosCore.Pages.Prestamos
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly KuotasDbContext _context;

        public IndexModel(KuotasDbContext context)
        {
            _context = context;
        }

        public IList<Prestamo> Prestamo { get;set; } = default!;

        public async Task OnGetAsync()
        {
            // Usamos .Include() para traer también los datos del Cliente y Acreedor relacionados
            Prestamo = await _context.Prestamos
                .Include(p => p.IdclienteNavigation)
                .Include(p => p.IdacreedorNavigation)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();
        }
    }
}