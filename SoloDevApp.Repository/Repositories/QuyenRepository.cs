using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IQuyenRepository : IRepository<Quyen>
    {
    }

    public class QuyenRepository : RepositoryBase<Quyen>, IQuyenRepository
    {
        public QuyenRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}