using Ventas_Consultas.BL.Models;

namespace Ventas_Consultas
{
    public class Calcular_Mostrar
    {
        public void MostrarTotalVentas(IEnumerable<Ventum> ventas)
        {
            decimal totalVentas = ventas.Sum(v => v.Total);
            int cantidadTotalVentas = ventas.SelectMany(v => v.VentaDetalles)
                .Sum(d => d.Cantidad);

            Console.WriteLine(string.Format("1 - El total de ventas de los últimos 30 días (monto total y cantidad total de ventas)." +
            "\nMonto total {0}. Cantidad total de ventas: {1}.\n", totalVentas, cantidadTotalVentas));
        }

        public void MostrarVentaMasAlta(IEnumerable<Ventum> ventas)
        {
            var ventaMasAlta = ventas.OrderByDescending(v => v.Total).FirstOrDefault();

            if (ventaMasAlta != null)
            {
                Console.WriteLine(string.Format("2 - El día y hora en que se realizó la venta con el monto más alto (y cuál es aquel monto)." +
                "\nEl día {0} se realizó la venta con el monto más alto, que fue por {1}.\n", ventaMasAlta.Fecha, ventaMasAlta.Total));
            }
            else
            {
                Console.WriteLine("2 - El día y hora en que se realizó la venta con el monto más alto (y cuál es aquel monto)." +
                "\nNo se encontraron ventas.\n");
            }
        }

        public void MostrarProductoConMayorMontoTotal(IEnumerable<Ventum> ventas)
        {
            var productoConMayorMontoTotal = ventas
                .SelectMany(v => v.VentaDetalles)
                .GroupBy(vd => vd.IdProductoNavigation)
                .Select(g => new
                {
                    Producto = g.Key,
                    MontoTotalVentas = g.Sum(vd => vd.TotalLinea)
                })
                .OrderByDescending(x => x.MontoTotalVentas)
                .FirstOrDefault();

            if (productoConMayorMontoTotal != null)
            {
                var producto = productoConMayorMontoTotal.Producto;
                var montoTotalVentas = productoConMayorMontoTotal.MontoTotalVentas;
                Console.WriteLine(string.Format("3 - Indicar cuál es el producto con mayor monto total de ventas." +
                "\nMonto total {0}. Cantidad total de ventas: {1}.\n", producto.Nombre, montoTotalVentas));
            }
            else
            {
                Console.WriteLine("3 - Indicar cuál es el producto con mayor monto total de ventas." +
                "\nNo se encontraron ventas.\n");
            }
        }

        public void MostrarLocalConMayorMonto(IEnumerable<Ventum> ventas)
        {
            var localConMayorMonto = ventas
                .GroupBy(v => v.IdLocalNavigation)
                .Select(g => new { Local = g.Key, MontoTotalVentas = g.Sum(v => v.Total) })
                .OrderByDescending(x => x.MontoTotalVentas)
                .FirstOrDefault();

            if (localConMayorMonto != null)
            {
                Console.WriteLine(string.Format("4 - Indicar el local con mayor monto de ventas." +
                "\nLocal: {0}. Cantidad total de ventas: {1}.\n", localConMayorMonto.Local.Nombre, localConMayorMonto.MontoTotalVentas));
            }
            else
            {
                Console.WriteLine("4 - Indicar el local con mayor monto de ventas." +
                "\nNo se encontraron ventas.\n");
            }
        }

        public void MostrarMarcaConMayorMargen(IEnumerable<Ventum> ventas)
        {
            var marcaConMayorMargen = ventas
                .SelectMany(venta => venta.VentaDetalles)
                .GroupBy(detalle => detalle.IdProductoNavigation.IdMarcaNavigation.Nombre)
                .Select(grupo => new
                {
                    Marca = grupo.Key,
                    MargenTotal = grupo.Sum(detalle =>
                    {
                        var costoUnitario = detalle.IdProductoNavigation.CostoUnitario;
                        var precioUnitario = detalle.PrecioUnitario;
                        var cantidad = detalle.Cantidad;
                        return (precioUnitario - costoUnitario) * cantidad;
                    })
                })
                .OrderByDescending(marca => marca.MargenTotal)
                .FirstOrDefault();

            if (marcaConMayorMargen != null)
            {
                Console.WriteLine(string.Format("5 - ¿Cuál es la marca con mayor margen de ganancias?" +
                    "\nMarca: {0}. Margen de ganancias: {1}.\n", marcaConMayorMargen.Marca, marcaConMayorMargen.MargenTotal));
            }
            else
            {
                Console.WriteLine("5 - ¿Cuál es la marca con mayor margen de ganancias?" +
                "\nNo se encontraron ventas.\n");
            }
        }

        public void MostrarProductoMasVendidoPorLocal(IEnumerable<Ventum> ventas)
        {
            var ventasPorLocalYProducto = ventas
                .SelectMany(venta => venta.VentaDetalles)
                .GroupBy(detalle => new { Local = detalle.IdVentaNavigation.IdLocalNavigation.Nombre, Producto = detalle.IdProductoNavigation.Nombre })
                .Select(grupo => new
                {
                    Local = grupo.Key.Local,
                    Producto = grupo.Key.Producto,
                    CantidadTotalVendida = grupo.Sum(detalle => detalle.Cantidad)
                });

            var productoMasVendidoPorLocal = ventasPorLocalYProducto
                .GroupBy(info => info.Local)
                .Select(grupo =>
                {
                    var maxCantidad = grupo.Max(info => info.CantidadTotalVendida);
                    var productosMasVendidos = grupo.Where(info => info.CantidadTotalVendida == maxCantidad);
                    return productosMasVendidos.ToList();
                });

            Console.WriteLine("6 - ¿Cuál es el producto que más se vende en cada local?");

            foreach (var productosPorLocal in productoMasVendidoPorLocal)
            {
                foreach (var _producto in productosPorLocal)
                {
                    if (_producto != null)
                    {
                        Console.WriteLine($"En el local {_producto.Local}, el producto más vendido es {_producto.Producto} " +
                            $"con una cantidad total vendida de {_producto.CantidadTotalVendida}");
                    }
                }
            }
        }

    }
}
