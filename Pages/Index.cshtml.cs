using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KalkulosCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace KalkulosCore.Pages
{
    [Authorize] // Hacemos que la página de inicio requiera iniciar sesión
    public class IndexModel : PageModel
    {
        private readonly KuotasDbContext _context;

        public IndexModel(KuotasDbContext context)
        {
            _context = context;
        }

        // Propiedades para almacenar los datos de las 3 tablas
        public IList<Pagostotal> PagosRecientesPorDeudor { get; set; } = new List<Pagostotal>();
        public IList<Pago> UltimosPagos { get; set; } = new List<Pago>();
        public IList<Prestamo> PrestamosActivos { get; set; } = new List<Prestamo>();

        public async Task OnGetAsync()
        {
            // --- Consulta 1: Últimos pagos totales por deudor ---
            PagosRecientesPorDeudor = await _context.Pagostotals
                .OrderByDescending(p => p.Fecha)
                .Take(10) // Tomamos los 10 más recientes
                .ToListAsync();

            // --- Consulta 2: Últimos pagos de cuotas individuales ---
            UltimosPagos = await _context.Pagos
                .Include(p => p.IdclienteNavigation) // Incluimos al cliente para poder mostrar su nombre
                .OrderByDescending(p => p.Fecha)
                .Take(10) // Tomamos los 10 más recientes
                .ToListAsync();

            // --- Consulta 3: Todas las operaciones activas ---
            PrestamosActivos = await _context.Prestamos
                .Include(p => p.IdclienteNavigation)
                .Include(p => p.Cuota) // Incluimos las cuotas para poder revisarlas
                .Where(p => p.Cuota.Any(c => c.Estado == "PENDIENTE" || c.Estado == "PENDIENTE CAPITAL"))
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();
        }
    }
}