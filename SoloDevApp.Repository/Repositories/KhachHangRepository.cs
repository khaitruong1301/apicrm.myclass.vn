using Dapper;
using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SoloDevApp.Repository.Repositories
{
    public interface IKhachHangRepository : IRepository<KhachHang>
    {
        Task<KhachHang> GetByEmailAsync(string email);
    }

    public class KhachHangRepository : RepositoryBase<KhachHang>, IKhachHangRepository
    {
        public KhachHangRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<KhachHang> GetByEmailAsync(string email)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@email", email);
                    return await conn.QueryFirstOrDefaultAsync<KhachHang>("KHACH_HANG_GET_BY_EMAIL", parameters, null, null, CommandType.StoredProcedure);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}