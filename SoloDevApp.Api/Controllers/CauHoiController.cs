using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/cauhoi")]
    [ApiController]
    public class CauHoiController : ControllerBase
    {
        private ICauHoiService _cauHoiService;

        public CauHoiController(ICauHoiService CauHoiService)
        {
            _cauHoiService = CauHoiService;
        }

        [HttpGet]
        //[Authorize(Roles = "VIEW_ROLE")]
        public async Task<IActionResult> Get()
        {
            return await _cauHoiService.GetAllAsync();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _cauHoiService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "ADD_ROLE")]
        public async Task<IActionResult> Get(int id)
        {
            return await _cauHoiService.GetSingleByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CauHoiViewModel model)
        {
            return await _cauHoiService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CauHoiViewModel model)
        {
            return await _cauHoiService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _cauHoiService.DeleteByIdAsync(Ids);
        }
    }
}