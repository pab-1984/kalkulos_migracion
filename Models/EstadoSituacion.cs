using System;
using System.Collections.Generic;

namespace KalkulosCore.Models;

public partial class EstadoSituacion
{
    public int Id { get; set; }

    public DateOnly? Fecha { get; set; }

    public int? CodigoOperacion { get; set; }

    public string? Concepto { get; set; }

    public int? CodigoMovimiento { get; set; }

    public string? TipoDeCuotas { get; set; }

    public decimal? CashDolares { get; set; }

    public decimal? CashPesos { get; set; }

    public double? MontoDolares { get; set; }

    public double? MontoUi { get; set; }
}
