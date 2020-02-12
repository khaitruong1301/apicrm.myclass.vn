using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface INhomQuyenRepository : IRepository<NhomQuyen>
    {
    }

    public class NhomQuyenRepository : RepositoryBase<NhomQuyen>, INhomQuyenRepository
    {
        public NhomQuyenRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}