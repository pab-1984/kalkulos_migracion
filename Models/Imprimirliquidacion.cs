using System;
using System.Collections.Generic;

namespace KalkulosCore.Models;

public partial class Imprimirliquidacion
{
    public int Id { get; set; }

    public string? Concepto { get; set; }

    public double? Monto { get; set; }

    public string? Fechainicio { get; set; }

    public string? Fechahasta { get; set; }

    public string? Dias { get; set; }

    public double? Mora { get; set; }

    public double? Total { get; set; }
}
