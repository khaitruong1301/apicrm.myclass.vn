using System.ComponentModel;
using Microsoft.AspNetCore.Http;

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
        public string GhiChu { get; set; }
        [DefaultValue(false)]
        public bool TrangThai { get; set; }
        public BaiTapNopViewModel BaiTapNop { get; set; }

        public void contructorBaiTapViewModel(int id, string tenBaiTap, string biDanh, string noiDung, int soNgayKichHoat, int maLoTrinh, string ghiChu, bool trangThai)
        {
            this.Id = id;
            this.TenBaiTap = tenBaiTap;
            this.BiDanh = biDanh;
            this.NoiDung = noiDung;
            this.SoNgayKichHoat = soNgayKichHoat;
            this.MaLoTrinh = maLoTrinh;
            this.GhiChu = ghiChu;
            this.TrangThai = trangThai;

        }

    }
    //    public class BaiTapViewModelFileUpload
    //{
    //    public int Id { get; set; }
    //    public string TenBaiTap { get; set; }
    //    public string BiDanh { get; set; }
    //    public string NoiDung { get; set; }
    //    public int SoNgayKichHoat { get; set; }
    //    public int MaLoTrinh { get; set; }
    //    public string GhiChu { get; set; }
    //    [DefaultValue(false)]
    //    public bool TrangThai { get; set; }
    //    public BaiTapNopViewModel BaiTapNop { get; set; }
    //    public IFormFileCollection File { get; set; }


    //    public BaiTapViewModelFileUpload(int id,string tenBaiTap,string biDanh,string noiDung, int soNgayKichHoat,int maLoTrinh,string ghiChu,bool trangThai, IFormFileCollection file=null)
    //    {
    //        this.Id = id;
    //        this.TenBaiTap = tenBaiTap;
    //        this.BiDanh = biDanh;
    //        this.NoiDung = noiDung;
    //        this.SoNgayKichHoat = soNgayKichHoat;
    //        this.MaLoTrinh = maLoTrinh;
    //        this.GhiChu = ghiChu;
    //        this.TrangThai = trangThai;
            
    //        this.File = file;
    //    }
    //}
}