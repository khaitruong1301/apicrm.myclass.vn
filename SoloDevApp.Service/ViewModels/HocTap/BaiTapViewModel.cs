using System.ComponentModel;

namespace SoloDevApp.Service.ViewModels
{
    public class BaiTapViewModel
    {
        public int Id { get; set; }
        public string TenBaiTap { get; set; }
        public string BiDanh { get; set; }
        public string NoiDung { get; set; }
        public int SoNgayKichHoat { get; set; }
        public int MaLoTrinh { get; set; }

        [DefaultValue(false)]
        public bool TrangThai { get; set; }
        public BaiTapNopViewModel BaiTapNop { get; set; }
    }
}