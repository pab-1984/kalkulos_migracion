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

        // 1. Propiedad para recibir el archivo desde el formulario
        [BindProperty]
        public IFormFile? PdfFile { get; set; }
        
        // Propiedades para las listas desplegables
        public SelectList? ClientesSelectList { get; set; }
        public SelectList? AcreedoresSelectList { get; set; }
        public List<SelectListItem>? MonedasOptions { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            ClientesSelectList = new SelectList(await _context.Clientes.OrderBy(c => c.Nombres).ToListAsync(), "Idcliente", "Nombres");
            AcreedoresSelectList = new SelectList(await _context.Acreedores.OrderBy(a => a.Nombres).ToListAsync(), "Id", "Nombres");
            MonedasOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "$", Text = "Pesos ($)" },
                new SelectListItem { Value = "U$S", Text = "Dólares (U$S)" },
                new SelectListItem { Value = "U.I.", Text = "Unidades Indexadas (U.I.)" }
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // Recargamos las listas si hay un error
                return Page();
            }

            // La lógica para guardar el préstamo y generar cuotas no cambia
            Prestamo.Fecha ??= DateTime.Now;
            _context.Prestamos.Add(Prestamo);
            
            if (Prestamo.Capitalinicial > 0 && Prestamo.Cuotas > 0 && Prestamo.Tea > 0)
            {
                // ... (toda la lógica de cálculo que ya teníamos)
                double capital = Prestamo.Capitalinicial.Value;
                double tea = Prestamo.Tea.Value / 100.0;
                int numeroDeCuotas = (int)Prestamo.Cuotas.Value;
                double tem = Math.Pow(1 + tea, 1.0 / 12.0) - 1;
                double montoCuota = (tem > 0) ? capital * (tem * Math.Pow(1 + tem, numeroDeCuotas)) / (Math.Pow(1 + tem, numeroDeCuotas) - 1) : capital / numeroDeCuotas;
                if (!DateTime.TryParse(Prestamo.Primervencimiento, out var fechaPrimerVencimiento)) { fechaPrimerVencimiento = DateTime.Now.AddMonths(1); }
                for (int i = 1; i <= numeroDeCuotas; i++)
                {
                    var nuevaCuota = new Cuota { Idcliente = Prestamo.Idcliente, Capital = capital, OperacionNavigation = Prestamo, Numero = i, Monto = Math.Round(montoCuota, 2), Pago = 0, Vence = fechaPrimerVencimiento.AddMonths(i - 1), Moneda = Prestamo.Moneda, Tipooperacion = Prestamo.Tipooperacion, Tipodecuotas = Prestamo.Tipodecuotas, Estado = "PENDIENTE", Numerodepago = 0 };
                    Prestamo.Cuota.Add(nuevaCuota);
                }
            }

            // 2. Lógica para procesar y guardar el PDF
            if (PdfFile != null && PdfFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await PdfFile.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                var pdfOperacion = new Pdfoperacion
                {
                    Idcliente = Prestamo.Idcliente,
                    Nombre = PdfFile.FileName,
                    IdoperacionNavigation = Prestamo,
                    Archivo = fileBytes
                };
                _context.Pdfoperacions.Add(pdfOperacion);
            }

            // Guardamos todo en una sola transacción
            await _context.SaveChangesAsync();
            return RedirectToPage("./Details", new { id = Prestamo.Operacion });
        }
    }
}
