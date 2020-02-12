using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/khoahoc")]
    [ApiController]
    public class KhoaHocController : ControllerBase
    {
        private IKhoaHocService _khoaHocService;

        public KhoaHocController(IKhoaHocService khoaHocService)
        {
            _khoaHocService = khoaHocService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _khoaHocService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _khoaHocService.GetInfoByIdAsync(id);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _khoaHocService.GetPagingAsync(page, size, keywords);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] KhoaHocViewModel model)
        {
            return await _khoaHocService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] KhoaHocViewModel model)
        {
            return await _khoaHocService.UpdateAsync(id, model);
        }

        [HttpPut("chapter/{id}")]
        public async Task<IActionResult> Chapter(int id, [FromBody] ChuongHocViewModel model)
        {
            return await _khoaHocService.AddChapterToCourseAsync(id, model);
        }

        [HttpPut("sorting/{id}")]
        public async Task<IActionResult> Sorting(int id, [FromBody] List<int> dsChuongHoc)
        {
            return await _khoaHocService.SortingAsync(id, dsChuongHoc);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _khoaHocService.DeleteByIdAsync(Ids);
        }
    }
}