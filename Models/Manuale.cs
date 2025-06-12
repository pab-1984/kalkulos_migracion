using System;
using System.Collections.Generic;

namespace KalkulosCore.Models;

public partial class Manuale
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public byte[]? Archivo { get; set; }
}
