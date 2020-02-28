using AutoMapper;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.Utilities;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface ILopHocService : IService<LopHoc, LopHocViewModel>
    {
        Task<ResponseEntity> GetInfoByIdAsync(dynamic id);

        Task<ResponseEntity> GetCourseByClassIdAsync(dynamic id);

        Task<ResponseEntity> GetByUserIdAsync(dynamic id);
    }

    public class LopHocService : ServiceBase<LopHoc, LopHocViewModel>, ILopHocService
    {
        private ILopHocRepository _lopHocRepository;
        private INguoiDungRepository _nguoiDungRepository;
        private ILoTrinhRepository _loTrinhRepository;
        private IKhoaHocRepository _khoaHocRepository;
        private IBaiTapRepository _baiTapRepository;
        private IBaiTapNopRepository _baiTapNopRepository;

        public LopHocService(ILopHocRepository lopHocRepository,
            INguoiDungRepository nguoiDungRepository,
            ILoTrinhRepository loTrinhRepository,
            IKhoaHocRepository khoaHocRepository,
            IBaiTapRepository baiTapRepository,
            IBaiTapNopRepository baiTapNopRepository,
            IMapper mapper)
            : base(lopHocRepository, mapper)
        {
            _lopHocRepository = lopHocRepository;
            _nguoiDungRepository = nguoiDungRepository;
            _loTrinhRepository = loTrinhRepository;
            _khoaHocRepository = khoaHocRepository;
            _baiTapRepository = baiTapRepository;
            _baiTapNopRepository = baiTapNopRepository;
        }

        public async Task<ResponseEntity> GetByUserIdAsync(dynamic id)
        {
            try
            {
                NguoiDung nguoiDung = await _nguoiDungRepository.GetSingleByIdAsync(id);
                if (nguoiDung == null || nguoiDung.DanhSachLopHoc == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                List<LopHocViewModel> dsLopHocVm = new List<LopHocViewModel>();
                if (nguoiDung.DanhSachLopHoc != null)
                {
                    List<dynamic> dsMaLopHoc = JsonConvert.DeserializeObject<List<dynamic>>(nguoiDung.DanhSachLopHoc);
                    var dsLopHoc = await _lopHocRepository.GetMultiByListIdAsync(dsMaLopHoc);
                    dsLopHocVm = _mapper.Map<List<LopHocViewModel>>(dsLopHoc);
                }
                return new ResponseEntity(StatusCodeConstants.OK, dsLopHocVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> GetCourseByClassIdAsync(dynamic id)
        {
            try
            {
                LopHoc lopHoc = await _lopHocRepository.GetSingleByIdAsync(id);
                if (lopHoc == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                // LẤY DANH SÁCH KHÓA HỌC ĐÃ KÍCH HOẠT
                List<KhoaHoc> dsKhoaHocKichHoat = await GetCourseActived(lopHoc.MaLoTrinh, lopHoc.NgayBatDau);
                var dsKhoaHocVm = _mapper.Map<List<KhoaHocViewModel>>(dsKhoaHocKichHoat);

                // LẤY DANH SÁCH BÀI TẬP ĐÃ KÍCH HOẠT
                IEnumerable<BaiTap> dsBaiTap = await _baiTapRepository.GetMultiByConditionAsync("MaLoTrinh", lopHoc.MaLoTrinh);
                List<BaiTapViewModel> dsBaiTapVm = _mapper.Map<List<BaiTapViewModel>>(dsBaiTap);
                dsBaiTapVm = await GetExerciseActived(dsBaiTapVm, dsKhoaHocKichHoat, lopHoc.NgayBatDau);

                var modelVm = new KhoaHocKichHoatViewModel()
                {
                    DanhSachKhoaHoc = _mapper.Map<List<KhoaHocViewModel>>(dsKhoaHocKichHoat),
                    DanhSachBaiTap = dsBaiTapVm
                };

                return new ResponseEntity(StatusCodeConstants.OK, modelVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> GetInfoByIdAsync(dynamic id)
        {
            HashSet<dynamic> listId = new HashSet<dynamic>();
            try
            {
                LopHoc lopHoc = await _lopHocRepository.GetSingleByIdAsync(id);
                if (lopHoc == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                ThongTinLopHocViewModel thongTinLopHocVm = _mapper.Map<ThongTinLopHocViewModel>(lopHoc);
                foreach (dynamic item in thongTinLopHocVm.DanhSachGiangVien)
                {
                    listId.Add(item);
                }
                foreach (dynamic item in thongTinLopHocVm.DanhSachMentor)
                {
                    listId.Add(item);
                }
                foreach (dynamic item in thongTinLopHocVm.DanhSachHocVien)
                {
                    listId.Add(item);
                }

                // LẤY DANH SÁCH HỌC VIÊN
                var dsNguoiDung = await _nguoiDungRepository.GetMultiByIdAsync(listId.ToList());
                thongTinLopHocVm.DanhSachNguoiDung = _mapper.Map<List<NguoiDungViewModel>>(dsNguoiDung);

                // LẤY DANH SÁCH KHÓA HỌC ĐÃ KÍCH HOẠT
                List<KhoaHoc> dsKhoaHocKichHoat = await GetCourseActived(lopHoc.MaLoTrinh, lopHoc.NgayBatDau);
                thongTinLopHocVm.DanhSachKhoaHoc = _mapper.Map<List<KhoaHocViewModel>>(dsKhoaHocKichHoat);

                return new ResponseEntity(StatusCodeConstants.OK, thongTinLopHocVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        private async Task<List<KhoaHoc>> GetCourseActived(int maLoTrinh, DateTime ngayBatDau)
        {
            LoTrinh loTrinh = await _loTrinhRepository.GetSingleByIdAsync(maLoTrinh);

            List<KhoaHoc> dsKhoaHoc = new List<KhoaHoc>();
            if (loTrinh.DanhSachKhoaHoc != null)
            {
                // Convert string json thành mảng
                List<dynamic> dsMaKhoaHoc = JsonConvert.DeserializeObject<List<dynamic>>(loTrinh.DanhSachKhoaHoc);
                // Lấy danh sách khóa học theo mảng id
                var listKhoaHoc = await _khoaHocRepository.GetMultiByIdAsync(dsMaKhoaHoc);
                // Sắp xếp đúng thứ tự
                foreach (dynamic idKhoaHoc in dsMaKhoaHoc)
                {
                    dsKhoaHoc.Add(listKhoaHoc.FirstOrDefault(x => x.Id == idKhoaHoc));
                }
            }

            // KIỂM TRA XEM ĐẾN NGÀY KÍCH HOẠT CHƯA
            int demNgay = 0;
            int soNgayTuLucKhaiGiang = FuncUtilities.TinhKhoangCachNgay(ngayBatDau);
            List<KhoaHoc> dsKhoaHocKichHoat = new List<KhoaHoc>();
            foreach (KhoaHoc item in dsKhoaHoc)
            {
                // Nếu là khóa học kích hoạt sẵn cho học viên
                if (item.KichHoatSan && soNgayTuLucKhaiGiang >= -7)
                {
                    dsKhoaHocKichHoat.Add(item);
                }
                else
                {
                    demNgay += item.SoNgayKichHoat;
                    // Kiểm tra xem đã đến ngày kích hoạt chưa
                    // Mở khóa học mới trước 7 ngày (1 tuần) cho học viên
                    if (demNgay > (soNgayTuLucKhaiGiang - 7))
                    {
                        break;
                    }
                    else
                    {
                        dsKhoaHocKichHoat.Add(item);
                    }
                }
                
            }
            return dsKhoaHocKichHoat;
        }

        private async Task<List<BaiTapViewModel>> GetExerciseActived(List<BaiTapViewModel> dsBaiTap, List<KhoaHoc> dsKhoaHoc, DateTime ngayBatDau)
        {
            List<BaiTapViewModel> dsBaiTapVm = new List<BaiTapViewModel>();

            // KIỂM TRA XEM ĐẾN NGÀY KÍCH HOẠT CHƯA
            int soNgayTuLucKhaiGiang = FuncUtilities.TinhKhoangCachNgay(ngayBatDau);
            if (soNgayTuLucKhaiGiang > 0 && dsKhoaHoc.Count > 0)
            {
                KhoaHoc khoaHocDauTien = dsKhoaHoc.FirstOrDefault();
                if (soNgayTuLucKhaiGiang >= khoaHocDauTien.SoNgayKichHoat)
                {
                    int soNgayKichHoat = 0;
                    foreach (BaiTapViewModel baiTap in dsBaiTap)
                    {
                        soNgayKichHoat += baiTap.SoNgayKichHoat;
                        // NẾU TỔNG SỐ NGÀY KÍCH HOẠT NHỎ HƠN SỐ NGÀY TÍNH TỪ LÚC KHAI GIẢNG ĐẾN HÔM NAY
                        if (soNgayKichHoat < soNgayTuLucKhaiGiang)
                        {
                            // HIỂN THỊ CHO HỌC VIÊN NHƯNG BÁO LÀ ĐÃ HẾT HẠN NỘP
                            baiTap.TrangThai = false;
                            dsBaiTapVm.Add(baiTap);
                        }
                        // NẾU TỔNG SỐ NGÀY KÍCH HOẠT BẰNG SỐ NGÀY TÍNH TỪ LÚC KHAI GIẢNG ĐẾN HÔM NAY
                        // HOẶC NẾU LỚN HƠN NHƯNG KHÔNG VƯỢT QUÁ SỐ NGÀY KÍCH HOẠT CỦA BÀI TẬP ĐÓ
                        else if (soNgayKichHoat == soNgayTuLucKhaiGiang || (soNgayKichHoat - soNgayTuLucKhaiGiang) <= baiTap.SoNgayKichHoat)
                        {
                            // HIỂN THỊ CHO HỌC VIÊN VÀ VẪN CHO NỘP BÀI
                            baiTap.TrangThai = true;
                            dsBaiTapVm.Add(baiTap);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return dsBaiTapVm;
        }
    }
}