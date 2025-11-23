using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Infrastructure.Persistence.Repositorios;

public class ReporteRepository : IReporteRepository
{
    private readonly ProjectContext context;
    public ReporteRepository(ProjectContext dbContext)
    {
        context = dbContext;
    }

    public async Task<(string,DataTable)> GenerateAsync(Reporte reporte)
    {
        
        DataTable reportData;
        string hexaID;
        using (var command = context.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = "REPORTDATAGENERATOR.CREATERETRIEVEREPORTDATA";
            command.CommandType = CommandType.StoredProcedure;

            // Usar bind por nombre para poder omitir parámetros con DEFAULT
            if (command is OracleCommand oc) oc.BindByName = true;

            // NTYPEREPORT IN
            var pTypeReport = new OracleParameter("NTYPEREPORT", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Value = (int)reporte.Tipo
            };
            command.Parameters.Add(pTypeReport);

            // DSTARTDATE IN
            if (reporte.FechaDesde.HasValue)
            {
                var pStartDate = new OracleParameter("DSTARTDATE", OracleDbType.Date)
                {
                    Direction = ParameterDirection.Input,
                    Value = reporte.FechaDesde.Value
                };
                command.Parameters.Add(pStartDate);
            }

            // DENDDATE IN
            if (reporte.FechaHasta.HasValue)
            {
                var pEndDate = new OracleParameter("DENDDATE", OracleDbType.Date)
                {
                    Direction = ParameterDirection.Input,
                    Value = reporte.FechaHasta.Value
                };
                command.Parameters.Add(pEndDate);
            }

            // NAFILIADOID IN
            if (reporte.AfiliadoId.HasValue)
            {
                var pAfiliadoId = new OracleParameter("NAFILIADOID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = reporte.AfiliadoId.Value
                };
                command.Parameters.Add(pAfiliadoId);
            }

            // REPORTCURSOR OUT
            var pCursor = new OracleParameter("REPORTCURSOR", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(pCursor);

            // HEXAID OUT
            var pHexaID = new OracleParameter("HEXAID", OracleDbType.NVarchar2, 8)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(pHexaID);

            var conn = command.Connection;
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();
            try
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    reportData = new DataTable();
                    reportData.Load(reader);
                }
                hexaID = pHexaID.Value.ToString() ?? string.Empty;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        return (hexaID,reportData);
    }

    public async Task<List<Reporte>> GetAllAsync()
    {
        List<Reporte> reportes = await context.Reportes.ToListAsync();
        return reportes;
    }

    public async Task<DataTable> RetrieveAsync(string hexaID)
    {
        DataTable reportData;

        using (var command = context.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = "REPORTDATAGENERATOR.CREATERETRIEVEREPORTDATA";
            command.CommandType = CommandType.StoredProcedure;

            // Usar bind por nombre para poder omitir parámetros con DEFAULT
            if (command is OracleCommand oc) oc.BindByName = true;

            // SCODE IN
            var pScode = new OracleParameter("SCODE", OracleDbType.NVarchar2)
            {
                Direction = ParameterDirection.Input,
                Value = hexaID
            };
            command.Parameters.Add(pScode);

            // REPORTCURSOR OUT
            var pCursor = new OracleParameter("REPORTCURSOR", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(pCursor);

            // HEXAID OUT
            var pHexaID = new OracleParameter("HEXAID", OracleDbType.NVarchar2, 8)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(pHexaID);


            var conn = command.Connection;
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();
            try
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    reportData = new DataTable();
                    reportData.Load(reader);
                }
            }
            finally
            {
                await conn.CloseAsync();
            }

        }

        return reportData;

    }
}
