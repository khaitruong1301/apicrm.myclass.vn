using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;
using System.Linq;
using System;

namespace SoloDevApp.Service.Services
{
    public interface ILopHocService : IService<LopHoc, LopHocViewModel>
    {
        Task<ResponseEntity> GetInfoByIdAsync(dynamic id);
    }

    public class LopHocService : ServiceBase<LopHoc, LopHocViewModel>, ILopHocService
    {
        ILopHocRepository _lopHocRepository;
        INguoiDungRepository _nguoiDungRepository;

        public LopHocService(ILopHocRepository lopHocRepository,
            INguoiDungRepository nguoiDungRepository,
            IMapper mapper)
            : base(lopHocRepository, mapper)
        {
            _lopHocRepository = lopHocRepository;
            _nguoiDungRepository = nguoiDungRepository;
        }

        public async Task<ResponseEntity> GetInfoByIdAsync(dynamic id)
        {
            HashSet<dynamic> listId = new HashSet<dynamic>();
            try
            {
                var entity = await _lopHocRepository.GetSingleByIdAsync(id);
                if (entity == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                ThongTinLopHocViewModel modelVm = _mapper.Map<ThongTinLopHocViewModel>(entity);
                foreach(dynamic item in modelVm.DanhSachGiangVien)
                {
                    listId.Add(item);
                }
                foreach (dynamic item in modelVm.DanhSachMentor)
                {
                    listId.Add(item);
                }
                foreach (dynamic item in modelVm.DanhSachHocVien)
                {
                    listId.Add(item);
                }

                var dsNguoiDung = await _nguoiDungRepository.GetMultiByIdAsync(listId.ToList());
                var dsNguoiDungVm = _mapper.Map<HashSet<NguoiDungViewModel>>(dsNguoiDung);

                modelVm.DanhSachNguoiDung = dsNguoiDungVm;
                return new ResponseEntity(StatusCodeConstants.OK, modelVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }
    }
}