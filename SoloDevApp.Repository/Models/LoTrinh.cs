namespace SoloDevApp.Repository.Models
{
    public class LoTrinh
    {
        public int Id { get; set; }
        public string TenLoTrinh { get; set; }
        public string BiDanh { get; set; }
        public string DeCuong { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; }
        public string VideoGioiThieu { get; set; }
        public int SoNguoiDangKy { get; set; }
        public float HocPhi { get; set; }
        public string DanhSachLopHoc { get; set; }
        public string DanhSachKhoaHoc { get; set; }
    }
}