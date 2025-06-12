using System;
using System.Collections.Generic;

namespace KalkulosCore.Models;

public partial class AuditoriaPrestamo
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int Operacion { get; set; }

    public DateTime? Modificacion { get; set; }

    public string Accion { get; set; } = null!;
}
