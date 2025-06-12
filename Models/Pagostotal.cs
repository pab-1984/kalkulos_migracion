using System;
using System.Collections.Generic;

namespace KalkulosCore.Models;

public partial class Pagostotal
{
    public int Id { get; set; }

    public int? Idcliente { get; set; }

    public string? NombreDeudor { get; set; }

    public double? Monto { get; set; }

    public DateTime? Fecha { get; set; }
}
