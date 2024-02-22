namespace Ventas_Consultas.BL.DTOs
{
    public class VentaDTO
    {
        public long IdVenta { get; set; }

        public int Total { get; set; }

        public DateTime Fecha { get; set; }

        public long IdLocal { get; set; }

        public virtual LocalDTO IdLocalNavigation { get; set; } = null!;

        public virtual ICollection<VentaDetalleDTO> VentaDetalles { get; set; } = new List<VentaDetalleDTO>();
    }
}
