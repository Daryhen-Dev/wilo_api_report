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
        public async Task<IActionResult> GetReportBancoList()
        {
            var result = await _bankRepository.GetReportBancoList();
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