using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/quyen")]
    [ApiController]
    public class QuyenController : ControllerBase
    {
        private IQuyenService _quyenService;

        public QuyenController(IQuyenService quyenService)
        {
            _quyenService = quyenService;
        }

        [HttpGet]
        //[Authorize(Roles = "VIEW_ROLE")]
        public async Task<IActionResult> Get()
        {
            return await _quyenService.GetAllAsync();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _quyenService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "ADD_ROLE")]
        public async Task<IActionResult> Get(string id)
        {
            return await _quyenService.GetSingleByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QuyenViewModel model)
        {
            return await _quyenService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] QuyenViewModel model)
        {
            return await _quyenService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _quyenService.DeleteByIdAsync(Ids);
        }
    }
}