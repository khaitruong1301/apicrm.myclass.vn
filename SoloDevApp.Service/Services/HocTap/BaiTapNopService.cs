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
        Task<ResponseEntity> GetByUserIdAsync(string userId);

        Task<ResponseEntity> GetByClassAndUserIdAsync(int classId, string userId);
        Task<ResponseEntity> GetByExerciseIdAsync(int classId, int exerciseId);
    }

    public class BaiTapNopService : ServiceBase<BaiTapNop, BaiTapNopViewModel>, IBaiTapNopService
    {
        IBaiTapNopRepository _baiTapNopRepository;
        INguoiDungRepository _nguoiDungRepository;
        public BaiTapNopService(IBaiTapNopRepository baiTapNopRepository,
             INguoiDungRepository nguoiDungRepository,
            IMapper mapper)
            : base(baiTapNopRepository, mapper)
        {
            _baiTapNopRepository = baiTapNopRepository;
            _nguoiDungRepository = nguoiDungRepository;
        }

        public async Task<ResponseEntity> GetByClassAndUserIdAsync(int classId, string userId)
        {
            try
            {
                List<KeyValuePair<string, dynamic>> columns = new List<KeyValuePair<string, dynamic>>();
                columns.Add(new KeyValuePair<string, dynamic>("MaNguoiDung", userId));
                columns.Add(new KeyValuePair<string, dynamic>("MaLopHoc", classId));

                IEnumerable<BaiTapNop> dsBaiTapNop = await _baiTapNopRepository.GetMultiByListConditionAndAsync(columns);
                List<BaiTapNopViewModel> dsBaiTapNopVm = _mapper.Map<List<BaiTapNopViewModel>>(dsBaiTapNop);

                return new ResponseEntity(StatusCodeConstants.OK, dsBaiTapNopVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> GetByExerciseIdAsync(int classId, int exerciseId)
        {
            try
            {
                List<KeyValuePair<string, dynamic>> columns = new List<KeyValuePair<string, dynamic>>();
                columns.Add(new KeyValuePair<string, dynamic>("MaBaiTap", exerciseId));
                columns.Add(new KeyValuePair<string, dynamic>("MaLopHoc", classId));

                IEnumerable<BaiTapNop> dsBaiTapNop = await _baiTapNopRepository.GetMultiByListConditionAndAsync(columns);
                List<BaiTapNopViewModel> dsBaiTapNopVm = _mapper.Map<List<BaiTapNopViewModel>>(dsBaiTapNop);

                return new ResponseEntity(StatusCodeConstants.OK, dsBaiTapNopVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> GetByUserIdAsync(string userId)
        {
            try
            {
                IEnumerable<BaiTapNop> dsBaiTapNop = await _baiTapNopRepository.GetMultiByConditionAsync("MaNguoiDung", userId);
                List<BaiTapNopViewModel> modelVm = _mapper.Map<List<BaiTapNopViewModel>>(dsBaiTapNop);
                return new ResponseEntity(StatusCodeConstants.OK, modelVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public override async Task<ResponseEntity> InsertAsync(BaiTapNopViewModel modelVm)
        {
            try
            {
                BaiTapNop entity = _mapper.Map<BaiTapNop>(modelVm);
                await _baiTapNopRepository.InsertAsync(entity);

                modelVm = _mapper.Map<BaiTapNopViewModel>(entity);
                return new ResponseEntity(StatusCodeConstants.CREATED, modelVm, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public override async Task<ResponseEntity> UpdateAsync(dynamic id, BaiTapNopViewModel modelVm)
        {
            try
            {
                List<KeyValuePair<string, dynamic>> columns = new List<KeyValuePair<string, dynamic>>();
                columns.Add(new KeyValuePair<string, dynamic>("Id", id));
                columns.Add(new KeyValuePair<string, dynamic>("MaLopHoc", modelVm.MaLopHoc));
                columns.Add(new KeyValuePair<string, dynamic>("MaBaiTap", modelVm.MaBaiTap));
                columns.Add(new KeyValuePair<string, dynamic>("MaNguoiDung", modelVm.MaNguoiDung));

                BaiTapNop entity = await _baiTapNopRepository.GetSingleByListConditionAsync(columns);
                if (entity == null)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, null, "Không tìm thấy tài nguyên!");

                entity = _mapper.Map<BaiTapNop>(modelVm);
                await _baiTapNopRepository.UpdateAsync(id, entity);

                modelVm = _mapper.Map<BaiTapNopViewModel>(entity);
                return new ResponseEntity(StatusCodeConstants.OK, modelVm, MessageConstants.UPDATE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }
    }
}