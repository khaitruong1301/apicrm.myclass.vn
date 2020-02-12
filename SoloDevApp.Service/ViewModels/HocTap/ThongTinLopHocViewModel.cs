using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    public class ThongTinLopHocViewModel
    {
        public int Id { get; set; }
        public string TenLopHoc { get; set; }
        public string BiDanh { get; set; }
        public int SoHocVien { get; set; }
        public string NgayBatDau { get; set; }
        public string NgayKetThuc { get; set; }
        public float HocPhi { get; set; }
        public string ThoiKhoaBieu { get; set; }
        public HashSet<dynamic> DanhSachGiangVien { get; set; }
        public HashSet<dynamic> DanhSachMentor { get; set; }
        public HashSet<dynamic> DanhSachHocVien { get; set; }
        public HashSet<NguoiDungViewModel> DanhSachNguoiDung { get; set; }
        public int MaTrangThai { get; set; }
        public int MaLoTrinh { get; set; }
    }
}