using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/hocphi")]
    [ApiController]
    public class HocPhiController : ControllerBase
    {
        private IHocPhiService _hocPhiService;

        public HocPhiController(IHocPhiService hocPhiService)
        {
            _hocPhiService = hocPhiService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _hocPhiService.GetAllAsync();
        }

        [HttpGet("debtor")]
        public async Task<IActionResult> GetDebtor()
        {
            return await _hocPhiService.GetListDebtorAsync();
        }

        [HttpGet("debtor-today")]
        public async Task<IActionResult> GetDebtorToday()
        {
            return await _hocPhiService.GetListDebtorToDayAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _hocPhiService.GetSingleByIdAsync(id);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _hocPhiService.GetPagingAsync(page, size, keywords);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] HocPhiViewModel model)
        {
            return await _hocPhiService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] HocPhiViewModel model)
        {
            return await _hocPhiService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _hocPhiService.DeleteByIdAsync(Ids);
        }
    }
}