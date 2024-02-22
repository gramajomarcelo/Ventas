namespace Ventas_Consultas.BL.DTOs
{
    public class LocalDTO
    {
        public long IdLocal { get; set; }

        public string Nombre { get; set; } = null!;

        public string Direccion { get; set; } = null!;

        public virtual ICollection<VentaDTO> Venta { get; set; } = new List<VentaDTO>();
    }
}
