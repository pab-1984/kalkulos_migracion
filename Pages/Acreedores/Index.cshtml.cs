using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KalkulosCore.Models;

namespace KalkulosCore.Pages.Acreedores
{
    public class IndexModel : PageModel
    {
        private readonly KuotasDbContext _context;

        public IndexModel(KuotasDbContext context)
        {
            _context = context;
        }

        public IList<Acreedore> Acreedores { get;set; } = default!;

        public async Task OnGetAsync()
        {
            // Esta línea busca la propiedad "Acreedores" en el DbContext
            Acreedores = await _context.Acreedores.ToListAsync();
        }
    }
}