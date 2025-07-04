using System;
using System.Collections.Generic;

namespace KalkulosCore.Models;

public partial class Cliente
{
    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    public int Idcliente { get; set; }

    public string? Nombres { get; set; }

    public string? Direccion { get; set; }

    public string? Telfijo { get; set; }

    public string? Telcelular { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}