using AutoMapper;
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
    public interface IHocPhiService : IService<HocPhi, HocPhiViewModel>
    {
        Task<ResponseEntity> GetListDebtorToDayAsync();
        Task<ResponseEntity> GetListDebtorAsync();
    }

    public class HocPhiService : ServiceBase<HocPhi, HocPhiViewModel>, IHocPhiService
    {
        IHocPhiRepository _hocPhiRepository;
        public HocPhiService(IHocPhiRepository hocPhiRepository, IMapper mapper)
            : base(hocPhiRepository, mapper)
        {
            _hocPhiRepository = hocPhiRepository;
        }

        public async Task<ResponseEntity> GetListDebtorAsync()
        {
            try
            {
                string date = DateTime.Now.ToString("yyyy-dd-MM");
                IEnumerable<HocPhi> hocPhi = await _hocPhiRepository.GetListDebtorAsync(date);
                List<HocPhiViewModel> modelVm = _mapper.Map<List<HocPhiViewModel>>(hocPhi);
                return new ResponseEntity(StatusCodeConstants.OK, modelVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> GetListDebtorToDayAsync()
        {
            try
            {
                string date = DateTime.Now.ToString("yyyy-dd-MM");
                IEnumerable<HocPhi> hocPhi = await _hocPhiRepository.GetListDebtorToDayAsync(date);
                List<HocPhiViewModel> modelVm = _mapper.Map<List<HocPhiViewModel>>(hocPhi);
                return new ResponseEntity(StatusCodeConstants.OK, modelVm);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }
    }
}