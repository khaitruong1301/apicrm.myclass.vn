using Dapper;
using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SoloDevApp.Repository.Repositories
{
    public interface IHocPhiRepository : IRepository<HocPhi>
    {
        Task<IEnumerable<HocPhi>> GetListDebtorToDayAsync(string date);
        Task<IEnumerable<HocPhi>> GetListDebtorAsync(string date);
    }

    public class HocPhiRepository : RepositoryBase<HocPhi>, IHocPhiRepository
    {
        public HocPhiRepository(IConfiguration configuration)
            : base(configuration)
        {
            
        }

        public async Task<IEnumerable<HocPhi>> GetListDebtorAsync(string date)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@date", date);
                    return await conn.QueryAsync<HocPhi>("HOC_PHI_GET_LIST_DEBTOR", parameters, null, null, CommandType.StoredProcedure);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<HocPhi>> GetListDebtorToDayAsync(string date)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@date", date);
                    return await conn.QueryAsync<HocPhi>("HOC_PHI_GET_LIST_DEBTOR_TODAY", parameters, null, null, CommandType.StoredProcedure);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}