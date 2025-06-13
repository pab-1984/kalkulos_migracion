using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KalkulosCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace KalkulosCore.Pages.Acreedores
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly KuotasDbContext _context;

        public DeleteModel(KuotasDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Acreedore Acreedore { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var acreedore = await _context.Acreedores.FirstOrDefaultAsync(m => m.Id == id);
            if (acreedore == null) return NotFound();
            
            Acreedore = acreedore;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var acreedore = await _context.Acreedores.FindAsync(id);
            if (acreedore != null)
            {
                _context.Acreedores.Remove(acreedore);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}