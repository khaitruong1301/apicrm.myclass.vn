using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/khachhang")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private IKhachHangService _khachHangService;

        public KhachHangController(IKhachHangService khachHangService)
        {
            _khachHangService = khachHangService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _khachHangService.GetAllAsync();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _khachHangService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _khachHangService.GetSingleByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] KhachHangViewModel model)
        {
            return await _khachHangService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] KhachHangViewModel model)
        {
            return await _khachHangService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _khachHangService.DeleteByIdAsync(Ids);
        }

        // Ghi danh vào lớp học
        [HttpPut("register/{id}")]
        public async Task<IActionResult> Register(int id, [FromBody] KhachHangGhiDanhViewModel model)
        {
            return await _khachHangService.RegisterAsync(id, model);
        }
    }
}