using AutoMapper;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Service.ViewModels;
using SoloDevApp.Service.Utilities;

namespace SoloDevApp.Service.AutoMapper
{
    public class ViewModelToEntityProfile : Profile
    {
        public ViewModelToEntityProfile()
        {
            /*=========== HỆ THỐNG =============*/
            CreateMap<QuyenViewModel, Quyen>();
            CreateMap<NhomQuyenViewModel, NhomQuyen>()
                .ForMember(entity => entity.DanhSachQuyen,
                                m => m.MapFrom(modelVm => JsonConvert.SerializeObject(modelVm.DanhSachQuyen)));

            /*=========== LỘ TRÌNH =============*/
            CreateMap<LoTrinhViewModel, LoTrinh>()
                .ForMember(entity => entity.DanhSachKhoaHoc,
                                m => m.MapFrom(modelVm => JsonConvert.SerializeObject(modelVm.DanhSachKhoaHoc)))
                .ForMember(entity => entity.DanhSachLopHoc,
                                m => m.MapFrom(modelVm => JsonConvert.SerializeObject(modelVm.DanhSachLopHoc)));

            /*=========== LỚP HỌC =============*/
            CreateMap<LopHocViewModel, LopHoc>()
                .ForMember(entity => entity.NgayBatDau,
                                m => m.MapFrom(modelVm => FuncUtilities.ConvertStringToDate(modelVm.NgayBatDau)))
                .ForMember(entity => entity.NgayKetThuc,
                                m => m.MapFrom(modelVm => FuncUtilities.ConvertStringToDate(modelVm.NgayKetThuc)))
                .ForMember(entity => entity.DanhSachGiangVien,
                                m => m.MapFrom(modelVm => JsonConvert.SerializeObject(modelVm.DanhSachGiangVien)))
                .ForMember(entity => entity.DanhSachHocVien,
                                m => m.MapFrom(modelVm => JsonConvert.SerializeObject(modelVm.DanhSachHocVien)))
                .ForMember(entity => entity.DanhSachMentor,
                                m => m.MapFrom(modelVm => JsonConvert.SerializeObject(modelVm.DanhSachMentor)));

            /*=========== KHÓA HỌC =============*/
            CreateMap<KhoaHocViewModel, KhoaHoc>()
                .ForMember(entity => entity.DanhSachLoTrinh,
                                m => m.MapFrom(modelVm => JsonConvert.SerializeObject(modelVm.DanhSachLoTrinh)))
                .ForMember(entity => entity.DanhSachChuongHoc,
                                m => m.MapFrom(modelVm => JsonConvert.SerializeObject(modelVm.DanhSachChuongHoc)));

            /*=========== CHƯƠNG HỌC =============*/
            CreateMap<ChuongHocViewModel, ChuongHoc>()
                .ForMember(entity => entity.DanhSachBaiHoc,
                                m => m.MapFrom(modelVm => JsonConvert.SerializeObject(modelVm.DanhSachBaiHoc)));

            /*=========== BÀI HỌC =============*/
            CreateMap<BaiHocViewModel, BaiHoc>()
                .ForMember(entity => entity.DanhSachCauHoi,
                                m => m.MapFrom(modelVm => JsonConvert.SerializeObject(modelVm.DanhSachCauHoi)));
            CreateMap<LoaiBaiHocViewModel, LoaiBaiHoc>();

            /*=========== BÀI TẬP =============*/
            CreateMap<BaiTapViewModel, BaiTap>();
            CreateMap<BaiTapNopViewModel, BaiTapNop>();

            /*=========== CÂU HỎI =============*/
            CreateMap<CauHoiViewModel, CauHoi>()
                .ForMember(entity => entity.CauTraLoi,
                                m => m.MapFrom(modelVm => JsonConvert.SerializeObject(modelVm.CauTraLoi)))
                .ForMember(entity => entity.CauHoiLienQuan,
                                m => m.MapFrom(modelVm => JsonConvert.SerializeObject(modelVm.CauHoiLienQuan)));

            /*=========== CHUYỂN LỚP =============*/
            CreateMap<ChuyenLopViewModel, ChuyenLop>();

            /*=========== TÀI KHOẢN =============*/
            CreateMap<DangKyViewModel, NguoiDung>();
            CreateMap<SuaNguoiDungViewModel, NguoiDung>();
            CreateMap<NguoiDungViewModel, NguoiDung>();

            /*=========== KHÁCH HÀNG =============*/
            CreateMap<KhachHangViewModel, KhachHang>()
                .ForMember(entity => entity.ThongTinKH,
                                m => m.MapFrom(modelVm => JsonConvert.SerializeObject(modelVm.ThongTinKH)))
                .ForMember(entity => entity.DiaChi,
                                m => m.MapFrom(modelVm => JsonConvert.SerializeObject(modelVm.DiaChi)));
            CreateMap<HocPhiViewModel, HocPhi>();

        }
    }
}