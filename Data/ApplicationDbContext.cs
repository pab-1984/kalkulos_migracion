using Microsoft.EntityFrameworkCore;
using KalkulosCore.Models;

namespace KalkulosCore.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Cliente> Clientes { get; set; }
    // Aquí añadiremos los DbSet para cada tabla (ej. Clientes, Prestamos)
}