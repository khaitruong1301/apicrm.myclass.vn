using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    public class KhoaHocKichHoatViewModel
    {
        public List<KhoaHocViewModel> DanhSachKhoaHoc { set; get; }
        public List<BaiTapViewModel> DanhSachBaiTap { set; get; }
        public List<BaiTapNopViewModel> DanhSachBaiTapNop { set; get; }
    }
}