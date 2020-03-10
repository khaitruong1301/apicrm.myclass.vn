namespace SoloDevApp.Repository.Models
{
    public class ChuyenLop
    {
        public int Id { get; set; }
        public string MaNguoiDung { get; set; }
        public string TenNguoiDung { get; set; }
        public string BiDanh { get; set; }
        public int MaLopChuyenDi { get; set; }
        public int MaLopChuyenDen { get; set; }
        public string TenLopChuyenDi { get; set; }
        public string TenLopChuyenDen { get; set; }
        public string NgayChuyen { get; set; }
    }
}