using System;
using System.Collections.Generic;

namespace KalkulosCore.Models;

public partial class Solicitude
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? Monto { get; set; }

    public string? Garantia { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Estado { get; set; }
}
