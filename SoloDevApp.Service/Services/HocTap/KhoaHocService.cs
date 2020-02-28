using AutoMapper;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface IKhoaHocService : IService<KhoaHoc, KhoaHocViewModel>
    {
        Task<ResponseEntity> GetAllActiveAsync(dynamic id);
        Task<ResponseEntity> GetInfoByIdAsync(dynamic id);
        Task<ResponseEntity> AddChapterToCourseAsync(dynamic id, ChuongHocViewModel modelVm);
        Task<ResponseEntity> SortingAsync(dynamic id, List<int> dsChuongHoc);
    }

    public class KhoaHocService : ServiceBase<KhoaHoc, KhoaHocViewModel>, IKhoaHocService
    {
        IKhoaHocRepository _khoaHocRepository;
        IChuongHocRepository _chuongHocRepository;
        IBaiHocRepository _baiHocRepository;
        ILoTrinhRepository _loTrinhRepository;
        public KhoaHocService(IKhoaHocRepository khoaHocRepository,
            IChuongHocRepository chuongHocRepository,
            IBaiHocRepository baiHocRepository,
            ILoTrinhRepository loTrinhRepository,
            IMapper mapper)
            : base(khoaHocRepository, mapper)
        {
            _khoaHocRepository = khoaHocRepository;
            _chuongHocRepository = chuongHocRepository;
            _baiHocRepository = baiHocRepository;
            _loTrinhRepository = loTrinhRepository;
        }

        public async Task<ResponseEntity> AddChapterToCourseAsync(dynamic id, ChuongHocViewModel modelVm)
        {
            try
            {
                KhoaHoc khoaHoc = await _khoaHocRepository.GetSingleByIdAsync(id);
                if (khoaHoc == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                // Thêm mới chương học
                ChuongHoc chuongHoc = _mapper.Map<ChuongHoc>(modelVm);
                chuongHoc = await _chuongHocRepository.InsertAsync(chuongHoc);
                if (chuongHoc == null) // Nếu thêm mới thất bại
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.INSERT_ERROR);

                var khoaHocVm = _mapper.Map<KhoaHocViewModel>(khoaHoc);
                khoaHocVm.DanhSachChuongHoc.Add(chuongHoc.Id);

                // Cập nhật lại danh sách chương của khóa học
                khoaHoc = _mapper.Map<KhoaHoc>(khoaHocVm);
                if ((await _khoaHocRepository.UpdateAsync(id, khoaHoc)) == null)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.INSERT_ERROR);

                var thongTinChuongHocVm = _mapper.Map<ThongTinChuongHocViewModel>(chuongHoc);
                return new ResponseEntity(StatusCodeConstants.OK, thongTinChuongHocVm, MessageConstants.INSERT_SUCCESS);
            }
            catch(Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }

        }

        public async Task<ResponseEntity> GetAllActiveAsync(dynamic id)
        {
            try
            {
                var entities = await _khoaHocRepository.GetAllAsync();
                var modelVm = _mapper.Map<KhoaHocViewModel>(entities);
                return new ResponseEntity(StatusCodeConstants.OK, modelVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> GetInfoByIdAsync(dynamic id)
        {
            try
            {
                KhoaHoc khoaHoc = await _khoaHocRepository.GetSingleByIdAsync(id);
                var khoaHocVm = _mapper.Map<ThongTinKhoaHocViewModel>(khoaHoc);

                var chuongHocs = (await _chuongHocRepository.GetMultiByIdAsync(khoaHocVm.DanhSachChuongHoc));
                var chuongHocVms = _mapper.Map<List<ThongTinChuongHocViewModel>>(chuongHocs);
                
                foreach (ThongTinChuongHocViewModel chuongHoc in chuongHocVms)
                {
                    // Lấy danh sách bài học thuộc chương học ( Sử dụng danh sách id bài học lưu trong chương học)
                    var baiHocs = (await _baiHocRepository.GetMultiByIdAsync(chuongHoc.DanhSachBaiHoc));
                    var baiHocVms = _mapper.Map<List<BaiHocViewModel>>(baiHocs);
                    // SẮP XẾP BÀI HỌC
                    List<BaiHocViewModel> dsBaiHoc = new List<BaiHocViewModel>();
                    chuongHoc.DanhSachBaiHoc.ForEach(maBaiHoc =>
                    {
                        dsBaiHoc.Add(baiHocVms.FirstOrDefault(x => x.Id == maBaiHoc));
                    });
                    chuongHoc.ThongTinBaiHoc = dsBaiHoc;
                }

                // SẮP XẾP CHƯƠNG HỌC
                List<ThongTinChuongHocViewModel> dsChuongHoc = new List<ThongTinChuongHocViewModel>();
                khoaHocVm.DanhSachChuongHoc.ForEach(maChuongHoc => {
                    dsChuongHoc.Add(chuongHocVms.FirstOrDefault(x => x.Id == maChuongHoc));
                });
                khoaHocVm.ThongTinChuongHoc = dsChuongHoc;
                return new ResponseEntity(StatusCodeConstants.OK, khoaHocVm);
            }
            catch(Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> SortingAsync(dynamic id, List<int> dsChuongHoc)
        {
            try
            {
                KhoaHoc khoaHoc = await _khoaHocRepository.GetSingleByIdAsync(id);
                if (khoaHoc == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                var khoaHocVm = _mapper.Map<KhoaHocViewModel>(khoaHoc);
                khoaHocVm.DanhSachChuongHoc = dsChuongHoc;

                khoaHoc = _mapper.Map<KhoaHoc>(khoaHocVm);

                await _khoaHocRepository.UpdateAsync(id, khoaHoc);
                return new ResponseEntity(StatusCodeConstants.OK, dsChuongHoc, MessageConstants.UPDATE_SUCCESS);
            }
            catch(Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public override async Task<ResponseEntity> DeleteByIdAsync(List<dynamic> listId)
        {
            try
            {
                IEnumerable<KhoaHoc> dsKhoaHoc = await _khoaHocRepository.GetMultiByIdAsync(listId);

                if (await _khoaHocRepository.DeleteByIdAsync(listId) == 0)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, listId, MessageConstants.DELETE_ERROR);

                // Xóa id khóa học khỏi danh sách khóa học của lộ trình
                foreach (KhoaHoc khoaHoc in dsKhoaHoc)
                {
                    if (khoaHoc.DanhSachLoTrinh != null)
                    {
                        List<dynamic> dsMaLoTrinh = JsonConvert.DeserializeObject<List<dynamic>>(khoaHoc.DanhSachLoTrinh);
                        IEnumerable<LoTrinh> dsLoTrinh = await _loTrinhRepository.GetMultiByIdAsync(dsMaLoTrinh);
                        List<LoTrinhViewModel> dsLoTrinhVm = _mapper.Map<List<LoTrinhViewModel>>(dsLoTrinh);
                        foreach(LoTrinhViewModel loTrinhVm in dsLoTrinhVm)
                        {
                            loTrinhVm.DanhSachKhoaHoc.Remove(khoaHoc.Id);
                            LoTrinh loTrinh = _mapper.Map<LoTrinh>(loTrinhVm);
                            await _loTrinhRepository.UpdateAsync(loTrinh.Id, loTrinh);
                        }
                    }
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