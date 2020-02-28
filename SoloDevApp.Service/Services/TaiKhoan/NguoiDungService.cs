using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Helpers;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface INguoiDungService : IService<NguoiDung, NguoiDungViewModel>
    {
        Task<ResponseEntity> SignInAsync(DangNhapViewModel modelVm);

        Task<ResponseEntity> SignInFacebookAsync(DangNhapFacebookViewModel modelVm);

        Task<ResponseEntity> SignUpAsync(DangKyViewModel modelVm);

        Task<ResponseEntity> InsertUserAsync(DangKyViewModel modelVm);

        Task<ResponseEntity> UpdateUserAsync(string id, SuaNguoiDungViewModel modelVm);

        Task<ResponseEntity> ChangePasswordAsync(DoiMatKhauViewModel modelVm);

        Task<ResponseEntity> GetByRoleGroupAsync(string column, List<dynamic> values);
    }

    public class NguoiDungService : ServiceBase<NguoiDung, NguoiDungViewModel>, INguoiDungService
    {
        private readonly INguoiDungRepository _nguoiDungRepository;
        private readonly INhomQuyenRepository _nhomQuyenRepository;
        private readonly IKhachHangRepository _khachHangRepository;
        private readonly ILopHocRepository _lopHocRepository;
        private readonly HttpClient _httpClient;
        private readonly IAppSettings _appSettings;

        public NguoiDungService(INguoiDungRepository nguoiDungRepository,
            IMapper mapper, INhomQuyenRepository nhomQuyenRepository,
            IKhachHangRepository khachHangRepository,
            ILopHocRepository lopHocRepository,
            IAppSettings appSettings)
            : base(nguoiDungRepository, mapper)
        {
            _nguoiDungRepository = nguoiDungRepository;
            _nhomQuyenRepository = nhomQuyenRepository;
            _khachHangRepository = khachHangRepository;
            _lopHocRepository = lopHocRepository;
            _appSettings = appSettings;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ResponseEntity> ChangePasswordAsync(DoiMatKhauViewModel modelVm)
        {
            try
            {
                NguoiDung entity = await _nguoiDungRepository.GetByEmailAsync(modelVm.Email);
                if (entity == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);

                entity.MatKhau = BCrypt.Net.BCrypt.HashPassword(modelVm.MatKhau);

                entity = await _nguoiDungRepository.UpdateAsync(entity.Id, entity);
                if (entity == null)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.UPDATE_ERROR);

                return new ResponseEntity(StatusCodeConstants.OK, modelVm, MessageConstants.UPDATE_SUCCESS);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER);
            }
        }

        public async Task<ResponseEntity> GetByRoleGroupAsync(string column, List<dynamic> values)
        {
            try
            {
                var columns = new List<KeyValuePair<string, dynamic>>();
                foreach (string value in values)
                {
                    columns.Add(new KeyValuePair<string, dynamic>(column, value));
                }

                IEnumerable<NguoiDung> entities = await _nguoiDungRepository.GetMultiByListConditionAsync(columns);
                List<NguoiDungViewModel> modelVm = _mapper.Map<List<NguoiDungViewModel>>(entities);
                return new ResponseEntity(StatusCodeConstants.OK, modelVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> SignInAsync(DangNhapViewModel modelVm)
        {
            try
            {
                // Lấy ra thông tin người dùng từ database dựa vào email
                NguoiDung entity = await _nguoiDungRepository.GetByEmailAsync(modelVm.Email);
                if (entity == null)// Nếu email sai
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, modelVm, MessageConstants.SIGNIN_WRONG);
                // Kiểm tra mật khẩu có khớp không
                if (!BCrypt.Net.BCrypt.Verify(modelVm.MatKhau, entity.MatKhau))
                    // Nếu password không khớp
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, modelVm, MessageConstants.SIGNIN_WRONG);
                // Tạo token
                string token = await GenerateToken(entity);
                if (token == string.Empty)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.TOKEN_GENERATE_ERROR);

                entity.Token = token;

                NguoiDungViewModel model = _mapper.Map<NguoiDungViewModel>(entity);
                return new ResponseEntity(StatusCodeConstants.OK, model, MessageConstants.SIGNIN_SUCCESS);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.SIGNIN_ERROR);
            }
        }

        public async Task<ResponseEntity> SignInFacebookAsync(DangNhapFacebookViewModel modelVm)
        {
            string[] ERR_MESSAGE = { "Vui lòng nhập email bạn đã đăng ký!", "Email này đã được sử dụng cho tài khoản facebook khác!", "Email không chính xác!" };
            string[] ERR_STATUS = { "EMAIL_ENTER", "EMAIL_EXISTS", "EMAIL_INCORRECT" };

            try
            {
                await _lopHocRepository.EnableAsync();
                await _lopHocRepository.DisableAsync();

                NguoiDung entity = await _nguoiDungRepository.GetByFacebookAsync(modelVm.FacebookId);
                if (entity != null) // Nếu FacebookId đúng => đăng nhập thành công
                {
                    // Tạo token
                    entity.Token = await GenerateToken(entity);
                    NguoiDungViewModel model = _mapper.Map<NguoiDungViewModel>(entity);
                    return new ResponseEntity(StatusCodeConstants.OK, model, MessageConstants.SIGNIN_SUCCESS);
                }

                // Nếu facebook id sai và email chưa nhập
                if (string.IsNullOrEmpty(modelVm.Email))
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ERR_STATUS[0], ERR_MESSAGE[0]);

                // Lấy ra thông tin người dùng từ database dựa vào email
                entity = await _nguoiDungRepository.GetByEmailAsync(modelVm.Email);
                if (entity == null)
                {
                    // Kiểm tra xem email đã tồn tại trong bảng khách hàng chưa
                    //  - Nếu chưa có thông báo đăng nhập thất bại
                    //  - Nếu có thì tạo tài khoản cho user=> đăng nhập thành công
                    KhachHang khachHang = await _khachHangRepository.GetByEmailAsync(modelVm.Email);
                    if (khachHang == null)
                        return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ERR_STATUS[2], ERR_MESSAGE[2]);

                    ThongTinKHViewModel thongTinKHVm = JsonConvert.DeserializeObject<ThongTinKHViewModel>(khachHang.ThongTinKH);
                    // Tạo tài khoản mới cho user
                    entity = new NguoiDung();
                    entity.Id = Guid.NewGuid().ToString();
                    entity.Email = thongTinKHVm.Email;
                    entity.MatKhau = BCrypt.Net.BCrypt.HashPassword("Cybersoft@123");
                    entity.HoTen = khachHang.TenKH;
                    entity.BiDanh = khachHang.BiDanh;
                    entity.SoDT = thongTinKHVm.SoDienThoai;
                    entity.Avatar = "/static/user-icon.png";
                    entity.MaNhomQuyen = "HOCVIEN";
                    // Thực hiện truy vấn thêm mới
                    entity = await _nguoiDungRepository.InsertAsync(entity);
                    if (entity == null)
                        return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.SIGNIN_ERROR);

                }
                // Email đúng, FacebookId có tồn tại nhưng không khớp với facebook id đang đăng nhập
                // Cái này để tránh trường hợp 1 email xài cho nhiều tài khoản
                else if (!string.IsNullOrEmpty(entity.FacebookId))
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ERR_STATUS[1], ERR_MESSAGE[1]);

                // Lưu FacebookId vào database
                entity.FacebookId = modelVm.FacebookId;
                entity = await _nguoiDungRepository.UpdateAsync(entity.Id, entity);

                // Tạo token
                entity.Token = await GenerateToken(entity);
                NguoiDungViewModel result = _mapper.Map<NguoiDungViewModel>(entity);
                return new ResponseEntity(StatusCodeConstants.OK, result, MessageConstants.SIGNIN_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, ex.Message, MessageConstants.SIGNIN_ERROR);
            }
        }

        public async Task<ResponseEntity> SignUpAsync(DangKyViewModel modelVm)
        {
            try
            {
                NguoiDung entity = await _nguoiDungRepository.GetByEmailAsync(modelVm.Email);
                if (entity != null) // Kiểm tra email đã được sử dụng bởi tài khoản khác chưa
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.EMAIL_EXITST);

                entity = _mapper.Map<NguoiDung>(modelVm);

                entity.Id = Guid.NewGuid().ToString();
                // Mã hóa mật khẩu
                entity.MatKhau = BCrypt.Net.BCrypt.HashPassword(modelVm.MatKhau);
                entity.Avatar = !string.IsNullOrEmpty(modelVm.Avatar) ? modelVm.Avatar : "/static/user-icon.png";
                entity.MaNhomQuyen = "HOCVIEN";

                entity = await _nguoiDungRepository.InsertAsync(entity);
                if (entity == null)
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.SIGNUP_ERROR);

                NguoiDungViewModel model = _mapper.Map<NguoiDungViewModel>(entity);
                return new ResponseEntity(StatusCodeConstants.CREATED, model, MessageConstants.SIGNUP_SUCCESS);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.SIGNUP_ERROR);
            }
        }

        public async Task<ResponseEntity> InsertUserAsync(DangKyViewModel modelVm)
        {
            try
            {
                NguoiDung entity = await _nguoiDungRepository.GetByEmailAsync(modelVm.Email);
                if (entity != null) // Kiểm tra email đã được sử dụng bởi tài khoản khác chưa
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.EMAIL_EXITST);

                entity = _mapper.Map<NguoiDung>(modelVm);

                entity.Id = Guid.NewGuid().ToString();
                // Mã hóa mật khẩu
                entity.MatKhau = BCrypt.Net.BCrypt.HashPassword(modelVm.MatKhau);
                entity.Avatar = !string.IsNullOrEmpty(modelVm.Avatar) ? modelVm.Avatar : "/static/user-icon.png";

                entity = await _nguoiDungRepository.InsertAsync(entity);

                NguoiDungViewModel model = _mapper.Map<NguoiDungViewModel>(entity);
                return new ResponseEntity(StatusCodeConstants.CREATED, model, MessageConstants.SIGNUP_SUCCESS);
            }
            catch
            {
                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.SIGNUP_ERROR);
            }
        }

        public async Task<ResponseEntity> UpdateUserAsync(string id, SuaNguoiDungViewModel modelVm)
        {
            try
            {
                NguoiDung entity = await _nguoiDungRepository.GetSingleByIdAsync(id);
                if (entity == null)
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, modelVm);

                entity = _mapper.Map<NguoiDung>(modelVm);
                await _nguoiDungRepository.UpdateAsync(id, entity);

                return new ResponseEntity(StatusCodeConstants.OK, modelVm, MessageConstants.UPDATE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        private async Task<string> GenerateToken(NguoiDung entity)
        {
            try
            {
                NhomQuyen nhomQuyen = await _nhomQuyenRepository.GetSingleByIdAsync(entity.MaNhomQuyen);
                if (nhomQuyen == null)
                    return string.Empty;

                List<string> roles = JsonConvert.DeserializeObject<List<string>>(nhomQuyen.DanhSachQuyen);

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, entity.Id));
                claims.Add(new Claim(ClaimTypes.Email, entity.Email));
                foreach (var item in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item.Trim()));
                }

                var secret = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var token = new JwtSecurityToken(
                        claims: claims,
                        notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                        expires: new DateTimeOffset(DateTime.Now.AddMinutes(60)).DateTime,
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
                    );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}