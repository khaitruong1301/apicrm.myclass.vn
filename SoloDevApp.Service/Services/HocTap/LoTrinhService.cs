using AutoMapper;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SoloDevApp.Service.Services
{
    public interface ILoTrinhService : IService<LoTrinh, LoTrinhViewModel>
    {
        Task<ResponseEntity> UpdateCourseFromSeriesAsync(dynamic id, HashSet<dynamic> listId);
        Task<ResponseEntity> SortingAsync(dynamic id, List<dynamic> listId);
    }

    public class LoTrinhService : ServiceBase<LoTrinh, LoTrinhViewModel>, ILoTrinhService
    {
        ILoTrinhRepository _loTrinhRepository;
        IKhoaHocRepository _khoaHocRepository;
        public LoTrinhService(ILoTrinhRepository loTrinhRepository, 
            IKhoaHocRepository khoaHocRepository,
            IMapper mapper)
            : base(loTrinhRepository, mapper)
        {
            _loTrinhRepository = loTrinhRepository;
            _khoaHocRepository = khoaHocRepository;
        }

        public async Task<ResponseEntity> UpdateCourseFromSeriesAsync(dynamic id, HashSet<dynamic> listId)
        {
            try
            {
                LoTrinh loTrinh = await _loTrinhRepository.GetSingleByIdAsync(id);
                if(loTrinh == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                var loTrinhVm = _mapper.Map<LoTrinhViewModel>(loTrinh);
                loTrinhVm.DanhSachKhoaHoc = listId.ToList();

                // Cập nhật lại danh sách khóa học của lộ trình
                loTrinh = _mapper.Map<LoTrinh>(loTrinhVm);
                loTrinh = await _loTrinhRepository.UpdateAsync(id, loTrinh);

                // Lấy ra danh sách thông tin các khóa học thuộc lộ trình sau khi cập nhật
                var khoaHocs = (await _khoaHocRepository.GetMultiByIdAsync(listId.ToList()));
                // Cập nhật lại danh sách lộ trình của mỗi khóa học
                foreach(KhoaHoc item in khoaHocs)
                {
                    HashSet<dynamic> dsMaLoTrinh = new HashSet<dynamic>();
                    if(item.DanhSachLoTrinh != null)
                    {
                        dsMaLoTrinh = JsonConvert.DeserializeObject<HashSet<dynamic>>(item.DanhSachLoTrinh);
                    }
                    dsMaLoTrinh.Add(id);
                    item.DanhSachLoTrinh = JsonConvert.SerializeObject(dsMaLoTrinh);
                    await _khoaHocRepository.UpdateAsync(item.Id, item);
                }

                // Convert về đối tượng ThongTinLoTrinhViewModel
                var modelVm = _mapper.Map<ThongTinLoTrinhViewModel>(loTrinh);
                modelVm.ThongTinKhoaHoc = _mapper.Map<List<KhoaHocViewModel>>(khoaHocs);

                return new ResponseEntity(StatusCodeConstants.OK, modelVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public override async Task<ResponseEntity> GetSingleByIdAsync(dynamic id)
        {
            try
            {
                LoTrinh loTrinh = await _loTrinhRepository.GetSingleByIdAsync(id);
                var loTrinhVm = _mapper.Map<ThongTinLoTrinhViewModel>(loTrinh);

                var khoaHocs = (await _khoaHocRepository.GetMultiByIdAsync(loTrinhVm.DanhSachKhoaHoc));
                var khoaHocVms = _mapper.Map<List<KhoaHocViewModel>>(khoaHocs);

                loTrinhVm.ThongTinKhoaHoc = khoaHocVms;
                return new ResponseEntity(StatusCodeConstants.OK, loTrinhVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> SortingAsync(dynamic id, List<dynamic> listId)
        {
            try
            {
                LoTrinh loTrinh = await _loTrinhRepository.GetSingleByIdAsync(id);
                if (loTrinh == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                var loTrinhVm = _mapper.Map<LoTrinhViewModel>(loTrinh);
                loTrinhVm.DanhSachKhoaHoc = listId;

                loTrinh = _mapper.Map<LoTrinh>(loTrinhVm);

                await _loTrinhRepository.UpdateAsync(id, loTrinh);
                return new ResponseEntity(StatusCodeConstants.OK, listId, MessageConstants.UPDATE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }
    }
}