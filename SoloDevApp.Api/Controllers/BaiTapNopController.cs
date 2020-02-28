using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/nopbaitap")]
    [ApiController]
    public class BaiTapNopController : ControllerBase
    {
        private IBaiTapNopService _baiTapNopService;

        public BaiTapNopController(IBaiTapNopService baiTapNopService)
        {
            _baiTapNopService = baiTapNopService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _baiTapNopService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _baiTapNopService.GetSingleByIdAsync(id);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _baiTapNopService.GetPagingAsync(page, size, keywords);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BaiTapNopViewModel model)
        {
            return await _baiTapNopService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BaiTapNopViewModel model)
        {
            return await _baiTapNopService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _baiTapNopService.DeleteByIdAsync(Ids);
        }
    }
}