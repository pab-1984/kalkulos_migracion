using System;
using System.Collections.Generic;

namespace KalkulosCore.Models;

public partial class Prestamo
{

    public virtual ICollection<Cuota> Cuota { get; set; } = new List<Cuota>();

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

    public double? Capitalinicial { get; set; }

    public string? Moneda { get; set; }

    public double? Cuotas { get; set; }

    public string? Tipodecuotas { get; set; }

    public double? Tea { get; set; }

    public double? Tem { get; set; }

    public double? Tmora { get; set; }

    public string? Primervencimiento { get; set; }

    public string? Lugardepago { get; set; }

    public string? Carpeta { get; set; }

    public string? Observaciones { get; set; }

    public double? Capital { get; set; }

    public string? Comentarios { get; set; }

    public string? Tipodevencimiento { get; set; }

    public string? Vencimientofinal { get; set; }

    public DateTime SysStartTime { get; set; }

    public DateTime SysEndTime { get; set; }

    public virtual Acreedore? IdacreedorNavigation { get; set; }
    
    public virtual Cliente? IdclienteNavigation { get; set; }
}
