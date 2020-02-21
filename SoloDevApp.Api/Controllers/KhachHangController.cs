using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SoloDevApp.Service.Helpers;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.Utilities;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/khachhang")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly IKhachHangService _khachHangService;
        private readonly ICaptchaSettings _captchaSettings;

        public KhachHangController(IKhachHangService khachHangService,
            ICaptchaSettings captchaSettings)
        {
            _khachHangService = khachHangService;
            _captchaSettings = captchaSettings;
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

        [HttpGet("generate-token/{id}")]
        public async Task<IActionResult> GenerateForm(int id)
        {
            return await _khachHangService.GenerateTokenAsync(id);
        }

        [HttpGet("check-token")]
        public async Task<IActionResult> CheckToken(string token)
        {
            return await _khachHangService.CheckTokenAsync(token);
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

        [HttpPut("update-info/{id}")]
        public async Task<IActionResult> UpdateInfo(int id, [FromBody] ThongTinKHViewModel model)
        {
            return await _khachHangService.UpdateInfoAsync(id, model);
        }

        [HttpGet("captcha")]
        public async Task<IActionResult> CheckCaptcha(string captcha)
        {
            var secret = "6LdOL6kUAAAAABK06FPtg5nTLrng7dhUipQffqTj";
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, captcha));
            var obj = JObject.Parse(result);
            return Ok(obj);
        }
    }
}