using Ventas_Consultas.BL.Data;
using Ventas_Consultas.BL.Models;

namespace Ventas_Consultas.BL.Repositories.Implements
{
    public class VentaRepository : GenericRepository<Ventum>, IVentaRepository 
    {
        public VentaRepository(Ventas_ConsultasContext ventas_ConsultasContext) : base (ventas_ConsultasContext)
        {

        }
    }
}
