namespace Ventas_Consultas.BL.DTOs
{
    public class MarcaDTO
    {
        public long IdMarca { get; set; }

        public string Nombre { get; set; } = null!;

        public virtual ICollection<ProductoDTO> Productos { get; set; } = new List<ProductoDTO>();
    }
}
