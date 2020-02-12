using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface ILoaiBaiHocRepository : IRepository<LoaiBaiHoc>
    {
    }

    public class LoaiBaiHocRepository : RepositoryBase<LoaiBaiHoc>, ILoaiBaiHocRepository
    {
        public LoaiBaiHocRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}