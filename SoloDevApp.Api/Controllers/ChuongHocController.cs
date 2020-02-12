using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/chuonghoc")]
    [ApiController]
    public class ChuongHocController : ControllerBase
    {
        private IChuongHocService _chuongHocService;

        public ChuongHocController(IChuongHocService chuongHocService)
        {
            _chuongHocService = chuongHocService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _chuongHocService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _chuongHocService.GetSingleByIdAsync(id);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _chuongHocService.GetPagingAsync(page, size, keywords);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChuongHocViewModel model)
        {
            return await _chuongHocService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ChuongHocViewModel model)
        {
            return await _chuongHocService.UpdateAsync(id, model);
        }

        [HttpPut("lesson/{id}")]
        public async Task<IActionResult> Lesson(int id, [FromBody] BaiHocViewModel model)
        {
            return await _chuongHocService.AddLessonToChapterAsync(id, model);
        }

        [HttpPut("sorting/{id}")]
        public async Task<IActionResult> Sorting(int id, [FromBody] List<int> dsBaiHoc)
        {
            return await _chuongHocService.SortingAsync(id, dsBaiHoc);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _chuongHocService.DeleteByIdAsync(Ids);
        }
    }
}