

namespace Report.Shared.DTOs.Bank
{
    public class ReportBankListDTO
    {
        public int IdAsientoCuenta { get; set; }
        public DateTime Fecha { get; set; }
        public string? Sucursal { get; set; }
        public string? Banco { get; set; }
        public string? Tipo { get; set; }
        public string? Comprobante { get; set; }
        public string? Concepto { get; set; }
        public decimal Diferencia { get; set; }

    }
}