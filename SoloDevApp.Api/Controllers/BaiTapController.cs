using Microsoft.AspNetCore.Mvc;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/baitap")]
    [ApiController]
    public class BaiTapController : ControllerBase
    {
        private IBaiTapService _baiTapService;

        public BaiTapController(IBaiTapService baiTapService)
        {
            _baiTapService = baiTapService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _baiTapService.GetAllAsync();
        }

        [HttpGet("byseries/{id}")]
        public async Task<IActionResult> GetBySeries(int id)
        {
            return await _baiTapService.GetBySeriesIdAsync(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _baiTapService.GetSingleByIdAsync(id);
        }

        [HttpGet("{classId}/{userId}")]
        public async Task<IActionResult> Get(int classId, string userId)
        {
            return await _baiTapService.GetByClassAndUserIdAsync(classId, userId);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _baiTapService.GetPagingAsync(page, size, keywords);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BaiTapViewModel model)
        {
            return await _baiTapService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BaiTapViewModel model)
        {
            return await _baiTapService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _baiTapService.DeleteByIdAsync(Ids);
        }
    }
}