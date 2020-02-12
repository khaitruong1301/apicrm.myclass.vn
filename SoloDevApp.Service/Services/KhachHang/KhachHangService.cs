using AutoMapper;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface IKhachHangService : IService<KhachHang, KhachHangViewModel>
    {
        Task<ResponseEntity> RegisterAsync(int id, KhachHangGhiDanhViewModel modelVm);
    }

    public class KhachHangService : ServiceBase<KhachHang, KhachHangViewModel>, IKhachHangService
    {
        private readonly IKhachHangRepository _khachHangRepository;
        private readonly ILopHocRepository _lopHocRepository;
        private readonly INguoiDungRepository _nguoiDungRepository;

        public KhachHangService(IKhachHangRepository khachHangRepository,
            ILopHocRepository lopHocRepository,
            INguoiDungRepository nguoiDungRepository,
            IMapper mapper)
            : base(khachHangRepository, mapper)
        {
            _lopHocRepository = lopHocRepository;
            _nguoiDungRepository = nguoiDungRepository;
            _khachHangRepository = khachHangRepository;
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

                return new ResponseEntity(StatusCodeConstants.CREATED, lopHoc, MessageConstants.SIGNUP_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }
    }
}