using AutoMapper;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.Utilities;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface IBaiTapService : IService<BaiTap, BaiTapViewModel>
    {
        Task<ResponseEntity> GetBySeriesIdAsync(dynamic id);
        Task<ResponseEntity> GetByClassAndUserIdAsync(int classId, string userId);
    }

    public class BaiTapService : ServiceBase<BaiTap, BaiTapViewModel>, IBaiTapService
    {
        IBaiTapRepository _baiTapRepository;
        IBaiTapNopRepository _baiTapNopRepository;
        ILopHocRepository _lopHocRepository;
        public BaiTapService(IBaiTapRepository baiTapRepository,
            IBaiTapNopRepository baiTapNopRepository,
            ILopHocRepository lopHocRepository,
            IMapper mapper)
            : base(baiTapRepository, mapper)
        {
            _baiTapRepository = baiTapRepository;
            _baiTapNopRepository = baiTapNopRepository;
            _lopHocRepository = lopHocRepository;
        }

        public async Task<ResponseEntity> GetByClassAndUserIdAsync(int classId, string userId)
        {
            try
            {
                LopHoc lopHoc = await _lopHocRepository.GetSingleByIdAsync(classId);

                IEnumerable<BaiTap> dsBaiTap = await _baiTapRepository.GetMultiByConditionAsync("MaLoTrinh", lopHoc.MaLoTrinh);
                List<BaiTapViewModel> dsBaiTapVm = _mapper.Map<List<BaiTapViewModel>>(dsBaiTap);

                dsBaiTapVm = await GetExerciseActived(dsBaiTapVm, lopHoc.NgayBatDau, classId, userId);

                return new ResponseEntity(StatusCodeConstants.OK, dsBaiTapVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> GetBySeriesIdAsync(dynamic id)
        {
            try
            {
                var dsBaiTap = await _baiTapRepository.GetMultiByConditionAsync("MaLoTrinh", id);

                List<BaiTapViewModel> dsBaiTapVm = _mapper.Map<List<BaiTapViewModel>>(dsBaiTap);
                return new ResponseEntity(StatusCodeConstants.OK, dsBaiTapVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        private async Task<List<BaiTapViewModel>> GetExerciseActived(List<BaiTapViewModel> dsBaiTap, DateTime ngayBatDau, int classId, string userId)
        {
            List<BaiTapViewModel> dsBaiTapVm = new List<BaiTapViewModel>();

            // KIỂM TRA XEM ĐẾN NGÀY KÍCH HOẠT CHƯA
            int soNgayTuLucKhaiGiang = FuncUtilities.TinhKhoangCachNgay(ngayBatDau);
            if (soNgayTuLucKhaiGiang > 0)
            {
                int soNgayKichHoat = 0;
                List<KeyValuePair<string, dynamic>> columns = null;

                foreach (BaiTapViewModel baiTap in dsBaiTap)
                {
                    soNgayKichHoat += baiTap.SoNgayKichHoat;

                    columns = new List<KeyValuePair<string, dynamic>>();
                    columns.Add(new KeyValuePair<string, dynamic>("MaBaiTap", baiTap.Id));
                    columns.Add(new KeyValuePair<string, dynamic>("MaNguoiDung", userId));
                    columns.Add(new KeyValuePair<string, dynamic>("MaLopHoc", classId));

                    // THÊM BÀI TẬP ĐÃ NỘP VÀO
                    BaiTapNop baiTapNop = await _baiTapNopRepository.GetSingleByListConditionAsync(columns);
                    baiTap.BaiTapNop = _mapper.Map<BaiTapNopViewModel>(baiTapNop);

                    // NẾU TỔNG SỐ NGÀY KÍCH HOẠT NHỎ HƠN SỐ NGÀY TÍNH TỪ LÚC KHAI GIẢNG ĐẾN HÔM NAY
                    if (soNgayKichHoat < soNgayTuLucKhaiGiang)
                    {
                        // HIỂN THỊ CHO HỌC VIÊN NHƯNG BÁO LÀ ĐÃ HẾT HẠN NỘP
                        baiTap.TrangThai = false;
                        dsBaiTapVm.Add(baiTap);
                    }
                    // NẾU TỔNG SỐ NGÀY KÍCH HOẠT BẰNG SỐ NGÀY TÍNH TỪ LÚC KHAI GIẢNG ĐẾN HÔM NAY
                    // HOẶC NẾU LỚN HƠN NHƯNG KHÔNG VƯỢT QUÁ SỐ NGÀY KÍCH HOẠT CỦA BÀI TẬP ĐÓ
                    else if (soNgayKichHoat >= soNgayTuLucKhaiGiang)
                    {
                        // HIỂN THỊ CHO HỌC VIÊN VÀ VẪN CHO NỘP BÀI
                        baiTap.TrangThai = true;
                        dsBaiTapVm.Add(baiTap);
                        break;
                    }
                }
            }
            return dsBaiTapVm;
        }
    }
}