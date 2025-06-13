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

        // Propiedad para mostrar los detalles del préstamo seleccionado
        public Prestamo Prestamo { get; set; } = default!;

        // Propiedad para el formulario de pago
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
            // Buscamos el préstamo y cargamos los datos del cliente y sus cuotas pendientes
            Prestamo = await _context.Prestamos
                .Include(p => p.IdclienteNavigation)
                .Include(p => p.Cuota.Where(c => c.Estado == "PENDIENTE" || c.Estado == "PENDIENTE CAPITAL").OrderBy(c => c.Vence))
                .FirstOrDefaultAsync(p => p.Operacion == id);

            if (Prestamo == null)
            {
                return NotFound();
            }

            // Inicializamos el Input para el formulario
            Input = new InputModel { OperacionId = id };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Si hay un error, volvemos a cargar la página
                return await OnGetAsync(Input.OperacionId);
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.Cuota.Where(c => c.Estado == "PENDIENTE" || c.Estado == "PENDIENTE CAPITAL").OrderBy(c => c.Vence))
                .FirstOrDefaultAsync(p => p.Operacion == Input.OperacionId);

            if (prestamo == null)
            {
                return NotFound();
            }

            double montoRestanteDelPago = Input.MontoPago;

            // Aplicamos el pago a las cuotas pendientes, empezando por la más antigua
            foreach (var cuota in prestamo.Cuota)
            {
                if (montoRestanteDelPago <= 0) break; // Si ya no queda monto por aplicar, salimos del bucle

                double montoNecesarioParaSaldar = (cuota.Monto ?? 0) - (cuota.Pago ?? 0);
                double montoAAplicar = Math.Min(montoRestanteDelPago, montoNecesarioParaSaldar);

                cuota.Pago = (cuota.Pago ?? 0) + montoAAplicar;

                if (cuota.Pago >= cuota.Monto)
                {
                    cuota.Estado = "PAGO";
                }

                // Creamos un registro del pago
                var nuevoPago = new Pago
                {
                    Numero = 0, // Debemos definir cómo se genera este número
                    Idcliente = prestamo.Idcliente,
                    Idcuota = cuota.Id,
                    Monto = montoAAplicar,
                    Fecha = Input.FechaPago,
                    Tipocuota = cuota.Tipodecuotas,
                    Operacion = prestamo.Operacion,
                    Tipo = "PAGO" // O el tipo que corresponda
                };
                _context.Pagos.Add(nuevoPago);

                montoRestanteDelPago -= montoAAplicar;
            }

            // Guardamos todos los cambios (actualización de cuotas y nuevos pagos)
            await _context.SaveChangesAsync();

            // Redirigimos de nuevo a la misma página para ver el estado actualizado
            return RedirectToPage(new { id = Input.OperacionId });
        }
    }
}