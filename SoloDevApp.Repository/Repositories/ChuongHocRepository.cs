using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IChuongHocRepository : IRepository<ChuongHoc>
    {
    }

    public class ChuongHocRepository : RepositoryBase<ChuongHoc>, IChuongHocRepository
    {
        public ChuongHocRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}