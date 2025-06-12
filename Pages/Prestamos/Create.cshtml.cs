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
        public SelectList? ClientesSelectList { get; set; }
        public SelectList? AcreedoresSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ClientesSelectList = new SelectList(await _context.Clientes.OrderBy(c => c.Nombres).ToListAsync(), "Idcliente", "Nombres");
            AcreedoresSelectList = new SelectList(await _context.Acreedores.OrderBy(a => a.Nombres).ToListAsync(), "Id", "Nombres");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // --- INICIO DE CÓDIGO DE DIAGNÓSTICO ---
            Console.WriteLine("\n>>> [DIAGNÓSTICO] Entrando a OnPostAsync para guardar el préstamo.");
            try
            {
                // Verificamos si los datos del formulario son válidos
                if (!ModelState.IsValid)
                {
                    Console.WriteLine(">>> [DIAGNÓSTICO] ModelState NO es válido. Mostrando formulario de nuevo.");
                    // Si el modelo no es válido, se recarga la página mostrando los errores de validación.
                    // Recargamos las listas desplegables antes de mostrar la página.
                    await OnGetAsync(); // Llama a OnGetAsync para recargar las listas
                    return Page();
                }
                
                Console.WriteLine($">>> [DIAGNÓSTICO] Datos recibidos: Capital={Prestamo.Capitalinicial}, Cuotas={Prestamo.Cuotas}, TEA={Prestamo.Tea}");

                Prestamo.Fecha ??= DateTime.Now;
                _context.Prestamos.Add(Prestamo);
                Console.WriteLine(">>> [DIAGNÓSTICO] Objeto 'Préstamo' añadido al contexto.");


                if (Prestamo.Capitalinicial > 0 && Prestamo.Cuotas > 0 && Prestamo.Tea > 0)
                {
                    Console.WriteLine(">>> [DIAGNÓSTICO] Iniciando cálculo y generación de cuotas...");
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
                    Console.WriteLine($">>> [DIAGNÓSTICO] {Prestamo.Cuota.Count} cuotas generadas y añadidas al contexto.");
                }
                else
                {
                    Console.WriteLine(">>> [DIAGNÓSTICO] No se generaron cuotas porque Capital, Cuotas o TEA no son mayores que cero.");
                }
                
                Console.WriteLine(">>> [DIAGNÓSTICO] Intentando guardar cambios en la base de datos...");
                await _context.SaveChangesAsync();
                Console.WriteLine(">>> [DIAGNÓSTICO] ¡SaveChangesAsync completado! Los datos fueron guardados.");

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n!!!!!!!!!! ERROR CAPTURADO EN OnPostAsync !!!!!!!!!!\n");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
                throw;
            }
            // --- FIN DE CÓDIGO DE DIAGNÓSTICO ---
        }
    }
}