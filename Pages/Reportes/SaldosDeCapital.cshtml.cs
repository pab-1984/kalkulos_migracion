using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KalkulosCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace KalkulosCore.Pages.Reportes
{
    [Authorize]
    public class SaldosDeCapitalModel : PageModel
    {
        private readonly KuotasDbContext _context;

        public SaldosDeCapitalModel(KuotasDbContext context)
        {
            _context = context;
        }

        public double TotalCapitalDolares { get; set; }
        public double TotalCapitalPesos { get; set; }
        public double TotalCapitalUi { get; set; }

        public async Task OnGetAsync()
        {
            // Consulta para Dólares (U$S)
            TotalCapitalDolares = await _context.Cuotas
                .Where(c => c.Estado == "PENDIENTE CAPITAL" 
                            && c.Tipodecuotas == "CAPITAL" // <-- FILTRO AÑADIDO
                            && c.Moneda == "U$S")
                .SumAsync(c => c.Monto ?? 0);

            // Consulta para Pesos ($)
            TotalCapitalPesos = await _context.Cuotas
                .Where(c => c.Estado == "PENDIENTE CAPITAL" 
                            && c.Tipodecuotas == "CAPITAL" // <-- FILTRO AÑADIDO
                            && c.Moneda == "$")
                .SumAsync(c => c.Monto ?? 0);

            // Consulta para UI
            TotalCapitalUi = await _context.Cuotas
                .Where(c => c.Estado == "PENDIENTE CAPITAL" 
                            && c.Tipodecuotas == "CAPITAL" // <-- FILTRO AÑADIDO
                            && c.Moneda == "U.I.")
                .SumAsync(c => c.Monto ?? 0);
        }
    }
}