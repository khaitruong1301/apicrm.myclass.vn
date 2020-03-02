using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Helpers;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.Utilities;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface IKhachHangService : IService<KhachHang, KhachHangViewModel>
    {
        Task<ResponseEntity> RegisterAsync(int id, KhachHangGhiDanhViewModel modelVm);
        Task<ResponseEntity> GenerateTokenAsync(int id);
        Task<ResponseEntity> CheckTokenAsync(string token);
        Task<ResponseEntity> UpdateInfoAsync(int id, ThongTinKHViewModel modelVm);
    }

    public class KhachHangService : ServiceBase<KhachHang, KhachHangViewModel>, IKhachHangService
    {
        private readonly IKhachHangRepository _khachHangRepository;
        private readonly ILopHocRepository _lopHocRepository;
        private readonly INguoiDungRepository _nguoiDungRepository;
        private readonly IAppSettings _appSettings;

        public KhachHangService(IKhachHangRepository khachHangRepository,
            ILopHocRepository lopHocRepository,
            INguoiDungRepository nguoiDungRepository,
            IAppSettings appSettings,
            IMapper mapper)
            : base(khachHangRepository, mapper)
        {
            _lopHocRepository = lopHocRepository;
            _nguoiDungRepository = nguoiDungRepository;
            _khachHangRepository = khachHangRepository;
            _appSettings = appSettings;
        }

        public override async Task<ResponseEntity> UpdateAsync(dynamic id, KhachHangViewModel modelVm)
        {
            try
            {
                var khachHang = await _khachHangRepository.GetSingleByIdAsync(id);
                if (khachHang == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, modelVm);

                KhachHangViewModel khachHangVm = _mapper.Map<KhachHangViewModel>(khachHang);
                modelVm.ThongTinKH.MucLuongMongMuon = khachHangVm.ThongTinKH.MucLuongMongMuon;
                modelVm.ThongTinKH.MucLuongMongMuonSau2Nam = khachHangVm.ThongTinKH.MucLuongMongMuonSau2Nam;
                modelVm.ThongTinKH.TraiNganh = khachHangVm.ThongTinKH.TraiNganh;
                modelVm.ThongTinKH.TiengAnh = khachHangVm.ThongTinKH.TiengAnh;
                modelVm.ThongTinKH.CmndTruoc = khachHangVm.ThongTinKH.CmndTruoc;
                modelVm.ThongTinKH.CmndSau = khachHangVm.ThongTinKH.CmndSau;
                modelVm.ThongTinKH.NoilamViec = khachHangVm.ThongTinKH.NoilamViec;
                modelVm.ThongTinKH.HoTroTimViec = khachHangVm.ThongTinKH.HoTroTimViec;
                modelVm.ThongTinKH.MaCaNhan = khachHangVm.ThongTinKH.MaCaNhan;


                khachHang = _mapper.Map<KhachHang>(khachHangVm);
                await _khachHangRepository.UpdateAsync(id, khachHang);

                return new ResponseEntity(StatusCodeConstants.OK, modelVm, MessageConstants.UPDATE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> CheckTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var jsonToken = handler.ReadJwtToken(token).Payload;
                var dateEXP = FuncUtilities.ConvertToTimeStamp((int)jsonToken.Exp);
                var dateNow = FuncUtilities.ConvertStringToDate();
                if (dateEXP < dateNow)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST);
                return new ResponseEntity(StatusCodeConstants.OK, jsonToken);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> GenerateTokenAsync(int id)
        {
            try
            {
                KhachHang khachHang = await _khachHangRepository.GetSingleByIdAsync(id);
                if (khachHang == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);
                string token = GenerateToken(khachHang);
                return new ResponseEntity(StatusCodeConstants.CREATED, token);
            }
            catch(Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> RegisterAsync(int id, KhachHangGhiDanhViewModel modelVm)
        {
            try
            {
                if (await _khachHangRepository.GetSingleByIdAsync(id) == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                DangKyViewModel dangKyModel = new DangKyViewModel()
                {
                    Email = modelVm.Email,
                    MatKhau = modelVm.MatKhau,
                    HoTen = modelVm.HoTen,
                    BiDanh = modelVm.BiDanh,
                    SoDT = modelVm.SoDT,
                    Avatar = "/static/user-icon.png"
                };

                NguoiDung entity = await _nguoiDungRepository.GetByEmailAsync(modelVm.Email);
                // Tạo tài khoản cho khách hàng nếu chưa có
                if (entity == null)
                {
                    entity = _mapper.Map<NguoiDung>(dangKyModel);
                    entity.Id = Guid.NewGuid().ToString();
                    // Mã hóa mật khẩu
                    entity.MatKhau = BCrypt.Net.BCrypt.HashPassword(modelVm.MatKhau);
                    entity.MaNhomQuyen = "HOCVIEN";

                    entity = await _nguoiDungRepository.InsertAsync(entity);
                }
                // Lấy ra lớp học có id trùng với mã lớp học truyền lên
                LopHoc lopHoc = await _lopHocRepository.GetSingleByIdAsync(modelVm.MaLopHoc);

                //LopHocViewModel lopHocVm = _mapper.Map<LopHocViewModel>(lopHoc);
                // Thêm vào danh sách
                HashSet<string> dsHocVien = JsonConvert.DeserializeObject<HashSet<string>>(lopHoc.DanhSachHocVien);
                dsHocVien.Add(entity.Id);

                lopHoc.DanhSachHocVien = JsonConvert.SerializeObject(dsHocVien);

                // Cập nhật lại thông tin lớp
                await _lopHocRepository.UpdateAsync(lopHoc.Id, lopHoc);

                // Cập nhật lại danh sách lớp học cho người dùng
                HashSet<string> dsLopHoc = new HashSet<string>();
                if(!string.IsNullOrEmpty(entity.DanhSachLopHoc))
                {
                    dsLopHoc = JsonConvert.DeserializeObject<HashSet<string>>(entity.DanhSachLopHoc);
                }
                dsLopHoc.Add(lopHoc.Id.ToString());
                entity.DanhSachLopHoc = JsonConvert.SerializeObject(dsLopHoc);

                await _nguoiDungRepository.UpdateAsync(entity.Id, entity);

                return new ResponseEntity(StatusCodeConstants.CREATED, lopHoc, MessageConstants.SIGNUP_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> UpdateInfoAsync(int id, ThongTinKHViewModel modelVm)
        {
            try
            {
                var khachHang = await _khachHangRepository.GetSingleByIdAsync(id);
                var khachHangVm = _mapper.Map<KhachHangViewModel>(khachHang);

                var thongTinKhacHang = khachHangVm.ThongTinKH;

                modelVm.NgaySinh = thongTinKhacHang.NgaySinh;
                modelVm.MucTieu = thongTinKhacHang.MucTieu;
                modelVm.GhiChu = thongTinKhacHang.GhiChu;
                modelVm.TiengAnh = thongTinKhacHang.TiengAnh;
                modelVm.MaCaNhan = thongTinKhacHang.MaCaNhan;

                khachHangVm.ThongTinKH = modelVm;

                khachHang = _mapper.Map<KhachHang>(khachHangVm);
                await _khachHangRepository.UpdateAsync(id, khachHang);
                return new ResponseEntity(StatusCodeConstants.OK, null, MessageConstants.UPDATE_SUCCESS);
            }
            catch(Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        private string GenerateToken(KhachHang khachHang)
        {
            var arrInfo = new List<Claim> {
                new Claim("MaKhachHang", khachHang.Id.ToString())
            };

            SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_appSettings.Secret));
            var token = new JwtSecurityToken(
                        claims: arrInfo,
                        notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                        expires: new DateTimeOffset(DateTime.Now.AddDays(3)).DateTime,
                        signingCredentials: new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256)
                );

            //string token1 = new JwtSecurityTokenHandler().WriteToken(token);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}