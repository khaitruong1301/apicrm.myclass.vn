using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    public class BaiHocViewModel
    {
        public int Id { get; set; }
        public string TenBaiHoc { get; set; }
        public string BiDanh { get; set; }
        public string NoiDung { get; set; }
        public string MaLoaiBaiHoc { get; set; }
        public int MaChuongHoc { get; set; }
        public List<dynamic> DanhSachCauHoi { get; set; }
    }
}