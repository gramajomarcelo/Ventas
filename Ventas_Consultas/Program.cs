using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ventas_consultas.BL.Services;
using Ventas_Consultas.BL.Data;
using Ventas_Consultas.BL.Models;
using Ventas_Consultas.BL.Repositories;
using Ventas_Consultas.BL.Repositories.Implements;
using Ventas_Consultas.BL.Services.Implements;

namespace Ventas_Consultas
{
    class Program
    {
        static async Task Main(string[] args)
        {

            Console.WriteLine("*********************************************");
            Console.WriteLine("Bienvenido al programa de consultas de ventas");
            Console.WriteLine("*********************************************\n");
            Console.WriteLine("Iniciando...");

            var host = CreateHostBuilder(args).Build();

            int diasHaciaAtras = 30;

            using (var serviceScope = host.Services.CreateScope())
            {
                Console.WriteLine("Generando el scope de servicios...");
                var services = serviceScope.ServiceProvider;
                try
                {
                    var ventaService = services.GetRequiredService<IVentaService>();

                    Console.WriteLine("Obteniendo registros de la base de datos...\n");
                    var ventas = await ventaService.GetAllWithDetails(diasHaciaAtras);

                    if (ventas == null || !ventas.Any())
                    {
                        Console.WriteLine("No se encontraron ventas.\n");
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("Registros obtenidos exitosamente.\n");
                    }

                    var calcularMostrar = new Calcular_Mostrar();

                    calcularMostrar.MostrarTotalVentas(ventas);

                    calcularMostrar.MostrarVentaMasAlta(ventas);

                    calcularMostrar.MostrarProductoConMayorMontoTotal(ventas);

                    calcularMostrar.MostrarLocalConMayorMonto(ventas);

                    calcularMostrar.MostrarMarcaConMayorMargen(ventas);

                    calcularMostrar.MostrarProductoMasVendidoPorLocal(ventas);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocurrió un error: {ex.Message}");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        })
        .ConfigureAppConfiguration((hostContext, config) =>
        {
            config.AddJsonFile("appsettings.json", optional: true);
        })
        .ConfigureServices((hostContext, services) =>
        {
            services.AddDbContext<Ventas_ConsultasContext>(options =>
                options.UseSqlServer(hostContext.Configuration.GetConnectionString("Ventas_ConsultasContext")));

            services.AddScoped<IVentaService, VentaService>();
            services.AddScoped<IVentaRepository, VentaRepository>();
        });
    }
}