using System;
using System.Collections.Generic;

namespace KalkulosCore.Models;

public partial class PagosBorrado
{
    public int Id { get; set; }

    public int? Numero { get; set; }

    public int? Idcliente { get; set; }

    public int? Idcuota { get; set; }

    public float? Monto { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Tipocuota { get; set; }

    public string? Tipo { get; set; }

    public int? Operacion { get; set; }
}
