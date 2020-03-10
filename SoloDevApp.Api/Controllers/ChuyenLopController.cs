using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/chuyenlop")]
    [ApiController]
    public class ChuyenLopController : ControllerBase
    {
        private IChuyenLopService _chuyenLopService;

        public ChuyenLopController(IChuyenLopService chuyenLopService)
        {
            _chuyenLopService = chuyenLopService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _chuyenLopService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _chuyenLopService.GetSingleByIdAsync(id);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _chuyenLopService.GetPagingAsync(page, size, keywords);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChuyenLopViewModel model)
        {
            return await _chuyenLopService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ChuyenLopViewModel model)
        {
            return await _chuyenLopService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _chuyenLopService.DeleteByIdAsync(Ids);
        }
    }
}