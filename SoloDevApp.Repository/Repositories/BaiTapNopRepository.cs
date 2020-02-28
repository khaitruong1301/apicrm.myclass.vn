using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IBaiTapNopRepository : IRepository<BaiTapNop>
    {
    }

    public class BaiTapNopRepository : RepositoryBase<BaiTapNop>, IBaiTapNopRepository
    {
        public BaiTapNopRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}