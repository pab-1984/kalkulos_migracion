using System;
using System.Collections.Generic;

namespace KalkulosCore.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Usuario1 { get; set; }

    public string? Contraseña { get; set; }

    public string? Tipo { get; set; }

    public string? Nombre { get; set; }
}
