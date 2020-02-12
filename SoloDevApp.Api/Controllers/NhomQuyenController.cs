using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/NhomQuyen")]
    [ApiController]
    public class NhomQuyenController : ControllerBase
    {
        private INhomQuyenService _nhomQuyenService;

        public NhomQuyenController(INhomQuyenService nhomQuyenService)
        {
            _nhomQuyenService = nhomQuyenService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _nhomQuyenService.GetAllAsync();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _nhomQuyenService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return await _nhomQuyenService.GetSingleByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NhomQuyenViewModel model)
        {
            return await _nhomQuyenService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] NhomQuyenViewModel model)
        {
            return await _nhomQuyenService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _nhomQuyenService.DeleteByIdAsync(Ids);
        }
    }
}