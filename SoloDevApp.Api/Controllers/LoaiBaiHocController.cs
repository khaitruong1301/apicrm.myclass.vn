using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/loaibaihoc")]
    [ApiController]
    public class LoaiBaiHocController : ControllerBase
    {
        private ILoaiBaiHocService _loaiBaiHocService;

        public LoaiBaiHocController(ILoaiBaiHocService loaiBaiHocService)
        {
            _loaiBaiHocService = loaiBaiHocService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _loaiBaiHocService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _loaiBaiHocService.GetSingleByIdAsync(id);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _loaiBaiHocService.GetPagingAsync(page, size, keywords);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoaiBaiHocViewModel model)
        {
            return await _loaiBaiHocService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] LoaiBaiHocViewModel model)
        {
            return await _loaiBaiHocService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _loaiBaiHocService.DeleteByIdAsync(Ids);
        }
    }
}