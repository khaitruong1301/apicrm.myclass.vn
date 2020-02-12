using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoloDevApp.Service.ViewModels
{
    public class LoTrinhViewModel
    {
        public int Id { get; set; }

        [Required]
        public string TenLoTrinh { get; set; }
        public string BiDanh { get; set; }

        public string DeCuong { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; }
        public string VideoGioiThieu { get; set; }
        public int SoNguoiDangKy { get; set; }

        [Required]
        public float HocPhi { get; set; }

        public List<dynamic> DanhSachLopHoc { get; set; }
        public List<dynamic> DanhSachKhoaHoc { get; set; }
    }
}