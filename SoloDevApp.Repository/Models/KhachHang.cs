namespace SoloDevApp.Repository.Models
{
    public class KhachHang
    {
        public int Id { get; set; }
        public string TenKH { get; set; }
        public string BiDanh { get; set; }
        public string DiaChi { get; set; }
        public string ThongTinKH { get; set; }
        public int MaTrangThaiKH { get; set; }
        public string MaDoiTacGioiThieu { get; set; }
        public string DanhSachNguoiTuVan { get; set; }
        public string LichSuGoiVaGhiChu { get; set; }
        public string MaNguoiTuVan { get; set; }
        public string MaNguonGioiThieu { get; set; }
        public string MaLoaiNguoiDung { get; set; }
    }
}