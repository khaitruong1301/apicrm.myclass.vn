using Dapper;
using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SoloDevApp.Repository.Repositories
{
    public interface ILopHocRepository : IRepository<LopHoc>
    {
        Task<int> EnableAsync();
    }

    public class LopHocRepository : RepositoryBase<LopHoc>, ILopHocRepository
    {
        public LopHocRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<int> EnableAsync()
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    return await conn.ExecuteAsync("KHOA_HOC_ENABLE", null, null, null, CommandType.StoredProcedure);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}