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
    public interface INguoiDungRepository : IRepository<NguoiDung>
    {
        Task<NguoiDung> GetByEmailAsync(string email);

        Task<NguoiDung> checkInfo(string email, string facebookEmail);
        Task<NguoiDung> checkEmailAndPhone(string email, string phone);
    }

    public class NguoiDungRepository : RepositoryBase<NguoiDung>, INguoiDungRepository
    {
        public NguoiDungRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<NguoiDung> checkEmailAndPhone(string email, string phone)
        {
            string query = $"SELECT * FROM {_table} WHERE Email = '{email}' AND SoDT = '{phone}' AND DaXoa = 0";

            using (var conn = CreateConnection())
            {
                try
                {
                    return await conn.QueryFirstOrDefaultAsync<NguoiDung>(query, null, null, null, CommandType.Text);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<NguoiDung> checkInfo(string email, string facebookEmail)
        {
            string query = $"SELECT * FROM {_table} WHERE Email = '{email}' AND FacebookEmail = '{facebookEmail}' AND DaXoa = 0";

            using (var conn = CreateConnection())
            {
                try
                {
                    return await conn.QueryFirstOrDefaultAsync<NguoiDung>(query, null, null, null, CommandType.Text);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<NguoiDung> GetByEmailAsync(string email)
        {
            List<KeyValuePair<string, dynamic>> columns = new List<KeyValuePair<string, dynamic>>();
            columns.Add(new KeyValuePair<string, dynamic>("Email", email));

            try
            {
                using (var conn = CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@tableName", _table);
                    parameters.Add("@listColumn", JsonConvert.SerializeObject(columns));
                    return await conn.QueryFirstOrDefaultAsync<NguoiDung>("GET_SINGLE_DATA", parameters, null, null, CommandType.StoredProcedure);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}