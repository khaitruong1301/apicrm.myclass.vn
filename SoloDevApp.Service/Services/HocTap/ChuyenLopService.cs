using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.Utilities;
using SoloDevApp.Service.ViewModels;

namespace SoloDevApp.Service.Services
{
    public interface IChuyenLopService : IService<ChuyenLop, ChuyenLopViewModel>
    {
    }

    public class ChuyenLopService : ServiceBase<ChuyenLop, ChuyenLopViewModel>, IChuyenLopService
    {
        IChuyenLopRepository _chuyenLopRepository;
        INguoiDungRepository _nguoiDungRepository;
        ILopHocRepository _lopHocRepository;
        public ChuyenLopService(IChuyenLopRepository chuyenLopRepository,
            INguoiDungRepository nguoiDungRepository,
            ILopHocRepository lopHocRepository,
            IMapper mapper)
            : base(chuyenLopRepository, mapper)
        {
            _chuyenLopRepository = chuyenLopRepository;
            _nguoiDungRepository = nguoiDungRepository;
            _lopHocRepository = lopHocRepository;
        }

        public override async Task<ResponseEntity> InsertAsync(ChuyenLopViewModel modelVm)
        {
            try
            {
                List<KeyValuePair<string, dynamic>> columns = new List<KeyValuePair<string, dynamic>>();
                columns.Add(new KeyValuePair<string, dynamic>("MaNguoiDung", modelVm.MaNguoiDung));
                columns.Add(new KeyValuePair<string, dynamic>("MaLopChuyenDi", modelVm.MaLopChuyenDi));
                columns.Add(new KeyValuePair<string, dynamic>("MaLopChuyenDen", modelVm.MaLopChuyenDen));

                ChuyenLop entity = await _chuyenLopRepository.GetSingleByListConditionAsync(columns);
                if(entity != null)
                {
                    if(entity.MaLopChuyenDen != modelVm.MaLopChuyenDen && entity.MaLopChuyenDi != modelVm.MaLopChuyenDi)
                    {
                        entity.MaLopChuyenDi = modelVm.MaLopChuyenDen;
                        entity.MaLopChuyenDen = modelVm.MaLopChuyenDi;
                        await _chuyenLopRepository.InsertAsync(entity);
                    }
                    return new ResponseEntity(StatusCodeConstants.OK, modelVm, MessageConstants.UPDATE_SUCCESS);
                }

                // XÓA MÃ LỚP HỌC CŨ, THÊM MÃ LỚP HỌC MỚI VÀO DANH SÁCH LỚP HỌC CỦA BẢNG USER
                NguoiDung nguoiDung = await _nguoiDungRepository.GetSingleByIdAsync(modelVm.MaNguoiDung);
                HashSet<int> dsMaLopHoc = JsonConvert.DeserializeObject<HashSet<int>>(nguoiDung.DanhSachLopHoc);
                dsMaLopHoc.RemoveWhere(x => x == modelVm.MaLopChuyenDi);
                dsMaLopHoc.Add(modelVm.MaLopChuyenDen);
                nguoiDung.DanhSachLopHoc = JsonConvert.SerializeObject(dsMaLopHoc);

                // XÓA MÃ NGƯỜI DÙNG CỦA KHỎI LỚP HỌC CHUYỂN ĐI
                LopHoc lopChuyenDi = await _lopHocRepository.GetSingleByIdAsync(modelVm.MaLopChuyenDi);
                HashSet<string> dsMaNguoiDung = JsonConvert.DeserializeObject<HashSet<string>>(lopChuyenDi.DanhSachHocVien);
                dsMaNguoiDung.RemoveWhere(x => x == modelVm.MaNguoiDung);
                lopChuyenDi.DanhSachHocVien = JsonConvert.SerializeObject(dsMaNguoiDung);

                // THÊM MÃ NGƯỜI DÙNG VÀO LỚP HỌC CHUYỂN ĐẾN
                LopHoc lopChuyenDen = await _lopHocRepository.GetSingleByIdAsync(modelVm.MaLopChuyenDen);
                dsMaNguoiDung = JsonConvert.DeserializeObject<HashSet<string>>(lopChuyenDen.DanhSachHocVien);
                dsMaNguoiDung.Add(modelVm.MaNguoiDung);
                lopChuyenDen.DanhSachHocVien = JsonConvert.SerializeObject(dsMaNguoiDung);

                await _nguoiDungRepository.UpdateAsync(nguoiDung.Id, nguoiDung);
                await _lopHocRepository.UpdateAsync(lopChuyenDi.Id, lopChuyenDi);
                await _lopHocRepository.UpdateAsync(lopChuyenDen.Id, lopChuyenDen);

                modelVm.TenLopChuyenDi = lopChuyenDi.TenLopHoc;
                modelVm.TenLopChuyenDen = lopChuyenDen.TenLopHoc;
                modelVm.NgayChuyen = DateTime.Now.ToString("dd/MM/yyyy");
                entity = _mapper.Map<ChuyenLop>(modelVm);
                entity = await _chuyenLopRepository.InsertAsync(entity);

                return new ResponseEntity(StatusCodeConstants.OK, modelVm, MessageConstants.UPDATE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }
    }
}