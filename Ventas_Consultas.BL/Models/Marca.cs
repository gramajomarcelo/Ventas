﻿namespace Ventas_Consultas.BL.Models;

public partial class Marca
{
    public long IdMarca { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
