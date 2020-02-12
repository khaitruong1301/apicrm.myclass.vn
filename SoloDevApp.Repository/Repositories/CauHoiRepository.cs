using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface ICauHoiRepository : IRepository<CauHoi>
    {
    }

    public class CauHoiRepository : RepositoryBase<CauHoi>, ICauHoiRepository
    {
        public CauHoiRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}