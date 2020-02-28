using AutoMapper;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;

namespace SoloDevApp.Service.Services
{
    public interface ICauHoiService : IService<CauHoi, CauHoiViewModel>
    {
    }

    public class CauHoiService : ServiceBase<CauHoi, CauHoiViewModel>, ICauHoiService
    {
        public CauHoiService(ICauHoiRepository cauHoiRepository, IMapper mapper)
            : base(cauHoiRepository, mapper)
        {
        }
    }
}