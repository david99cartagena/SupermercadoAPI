using Microsoft.AspNetCore.Mvc;
using SupermercadoAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class VentasReportController : ControllerBase
{
    private readonly VentasReportService _service;
    public VentasReportController(VentasReportService service)
    {
        _service = service;
    }

    [HttpGet("diario/{fecha}")]
    public async Task<IActionResult> ReporteDiario(DateTime fecha)
    {
        try
        {
            var resultado = await _service.ReporteDiario(fecha);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("semanal")]
    public async Task<IActionResult> ReporteSemanal(
        [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
    {
        try
        {
            var resultado = await _service.ReporteSemanal(fechaInicio, fechaFin);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("mensual/{anio}/{mes}")]
    public async Task<IActionResult> ReporteMensual(int anio, int mes)
    {
        try
        {
            var resultado = await _service.ReporteMensual(anio, mes);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("anual/{anio}")]
    public async Task<IActionResult> ReporteAnual(int anio)
    {
        try
        {
            var resultado = await _service.ReporteAnual(anio);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}

