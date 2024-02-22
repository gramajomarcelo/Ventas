using Microsoft.EntityFrameworkCore;
using Ventas_consultas.BL.Services;
using Ventas_Consultas.BL.Data;
using Ventas_Consultas.BL.Models;
using Ventas_Consultas.BL.Repositories;

namespace Ventas_Consultas.BL.Services.Implements
{
    public class VentaService : GenericService<Ventum>, IVentaService
    {
        private readonly Ventas_ConsultasContext _context;

        public VentaService(IVentaRepository ventaRepository, Ventas_ConsultasContext context) : base(ventaRepository)
        {
            _context = context;
        }

        /// <summary>
        /// Función que se encarga de obtener todos los registros de la base utilizando el parámetro dias 
        /// como pivote de cuantos días hacia atrás debe obtener.
        /// NOTA: Se desestiman registros con fecha venta posterior al día de ejecución.
        /// </summary>
        /// <param name="dias"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Ventum>> GetAllWithDetails(int diasHaciaAtras)
        {
            var fechaHasta = DateTime.Today;
            var fechaDesde = fechaHasta.AddDays(-diasHaciaAtras);

            // Consulta para obtener las ventas con detalles dentro del rango de fechas
            var ventasConDetalles = await _context.Venta
                .Where(v => v.Fecha >= fechaDesde && v.Fecha <= fechaHasta)
                .Include(v => v.VentaDetalles)
                    .ThenInclude(d => d.IdProductoNavigation)
                        .ThenInclude(p => p.IdMarcaNavigation)
                .Include(v => v.IdLocalNavigation)
                .ToListAsync();

            return ventasConDetalles;
        }
    }
}
