using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KalkulosCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace KalkulosCore.Pages.Clientes
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly KuotasDbContext _context;

        public CreateModel(KuotasDbContext context)
        {
            _context = context;
        }

        // Esta propiedad enlazará los datos del formulario
        [BindProperty]
        public Cliente Cliente { get; set; } = default!;

        // Este método prepara la página cuando se carga por primera vez
        public IActionResult OnGet()
        {
            return Page();
        }

        // Este método se ejecuta cuando se presiona el botón de guardar
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Clientes.Add(Cliente);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index"); // Redirige a la lista de clientes
        }
    }
}