using System;
using System.Collections.Generic;

namespace KalkulosCore.Models;

public partial class Pdfoperacion
{
    public int Id { get; set; }

    public int? Idcliente { get; set; }

    public string? Nombre { get; set; }

    public int? Idoperacion { get; set; }

    public byte[]? Archivo { get; set; }
}
