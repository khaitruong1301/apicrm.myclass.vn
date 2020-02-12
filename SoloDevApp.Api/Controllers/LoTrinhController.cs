using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/lotrinh")]
    [ApiController]
    public class LoTrinhController : ControllerBase
    {
        private ILoTrinhService _loTrinhService;

        public LoTrinhController(ILoTrinhService loTrinhService)
        {
            _loTrinhService = loTrinhService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _loTrinhService.GetAllAsync();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _loTrinhService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _loTrinhService.GetSingleByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoTrinhViewModel model)
        {
            return await _loTrinhService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] LoTrinhViewModel model)
        {
            return await _loTrinhService.UpdateAsync(id, model);
        }

        [HttpPut("course/{id}")]
        public async Task<IActionResult> Course(int id, [FromBody] HashSet<dynamic> dsMaKhoaHoc)
        {
            return await _loTrinhService.UpdateCourseFromSeriesAsync(id, dsMaKhoaHoc);
        }

        [HttpPut("sorting/{id}")]
        public async Task<IActionResult> Sorting(int id, [FromBody] List<dynamic> dsMaKhoaHoc)
        {
            return await _loTrinhService.SortingAsync(id, dsMaKhoaHoc);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _loTrinhService.DeleteByIdAsync(Ids);
        }
    }
}