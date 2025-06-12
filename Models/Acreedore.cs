using System;
using System.Collections.Generic;

namespace KalkulosCore.Models;

public partial class Acreedore
{

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();

    public int Id { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public int? Documento { get; set; }

    public string? Direccion { get; set; }

    public string? Telfijo { get; set; }

    public string? Telcelular { get; set; }
}
