using AutoMapper;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;

namespace SoloDevApp.Service.Services
{
    public interface ILoaiBaiHocService : IService<LoaiBaiHoc, LoaiBaiHocViewModel>
    {
    }

    public class LoaiBaiHocService : ServiceBase<LoaiBaiHoc, LoaiBaiHocViewModel>, ILoaiBaiHocService
    {
        public LoaiBaiHocService(ILoaiBaiHocRepository loaiBaiHocRepository, IMapper mapper)
            : base(loaiBaiHocRepository, mapper)
        {
        }
    }
}