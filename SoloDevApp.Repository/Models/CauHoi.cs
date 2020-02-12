namespace SoloDevApp.Repository.Models
{
    public class CauHoi
    {
        public int Id { get; set; }
        public string TenCauHoi { get; set; }
        public string BiDanh { get; set; }
        public string NoiDung { get; set; }
        public string GhiChu { get; set; }
        public string CauTraLoi { get; set; }
        public string LoaiCauTraLoi { get; set; }
        public string CauHoiLienQuan { get; set; }
        public int MaBaiHoc { get; set; }
    }
}