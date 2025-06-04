
using Report.Shared.DTOs.Bank;

namespace Report.Dapper.Repository.Bank
{
    public interface IBank
    {
         Task<IEnumerable<ReportBankListDTO>> GetReportBancoList();
    }
}