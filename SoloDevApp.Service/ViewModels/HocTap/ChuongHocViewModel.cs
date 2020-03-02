using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    public class ChuongHocViewModel
    {
        public int Id { get; set; }
        public string TenChuong { get; set; }
        public string BiDanh { get; set; }
        public List<dynamic> DanhSachBaiHoc { get; set; }
        public int MaKhoaHoc { get; set; }
    }
}