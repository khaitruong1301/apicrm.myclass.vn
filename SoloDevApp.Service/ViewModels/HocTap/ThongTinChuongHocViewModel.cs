using System;
using System.Collections.Generic;
using System.Text;

namespace SoloDevApp.Service.ViewModels
{
    public class ThongTinChuongHocViewModel
    {
        public int Id { get; set; }
        public string TenChuong { get; set; }
        public string BiDanh { get; set; }
        public List<dynamic> DanhSachBaiHoc { get; set; }
        public List<BaiHocViewModel> ThongTinBaiHoc { get; set; }
        public int MaKhoaHoc { get; set; }
    }
}
