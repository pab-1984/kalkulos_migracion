using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KalkulosCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace KalkulosCore.Pages.Pagos
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly KuotasDbContext _context;

        public IndexModel(KuotasDbContext context)
        {
            _context = context;
        }

        // Propiedad para recibir el término de búsqueda del formulario
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        // Lista para guardar los resultados de la búsqueda
        public IList<Prestamo> Prestamos { get;set; } = default!;

        public async Task OnGetAsync()
        {
            // Preparamos una consulta base para los préstamos
            var prestamosQuery = _context.Prestamos
                                    .Include(p => p.IdclienteNavigation) // Incluimos al cliente para mostrar su nombre
                                    .Where(p => p.Cuota.Any(c => c.Estado == "PENDIENTE")); // Solo préstamos con cuotas pendientes

            if (!string.IsNullOrEmpty(SearchString))
            {
                // Si hay un término de búsqueda, filtramos por nombre de cliente o por número de operación
                prestamosQuery = prestamosQuery.Where(p => p.IdclienteNavigation.Nombres.Contains(SearchString) 
                                                        || p.Operacion.ToString() == SearchString);
            }

            Prestamos = await prestamosQuery.OrderByDescending(p => p.Fecha).ToListAsync();
        }
    }
}