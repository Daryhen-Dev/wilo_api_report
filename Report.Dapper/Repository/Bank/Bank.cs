
using System.Data;
using Dapper;
using Report.Shared.DTOs.Bank;

namespace Report.Dapper.Repository.Bank
{
    public class Bank : IBank
    {

        private readonly IDbConnection _dbConnection;

        public Bank(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Task<IEnumerable<ReportBankListDTO>> GetReportBancoList()
        {
            var sql = @"SELECT A.IdAsientoCuenta, A.Fecha, A.Sucursal, A.Banco, A.Tipo, A.Comprobante, A.Concepto, A.Diferencia
                        FROM (
                        SELECT CAC.idAsientoCuenta IdAsientoCuenta, CAD.Fecha, L.[Local] Sucursal, CC.nombre Banco, ISNULL(
                        CASE
                        WHEN CAC.Comprobante LIKE 'TRANSFERENCIA EMITIDA N.-%'  THEN 'TRANSFERENCIA EMITIDA' 
                        WHEN CAC.Comprobante LIKE 'TRANSFERENCIA RECIBIDA N.-%' THEN 'TRANSFERENCIA RECIBIDA' END, '---') Tipo,
                        ISNULL( 
                        CASE
                        WHEN CAC.Comprobante LIKE 'TRANSFERENCIA EMITIDA N.-%'  THEN SUBSTRING(CAC.Comprobante, 27, len(CAC.Comprobante)) 
                        WHEN CAC.Comprobante LIKE 'TRANSFERENCIA RECIBIDA N.-%' THEN SUBSTRING(CAC.Comprobante, 28, len(CAC.Comprobante)) END, '---' ) Comprobante,
                        CAD.ConceptoGeneral Concepto, ABS(CAC.Debe - CAC.Haber) Diferencia
                        FROM Conta_Asiento_Cuenta CAC
                        INNER JOIN Conta_Cuenta CC ON CAC.idContaCuenta = CC.idContaCuenta
                        INNER JOIN Conta_Cuenta_Banco_Enlace CCBE ON  CC.idContaCuenta = CCBE.idContaCuenta
                        INNER JOIN Conta_Asiento_Detalle CAD ON CAC.idAsientoDetalle  = CAD.idAsientoDetalle
                        INNER JOIN [Local] L ON CAD.idLocal = L.IDLocal
                        WHERE CAC.Comprobante IS NOT NULL AND CAC.Comprobante LIKE 'TRANSFERENCIA%' AND CAD.idAsientoDetalle 
                        NOT IN ( 
                        		SELECT CBI.idAsientoDetalle
                        		FROM Conta_Banco_Iterbancaria CBI 
                        		INNER JOIN Conta_Asiento_Detalle CAD ON CBI.idAsientoDetalle = CAD.idAsientoDetalle
                        		) 
                        UNION ALL
                        SELECT CAC.idAsientoCuenta IdAsientoCuenta, CAD.Fecha, L.[Local] Sucursal, CC.nombre Banco, 
                         ISNULL(
                        CASE
                        WHEN CAC.Comprobante LIKE 'DEPOSITO RECIBIDO N.-%' THEN 'DEPOSITO RECIBIDO' END, '---') Tipo,
                        ISNULL( 
                        CASE
                        WHEN CAC.Comprobante LIKE 'DEPOSITO RECIBIDO N.-%' THEN SUBSTRING(CAC.Comprobante, 23, len(CAC.Comprobante)) END, '---' ) Comprobante,
                        CAD.ConceptoGeneral Concepto, ABS(CAC.Debe - CAC.Haber) Diferencia
                        FROM Conta_Asiento_Cuenta CAC
                        INNER JOIN Conta_Cuenta CC ON CAC.idContaCuenta = CC.idContaCuenta
                        INNER JOIN Conta_Cuenta_Banco_Enlace CCBE ON  CC.idContaCuenta = CCBE.idContaCuenta
                        INNER JOIN Conta_Asiento_Detalle CAD ON CAC.idAsientoDetalle  = CAD.idAsientoDetalle
                        INNER JOIN [Local] L ON CAD.idLocal = L.IDLocal
                        WHERE CAC.Comprobante IS NOT NULL AND CAC.Comprobante LIKE 'DEPOSITO%' AND CAD.idAsientoDetalle 
                        NOT IN ( 
                        		SELECT CBI.idAsientoDetalle
                        		FROM Conta_Banco_Iterbancaria CBI 
                        		INNER JOIN Conta_Asiento_Detalle CAD ON CBI.idAsientoDetalle = CAD.idAsientoDetalle
                        		) 
                        ) A
                        ORDER BY A.Fecha ASC
                        ";
            return _dbConnection.QueryAsync<ReportBankListDTO>(sql);
        }
    }
}