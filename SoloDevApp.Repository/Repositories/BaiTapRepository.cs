using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IBaiTapRepository : IRepository<BaiTap>
    {
    }

    public class BaiTapRepository : RepositoryBase<BaiTap>, IBaiTapRepository
    {
        public BaiTapRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}