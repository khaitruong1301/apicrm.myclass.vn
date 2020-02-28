using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    public class KhoaHocViewModel
    {
        public int Id { get; set; }
        public string TenKhoaHoc { get; set; }
        public string BiDanh { get; set; }
        public string HinhAnh { get; set; }
        public string TaiLieu { get; set; }
        public string MoTa { get; set; }
        public HashSet<int> DanhSachLoTrinh { get; set; }
        public List<int> DanhSachChuongHoc { get; set; }
        public int SoNgayKichHoat { get; set; }
        public bool KichHoatSan { get; set; }
    }
}