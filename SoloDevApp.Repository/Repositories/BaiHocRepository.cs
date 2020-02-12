using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IBaiHocRepository : IRepository<BaiHoc>
    {
    }

    public class BaiHocRepository : RepositoryBase<BaiHoc>, IBaiHocRepository
    {
        public BaiHocRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}