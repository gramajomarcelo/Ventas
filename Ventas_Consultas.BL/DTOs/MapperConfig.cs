using AutoMapper;
using Ventas_Consultas.BL.Models;

namespace Ventas_Consultas.BL.DTOs
{
    public class MapperConfig
    {
        public static MapperConfiguration MapperConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Ventum, VentaDTO>();
                cfg.CreateMap<VentaDTO, Ventum>();

                cfg.CreateMap<VentaDetalle, VentaDetalleDTO>();
                cfg.CreateMap<VentaDetalleDTO, VentaDetalle>();

                cfg.CreateMap<Producto, ProductoDTO>();
                cfg.CreateMap<ProductoDTO, Producto>();

                cfg.CreateMap<Local, LocalDTO>();
                cfg.CreateMap<LocalDTO, Local>();

                cfg.CreateMap<Marca, MarcaDTO>();
                cfg.CreateMap<MarcaDTO, Marca>();
            });
        }
    }
}
