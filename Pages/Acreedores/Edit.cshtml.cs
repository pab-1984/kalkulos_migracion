using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KalkulosCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace KalkulosCore.Pages.Acreedores
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly KuotasDbContext _context;

        public EditModel(KuotasDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Acreedore Acreedore { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acreedore = await _context.Acreedores.FirstOrDefaultAsync(m => m.Id == id);
            if (acreedore == null)
            {
                return NotFound();
            }
            Acreedore = acreedore;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Acreedore).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Manejar concurrencia si es necesario
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}