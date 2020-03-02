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
    public interface IBaiHocService : IService<BaiHoc, BaiHocViewModel>
    {
        Task<ResponseEntity> AddQuestionToLessonAsync(dynamic id, CauHoiViewModel modelVm);
    }

    public class BaiHocService : ServiceBase<BaiHoc, BaiHocViewModel>, IBaiHocService
    {
        IBaiHocRepository _baiHocRepository;
        ICauHoiRepository _cauHoiRepository;
        IChuongHocRepository _chuongHocRepository;
        public BaiHocService(IBaiHocRepository baiHocRepository,
            ICauHoiRepository cauHoiRepository,
            IChuongHocRepository chuongHocRepository,
            IMapper mapper)
            : base(baiHocRepository, mapper)
        {
            _baiHocRepository = baiHocRepository;
            _cauHoiRepository = cauHoiRepository;
            _chuongHocRepository = chuongHocRepository;
        }

        public async Task<ResponseEntity> AddQuestionToLessonAsync(dynamic id, CauHoiViewModel modelVm)
        {
            try
            {
                BaiHoc baiHoc = await _baiHocRepository.GetSingleByIdAsync(id);
                if (baiHoc == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                // Thêm mới câu hỏi
                CauHoi cauHoi = _mapper.Map<CauHoi>(modelVm);
                cauHoi = await _cauHoiRepository.InsertAsync(cauHoi);
                if (cauHoi == null) // Nếu thêm mới thất bại
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.INSERT_ERROR);

                var baiHocVm = _mapper.Map<BaiHocViewModel>(baiHoc);
                baiHocVm.DanhSachCauHoi.Add(cauHoi.Id);

                // Cập nhật lại danh sách bài của chương học
                baiHoc = _mapper.Map<BaiHoc>(baiHocVm);
                if ((await _baiHocRepository.UpdateAsync(id, baiHoc)) == null)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.INSERT_ERROR);

                modelVm.Id = cauHoi.Id;
                return new ResponseEntity(StatusCodeConstants.OK, modelVm, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public override async Task<ResponseEntity> DeleteByIdAsync(List<dynamic> listId)
        {
            try
            {
                IEnumerable<BaiHoc> dsBaiHoc = await _baiHocRepository.GetMultiByIdAsync(listId);

                if (await _baiHocRepository.DeleteByIdAsync(listId) == 0)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, listId, MessageConstants.DELETE_ERROR);

                // Xóa id khỏi danh sách bài học của chương học
                foreach (BaiHoc baiHoc in dsBaiHoc)
                {
                    ChuongHoc chuongHoc = await _chuongHocRepository.GetSingleByIdAsync(baiHoc.MaChuongHoc);
                    ChuongHocViewModel chuongHocVm = _mapper.Map<ChuongHocViewModel>(chuongHoc);
                    chuongHocVm.DanhSachBaiHoc.RemoveAll(x => x == baiHoc.Id);

                    chuongHoc = _mapper.Map<ChuongHoc>(chuongHocVm);
                    await _chuongHocRepository.UpdateAsync(chuongHoc.Id, chuongHoc);
                }

                return new ResponseEntity(StatusCodeConstants.OK, listId, MessageConstants.DELETE_SUCCESS);

            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }
    }
}