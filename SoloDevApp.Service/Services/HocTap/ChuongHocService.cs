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
    public interface IChuongHocService : IService<ChuongHoc, ChuongHocViewModel>
    {
        Task<ResponseEntity> AddLessonToChapterAsync(dynamic id, BaiHocViewModel modelVm);
        Task<ResponseEntity> SortingAsync(dynamic id, List<int> dsBaiHoc);
    }

    public class ChuongHocService : ServiceBase<ChuongHoc, ChuongHocViewModel>, IChuongHocService
    {
        IChuongHocRepository _chuongHocRepository;
        IBaiHocRepository _baiHocRepository;
        public ChuongHocService(IChuongHocRepository chuongHocRepository,
            IBaiHocRepository baiHocRepository,
            IMapper mapper)
            : base(chuongHocRepository, mapper)
        {
            _chuongHocRepository = chuongHocRepository;
            _baiHocRepository = baiHocRepository;
        }

        public async Task<ResponseEntity> AddLessonToChapterAsync(dynamic id, BaiHocViewModel modelVm)
        {
            try
            {
                ChuongHoc chuongHoc = await _chuongHocRepository.GetSingleByIdAsync(id);
                if (chuongHoc == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                // Thêm mới bài học
                BaiHoc baiHoc = _mapper.Map<BaiHoc>(modelVm);
                baiHoc = await _baiHocRepository.InsertAsync(baiHoc);
                if (baiHoc == null) // Nếu thêm mới thất bại
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.INSERT_ERROR);

                var chuongHocVm = _mapper.Map<ChuongHocViewModel>(chuongHoc);
                chuongHocVm.DanhSachBaiHoc.Add(baiHoc.Id);

                // Cập nhật lại danh sách bài của chương học
                chuongHoc = _mapper.Map<ChuongHoc>(chuongHocVm);
                if ((await _chuongHocRepository.UpdateAsync(id, chuongHoc)) == null)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.INSERT_ERROR);

                modelVm.Id = baiHoc.Id;
                return new ResponseEntity(StatusCodeConstants.OK, modelVm, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> SortingAsync(dynamic id, List<int> dsBaiHoc)
        {
            try
            {
                ChuongHoc chuongHoc = await _chuongHocRepository.GetSingleByIdAsync(id);
                if (chuongHoc == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                var chuongHocVm = _mapper.Map<ChuongHocViewModel>(chuongHoc);
                chuongHocVm.DanhSachBaiHoc = dsBaiHoc;

                chuongHoc = _mapper.Map<ChuongHoc>(chuongHocVm);

                await _chuongHocRepository.UpdateAsync(id, chuongHoc);
                return new ResponseEntity(StatusCodeConstants.OK, dsBaiHoc, MessageConstants.UPDATE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }
    }
}