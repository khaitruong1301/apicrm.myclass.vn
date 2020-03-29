using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.Utilities;
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
        private IFileService _fileService;

        public BaiTapController(IBaiTapService baiTapService,IFileService fileService)
        {
            _baiTapService = baiTapService;
            _fileService = fileService;
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

        //[HttpGet("{classId}/{exercire}")]
        //public async Task<IActionResult> GetByClassAndExercireId(int classId, string userId)
        //{
        //    return await _baiTapService.GetByClassAndExercireIdAsync(classId, userId);
        //}

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _baiTapService.GetPagingAsync(page, size, keywords);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BaiTapViewModel model)
        {
            if(model.NoiDung.Contains("pdf") || model.NoiDung.Contains("zip") || model.NoiDung.Contains("xlsl") || model.NoiDung.Contains("png") || model.NoiDung.Contains("jpg"))
            {
                model.NoiDung = FuncUtilities.BestLower(model.NoiDung);
                //string typeFile = model.NoiDung.Split('-')

            }
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
        
        
        [HttpPost("ThemBaiTap_UploadFile")]
        public async Task<IActionResult> ThemBaiTap_UploadFile()
        {
            var frmData = Request.Form;

            IFormFileCollection files = null;

            BaiTapViewModel model = new BaiTapViewModel();
            model.contructorBaiTapViewModel(int.Parse(frmData["id"]), frmData["tenBaiTap"], frmData["biDanh"], frmData["noiDung"], int.Parse(frmData["soNgayKichHoat"]), int.Parse(frmData["maLoTrinh"]), frmData["ghiChu"], false);

            if (frmData.Files.Count == 1)
            {
                files = Request.Form.Files;
                model.NoiDung = model.MaLoTrinh + "-" +FuncUtilities.BestLower(model.TenBaiTap) + "." + files[0].FileName.Split(".")[files[0].FileName.Split('.').Length-1];

               await _fileService.UploadFileAsync(files, model.NoiDung);
            }else
            {
                model.NoiDung = "";
            }
            return  await _baiTapService.InsertAsync(model);
        }


        [HttpPost("CapNhatBaiTapUploadFile")]
        public async Task<IActionResult> CapNhatBaiTapUploadFile()
        {

            var frmData = Request.Form;

            IFormFileCollection files = null;

            BaiTapViewModel model = new BaiTapViewModel();
            model.contructorBaiTapViewModel(int.Parse(frmData["id"]), frmData["tenBaiTap"], frmData["biDanh"], frmData["noiDung"], int.Parse(frmData["soNgayKichHoat"]), int.Parse(frmData["maLoTrinh"]), frmData["ghiChu"], false);

            if (frmData.Files.Count == 1)
            {
                files = Request.Form.Files;
                model.NoiDung = model.MaLoTrinh + "-" + FuncUtilities.BestLower(model.TenBaiTap) + "." + files[0].FileName.Split(".")[files[0].FileName.Split('.').Length - 1];

                await _fileService.UploadFileAsync(files, model.NoiDung);
            }
            else
            {
                model.NoiDung = "";

            }

            return await _baiTapService.UpdateAsync(model.Id, model);
        }

    }
}