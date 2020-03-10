using System;

namespace SoloDevApp.Repository.Models
{
    public class HocPhi
    {
        public int Id { get; set; }
        public int MaKH { get; set; }
        public string TenKH { get; set; }
        public float DaDong { get; set; }
        public float ConLai { get; set; }
        public DateTime HanDongTien1 { get; set; }
        public DateTime HanDongTien2 { get; set; }
        public DateTime HanDongTien3 { get; set; }
        public string GhiChu1 { get; set; }
        public string GhiChu2 { get; set; }
        public string GhiChu3 { get; set; }
    }
}