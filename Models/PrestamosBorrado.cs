using System;
using System.Collections.Generic;

namespace KalkulosCore.Models;

public partial class PrestamosBorrado
{
    public int Operacion { get; set; }

    public DateTime? Fecha { get; set; }

    public int? Idcliente { get; set; }

    public int? Idacreedor { get; set; }

    public string? Tipooperacion { get; set; }

    public string? Tipogarantia { get; set; }

    public string? Hipotecante { get; set; }

    public string? Padronhipotecario { get; set; }

    public string? Dadorenprenda { get; set; }

    public string? Vehiculoprendado { get; set; }

    public float? Capitalinicial { get; set; }

    public string? Moneda { get; set; }

    public float? Cuotas { get; set; }

    public string? Tipodecuotas { get; set; }

    public float? Tea { get; set; }

    public float? Tem { get; set; }

    public float? Tmora { get; set; }

    public string? Primervencimiento { get; set; }

    public string? Lugardepago { get; set; }

    public string? Carpeta { get; set; }

    public string? Observaciones { get; set; }

    public float? Capital { get; set; }

    public string? Comentarios { get; set; }

    public string? Tipodevencimiento { get; set; }

    public string? Vencimientofinal { get; set; }
}
