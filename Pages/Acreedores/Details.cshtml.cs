using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KalkulosCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace KalkulosCore.Pages.Acreedores
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly KuotasDbContext _context;

        public DetailsModel(KuotasDbContext context)
        {
            _context = context;
        }

        public Acreedore Acreedore { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var acreedore = await _context.Acreedores.FirstOrDefaultAsync(m => m.Id == id);
            if (acreedore == null) return NotFound();

            Acreedore = acreedore;
            return Page();
        }
    }
}