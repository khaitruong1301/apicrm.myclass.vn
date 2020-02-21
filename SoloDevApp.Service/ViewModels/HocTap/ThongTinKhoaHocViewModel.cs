using System;
using System.Collections.Generic;
using System.Text;

namespace SoloDevApp.Service.ViewModels
{
    public class ThongTinKhoaHocViewModel
    {
        public int Id { get; set; }
        public string TenKhoaHoc { get; set; }
        public string BiDanh { get; set; }
        public string HinhAnh { get; set; }
        public string VideoGioiThieu { get; set; }
        public string MoTa { get; set; }
        public HashSet<dynamic> DanhSachLoTrinh { get; set; }
        public List<dynamic> DanhSachChuongHoc { get; set; }
        public List<ThongTinChuongHocViewModel> ThongTinChuongHoc { get; set; }
        public int SoNgayKichHoat { get; set; }
        public bool KichHoatSan { get; set; }
    }
}
