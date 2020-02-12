using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    public class CauHoiViewModel
    {
        public int Id { get; set; }
        public string TenCauHoi { get; set; }
        public string BiDanh { get; set; }
        public string NoiDung { get; set; }
        public string GhiChu { get; set; }
        public List<CauTraLoiViewModel> CauTraLoi { get; set; }
        public string LoaiCauTraLoi { get; set; }
        public List<CauHoiLienQuanViewModel> CauHoiLienQuan { get; set; }
        public int MaBaiHoc { get; set; }
    }
}