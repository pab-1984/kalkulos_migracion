using System;
using System.Collections.Generic;

namespace KalkulosCore.Models;

public partial class HistorialAcceso
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public DateTime? Ingreso { get; set; }

    public DateTime? Salida { get; set; }
}
