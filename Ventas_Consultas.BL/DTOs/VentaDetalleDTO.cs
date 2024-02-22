namespace Ventas_Consultas.BL.DTOs
{
    public class VentaDetalleDTO
    {
        public long IdVentaDetalle { get; set; }

        public long IdVenta { get; set; }

        public int PrecioUnitario { get; set; }

        public int Cantidad { get; set; }

        public int TotalLinea { get; set; }

        public long IdProducto { get; set; }

        public virtual ProductoDTO IdProductoNavigation { get; set; } = null!;

        public virtual VentaDTO IdVentaNavigation { get; set; } = null!;
    }
}
