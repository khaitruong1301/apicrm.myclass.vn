using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IChuyenLopRepository : IRepository<ChuyenLop>
    {
    }

    public class ChuyenLopRepository : RepositoryBase<ChuyenLop>, IChuyenLopRepository
    {
        public ChuyenLopRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}