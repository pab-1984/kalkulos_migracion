using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KalkulosCore.Models;

namespace KalkulosCore.Pages.Acreedores
{
    public class CreateModel : PageModel
    {
        private readonly KuotasDbContext _context;

        public CreateModel(KuotasDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Acreedore Acreedore { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Acreedores.Add(Acreedore);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}