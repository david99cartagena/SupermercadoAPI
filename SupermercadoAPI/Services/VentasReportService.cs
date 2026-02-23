using Microsoft.EntityFrameworkCore;
using SupermercadoAPI.Data;
using SupermercadoAPI.DTOs;

namespace SupermercadoAPI.Services
{
    public class VentasReportService
    {
        private readonly SupermercadoContext _context;
        public VentasReportService(SupermercadoContext context)
        {
            _context = context;
        }

        // Reporte Diario
        public async Task<object> ReporteDiario(DateTime fecha)
        {
            try
            {
                // Validar fecha válida
                if (fecha == DateTime.MinValue)
                    throw new Exception("Fecha inválida");

                var ventas = await _context.Ventas

                    .Where(v => v.FechaVenta.Date == fecha.Date)
                    .Select(v => new ReporteVentaDTO
                    {
                        VentaId = v.Id,
                        Cliente = v.Cliente.Nombre,
                        Usuario = v.Usuario.Nombre,
                        Fecha = v.FechaVenta,
                        Total = v.TotalVenta
                    })
                    .ToListAsync();

                // Si no hay ventas
                if (ventas.Count == 0)

                    return new
                    {
                        mensaje = "No hay ventas",
                        fecha = fecha
                    };

                var total = ventas.Sum(x => x.Total);

                return new
                {
                    Fecha = fecha,
                    TotalVentas = total,
                    CantidadVentas = ventas.Count,
                    Ventas = ventas
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Reporte Semanal
        public async Task<object> ReporteSemanal(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                if (fechaInicio == DateTime.MinValue || fechaFin == DateTime.MinValue)
                    throw new Exception("Fechas inválidas");

                if (fechaFin < fechaInicio)
                    throw new Exception("La fecha final no puede ser menor que la inicial");

                var ventas = await _context.Ventas
                    .Where(v => v.FechaVenta.Date >= fechaInicio.Date && v.FechaVenta.Date <= fechaFin.Date)
                    .Select(v => new ReporteVentaDTO
                    {
                        VentaId = v.Id,
                        Cliente = v.Cliente.Nombre,
                        Usuario = v.Usuario.Nombre,
                        Fecha = v.FechaVenta,
                        Total = v.TotalVenta
                    })
                    .ToListAsync();

                if (!ventas.Any())
                    return new { mensaje = "No hay ventas", fechaInicio, fechaFin };

                var total = ventas.Sum(x => x.Total);

                return new
                {
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    TotalVentas = total,
                    CantidadVentas = ventas.Count,
                    Ventas = ventas
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Reporte Mensual
        public async Task<object> ReporteMensual(int anio, int mes)
        {
            try
            {
                var ventas = await _context.Ventas
                    .Where(v => v.FechaVenta.Year == anio && v.FechaVenta.Month == mes)
                    .Select(v => new ReporteVentaDTO
                    {
                        VentaId = v.Id,
                        Cliente = v.Cliente.Nombre,
                        Usuario = v.Usuario.Nombre,
                        Fecha = v.FechaVenta,
                        Total = v.TotalVenta
                    })
                    .ToListAsync();

                if (!ventas.Any())
                    return new { mensaje = "No hay ventas", anio, mes };

                var total = ventas.Sum(x => x.Total);

                return new
                {
                    Año = anio,
                    Mes = mes,
                    TotalVentas = total,
                    CantidadVentas = ventas.Count,
                    Ventas = ventas
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Reporte Mensual
        public async Task<object> ReporteAnual(int anio)
        {
            try
            {
                var ventas = await _context.Ventas
                    .Where(v => v.FechaVenta.Year == anio)
                    .Select(v => new ReporteVentaDTO
                    {
                        VentaId = v.Id,
                        Cliente = v.Cliente.Nombre,
                        Usuario = v.Usuario.Nombre,
                        Fecha = v.FechaVenta,
                        Total = v.TotalVenta
                    })
                    .ToListAsync();

                if (!ventas.Any())
                    return new { mensaje = "No hay ventas", anio };

                var total = ventas.Sum(x => x.Total);
                return new
                {
                    Año = anio,
                    TotalVentas = total,
                    CantidadVentas = ventas.Count,
                    Ventas = ventas
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
