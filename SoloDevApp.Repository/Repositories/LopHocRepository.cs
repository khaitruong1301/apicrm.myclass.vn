using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SoloDevApp.Repository.Repositories
{
    public interface ILopHocRepository : IRepository<LopHoc>
    {
        Task<int> EnableAsync();
        Task<int> DisableAsync();
        Task<IEnumerable<LopHoc>> GetMultiByListIdAsync(List<dynamic> listId);
    }

    public class LopHocRepository : RepositoryBase<LopHoc>, ILopHocRepository
    {
        public LopHocRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<int> DisableAsync()
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    return await conn.ExecuteAsync("LOP_HOC_DISABLE", null, null, null, CommandType.StoredProcedure);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public async Task<int> EnableAsync()
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    return await conn.ExecuteAsync("LOP_HOC_ENABLE", null, null, null, CommandType.StoredProcedure);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<LopHoc>> GetMultiByListIdAsync(List<dynamic> listId)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@listId", JsonConvert.SerializeObject(listId));
                    return await conn.QueryAsync<LopHoc>("LOP_HOC_GET_BY_STATUS_ENABLE", parameters, null, null, CommandType.StoredProcedure);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}