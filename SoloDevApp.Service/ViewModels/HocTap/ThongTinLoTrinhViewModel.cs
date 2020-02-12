﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SoloDevApp.Service.ViewModels
{
    public class ThongTinLoTrinhViewModel
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
        public List<dynamic> DanhSachKhoaHoc { get; set; }
        public List<dynamic> DanhSachLopHoc { get; set; }
        public List<KhoaHocViewModel> ThongTinKhoaHoc { get; set; }
        public List<LopHocViewModel> ThongTinLopHoc { get; set; }
    }
}
