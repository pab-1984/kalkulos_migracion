using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KalkulosCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace KalkulosCore.Pages.Prestamos
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly KuotasDbContext _context;

        public CreateModel(KuotasDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Prestamo Prestamo { get; set; } = default!;

        // Propiedades para las listas desplegables existentes
        public SelectList? ClientesSelectList { get; set; }
        public SelectList? AcreedoresSelectList { get; set; }

        // --- INICIO DE CÓDIGO AÑADIDO ---
        // 1. Nueva propiedad para la lista de monedas
        public List<SelectListItem> MonedasOptions { get; set; }
        // --- FIN DE CÓDIGO AÑADIDO ---

        public async Task<IActionResult> OnGetAsync()
        {
            // Cargamos Clientes y Acreedores como antes
            ClientesSelectList = new SelectList(await _context.Clientes.OrderBy(c => c.Nombres).ToListAsync(), "Idcliente", "Nombres");
            AcreedoresSelectList = new SelectList(await _context.Acreedores.OrderBy(a => a.Nombres).ToListAsync(), "Id", "Nombres");

            // --- INICIO DE CÓDIGO AÑADIDO ---
            // 2. Creamos la lista de opciones de moneda
            MonedasOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "$", Text = "Pesos ($)" },
                new SelectListItem { Value = "U$S", Text = "Dólares (U$S)" },
                new SelectListItem { Value = "U.I.", Text = "Unidades Indexadas (U.I.)" }
            };
            // --- FIN DE CÓDIGO AÑADIDO ---
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // La lógica del POST no necesita cambios, ya que recibirá el valor 
            // seleccionado ("$", "U$S", etc.) en la propiedad Prestamo.Moneda
            
            if (!ModelState.IsValid)
            {
                // Si hay un error, debemos recargar las listas antes de mostrar la página
                await OnGetAsync();
                return Page();
            }

            Prestamo.Fecha ??= DateTime.Now;
            _context.Prestamos.Add(Prestamo);

            var cuotasGeneradas = new List<Cuota>();
            if (Prestamo.Capitalinicial > 0 && Prestamo.Cuotas > 0 && Prestamo.Tea > 0)
            {
                double capital = Prestamo.Capitalinicial.Value;
                double tea = Prestamo.Tea.Value / 100.0;
                int numeroDeCuotas = (int)Prestamo.Cuotas.Value;
                double tem = Math.Pow(1 + tea, 1.0 / 12.0) - 1;
                
                double montoCuota = (tem > 0)
                    ? capital * (tem * Math.Pow(1 + tem, numeroDeCuotas)) / (Math.Pow(1 + tem, numeroDeCuotas) - 1)
                    : capital / numeroDeCuotas;

                if (!DateTime.TryParse(Prestamo.Primervencimiento, out var fechaPrimerVencimiento))
                {
                    fechaPrimerVencimiento = DateTime.Now.AddMonths(1);
                }

                for (int i = 1; i <= numeroDeCuotas; i++)
                {
                    var nuevaCuota = new Cuota
                    {
                        Idcliente = Prestamo.Idcliente,
                        Capital = capital,
                        OperacionNavigation = Prestamo,
                        Numero = i,
                        Monto = Math.Round(montoCuota, 2),
                        Pago = 0,
                        Vence = fechaPrimerVencimiento.AddMonths(i - 1),
                        Moneda = Prestamo.Moneda,
                        Tipooperacion = Prestamo.Tipooperacion,
                        Tipodecuotas = Prestamo.Tipodecuotas,
                        Estado = "PENDIENTE",
                        Numerodepago = 0
                    };
                    Prestamo.Cuota.Add(nuevaCuota);
                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToPage("./Details", new { id = Prestamo.Operacion });
        }
    }
}