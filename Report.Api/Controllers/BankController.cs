using System.Data;
using Microsoft.AspNetCore.Mvc;
using Report.Dapper.Repository.Bank;

namespace Report.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IBank _bankRepository;
        private readonly IDbConnection _db;
        public BankController(IBank bankRepository, IDbConnection db)
        {
            _bankRepository = bankRepository;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetReportBancoList([FromQuery] DateTime Fecha1, [FromQuery] DateTime Fecha2)
        {
            var result = await _bankRepository.GetReportBancoList(Fecha1, Fecha2);
            return Ok(result);
        }

        [HttpGet("AllWithImage")]
        public async Task<IActionResult> GetReportBancoAllImageList([FromQuery] DateTime Fecha1, [FromQuery] DateTime Fecha2)
        { 
            var result = await _bankRepository.GetReportBancoAllImageList(Fecha1, Fecha2);
            return Ok(result);
        }
    [HttpGet("TestConnection")]
    public IActionResult Get()
    {
        try
        {
            _db.Open();
            _db.Close();
            return Ok("Conexión exitosa a la base de datos.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error de conexión: {ex.Message}");
        }
    }
    }
}