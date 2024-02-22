using Ventas_Consultas.BL.Models;

namespace Ventas_consultas.BL.Services
{
    public interface IVentaService : IGenericService<Ventum>
    {
        Task<IEnumerable<Ventum>> GetAllWithDetails(int diasHaciaAtras);
    }
}
