using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface ILopHocRepository : IRepository<LopHoc>
    {
    }

    public class LopHocRepository : RepositoryBase<LopHoc>, ILopHocRepository
    {
        public LopHocRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}