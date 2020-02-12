using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    public class NhomQuyenViewModel
    {
        public string Id { get; set; }
        public string TenNhom { get; set; }
        public string BiDanh { get; set; }
        public List<string> DanhSachQuyen { get; set; }
    }
}