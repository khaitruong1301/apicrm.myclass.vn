using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface ILoTrinhRepository : IRepository<LoTrinh>
    {
    }

    public class LoTrinhRepository : RepositoryBase<LoTrinh>, ILoTrinhRepository
    {
        public LoTrinhRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}