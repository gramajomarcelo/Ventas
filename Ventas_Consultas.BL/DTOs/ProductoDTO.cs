namespace Ventas_Consultas.BL.DTOs
{
    public class ProductoDTO
    {
        public long IdProducto { get; set; }

        public string Nombre { get; set; } = null!;

        public string Codigo { get; set; } = null!;

        public long IdMarca { get; set; }

        public string Modelo { get; set; } = null!;

        public int CostoUnitario { get; set; }

        public virtual MarcaDTO IdMarcaNavigation { get; set; } = null!;

        public virtual ICollection<VentaDetalleDTO> VentaDetalles { get; set; } = new List<VentaDetalleDTO>();
    }
}
