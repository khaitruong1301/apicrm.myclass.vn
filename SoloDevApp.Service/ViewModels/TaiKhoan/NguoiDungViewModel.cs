using SoloDevApp.Repository.Models;
using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    public class NguoiDungViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string HoTen { get; set; }
        public string BiDanh { get; set; }
        public string SoDT { get; set; }
        public string FacebookId { get; set; }
        public string Avatar { get; set; }
        public string Urls { get; set; }
        public string Token { get; set; }
        public string ThongTinMoRong { get; set; }
        public string DanhSachLopHoc { get; set; }
        public List<LopHoc> ThongTinLopHoc { get; set; }
        public string MaNhomQuyen { get; set; }
    }
}