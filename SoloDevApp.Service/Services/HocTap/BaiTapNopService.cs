using AutoMapper;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface IBaiTapNopService : IService<BaiTapNop, BaiTapNopViewModel>
    {
        
    }

    public class BaiTapNopService : ServiceBase<BaiTapNop, BaiTapNopViewModel>, IBaiTapNopService
    {
        IBaiTapNopRepository _baiTapNopRepository;
        public BaiTapNopService(IBaiTapNopRepository baiTapNopRepository, IMapper mapper)
            : base(baiTapNopRepository, mapper)
        {
            _baiTapNopRepository = baiTapNopRepository;
        }
    }
}