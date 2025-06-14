using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KalkulosCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace KalkulosCore.Pages.Pagos
{
    [Authorize]
    public class RegistrarModel : PageModel
    {
        private readonly KuotasDbContext _context;

        public RegistrarModel(KuotasDbContext context)
        {
            _context = context;
        }

        public Prestamo Prestamo { get; set; } = default!;
        
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public int OperacionId { get; set; }
            public double MontoPago { get; set; }
            public DateTime FechaPago { get; set; } = DateTime.Now;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Prestamo = await _context.Prestamos
                .Include(p => p.IdclienteNavigation)
                .Include(p => p.Cuota.Where(c => c.Estado == "PENDIENTE" || c.Estado == "PENDIENTE CAPITAL").OrderBy(c => c.Vence))
                .FirstOrDefaultAsync(p => p.Operacion == id);

            if (Prestamo == null)
            {
                return NotFound();
            }

            Input = new InputModel { OperacionId = id };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Input.MontoPago <= 0)
            {
                // Si el modelo no es válido o el monto es cero/negativo, recargamos la página.
                return await OnGetAsync(Input.OperacionId);
            }

            // 1. Buscamos solo las cuotas pendientes del préstamo, ordenadas por vencimiento.
            var cuotasPendientes = await _context.Cuotas
                .Where(c => c.Operacion == Input.OperacionId && (c.Estado == "PENDIENTE" || c.Estado == "PENDIENTE CAPITAL"))
                .OrderBy(c => c.Vence)
                .ToListAsync();
            
            if (!cuotasPendientes.Any())
            {
                // No hay cuotas pendientes para pagar.
                return RedirectToPage(new { id = Input.OperacionId });
            }

            double montoRestanteDelPago = Input.MontoPago;

            // 2. Iteramos sobre las cuotas pendientes para aplicar el pago.
            foreach (var cuota in cuotasPendientes)
            {
                if (montoRestanteDelPago <= 0) break; // Salimos si ya no queda monto por aplicar.

                double saldoDeLaCuota = (cuota.Monto ?? 0) - (cuota.Pago ?? 0);
                double montoAAplicar = Math.Min(montoRestanteDelPago, saldoDeLaCuota);

                cuota.Pago = (cuota.Pago ?? 0) + montoAAplicar;
                
                // 3. Verificamos si la cuota fue saldada para cambiar su estado.
                if (cuota.Pago >= cuota.Monto)
                {
                    cuota.Estado = "PAGO";
                }

                // 4. Creamos un registro de la transacción en la tabla PAGOS.
                var nuevoPago = new Pago
                {
                    Idcliente = cuota.Idcliente,
                    Idcuota = cuota.Id,
                    Monto = montoAAplicar,
                    Fecha = Input.FechaPago,
                    Tipocuota = cuota.Tipodecuotas,
                    Operacion = cuota.Operacion,
                    Tipo = "PAGO"
                };
                _context.Pagos.Add(nuevoPago);

                montoRestanteDelPago -= montoAAplicar;
            }
            
            // 5. Guardamos todos los cambios en la base de datos.
            await _context.SaveChangesAsync();

            // Redirigimos de nuevo a la misma página para ver el estado actualizado.
            return RedirectToPage(new { id = Input.OperacionId });
        }
    }
}