using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/baihoc")]
    [ApiController]
    public class BaiHocController : ControllerBase
    {
        private IBaiHocService _baiHocService;

        public BaiHocController(IBaiHocService baiHocService)
        {
            _baiHocService = baiHocService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _baiHocService.GetAllAsync();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _baiHocService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _baiHocService.GetSingleByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BaiHocViewModel model)
        {
            return await _baiHocService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BaiHocViewModel model)
        {
            return await _baiHocService.UpdateAsync(id, model);
        }

        [HttpPut("question/{id}")]
        public async Task<IActionResult> Question(int id, [FromBody] CauHoiViewModel model)
        {
            return await _baiHocService.AddQuestionToLessonAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _baiHocService.DeleteByIdAsync(Ids);
        }
    }
}