using System;
using System.Collections.Generic;

namespace KalkulosCore.Models;

public partial class CuotasBorrada
{
    public int? Id { get; set; }

    public int? Idcliente { get; set; }

    public float? Capital { get; set; }

    public int? Operacion { get; set; }

    public int? Numero { get; set; }

    public int? Monto { get; set; }

    public int? Pago { get; set; }

    public DateTime? Vence { get; set; }

    public string? Moneda { get; set; }

    public string? Tipooperacion { get; set; }

    public string? Tipodecuotas { get; set; }

    public string? Estado { get; set; }

    public int? Numerodepago { get; set; }
}
