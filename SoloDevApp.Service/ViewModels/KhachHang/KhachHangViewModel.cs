namespace SoloDevApp.Service.ViewModels
{
    public class KhachHangViewModel
    {
        public int Id { get; set; }
        public string TenKH { get; set; }
        public string BiDanh { get; set; }
        public DiaChiViewModel DiaChi { get; set; }
        public ThongTinKHViewModel ThongTinKH { get; set; }
        public HocPhiViewModel HocPhi { get; set; }
        public int MaTrangThaiKH { get; set; }
        public string MaDoiTacGioiThieu { get; set; }
        public string DanhSachNguoiTuVan { get; set; }
        public string LichSuGoiVaGhiChu { get; set; }
        public string MaNguoiTuVan { get; set; }
        public string MaNguonGioiThieu { get; set; }
        public string MaLoaiNguoiDung { get; set; }
        public string Captcha { get; set; }
    }
}