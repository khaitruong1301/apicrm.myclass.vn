using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoloDevApp.Service.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/file")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("")]
        public async Task<IActionResult> File()
        {
            try
            {
                IFormFileCollection files = Request.Form.Files;
                List<string> result = await _fileService.UploadFileAsync(files);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("image")]
        public async Task<IActionResult> Image()
        {
            try
            {
                IFormFileCollection files = Request.Form.Files;
                List<string> result = await _fileService.UploadImageAsync(files);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("cmnd")]
        public async Task<IActionResult> Cmnd()
        {
            try
            {
                IFormFileCollection files = Request.Form.Files;
                string result = await _fileService.UploadCmndAsync(files[0]);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("video")]
        public async Task<IActionResult> Video()
        {
            try
            {
                IFormFileCollection files = Request.Form.Files;
                List<string> result = await _fileService.UploadVideoAsync(files);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ftp-video")]
        public async Task<IActionResult> FtpVideo()
        {
            try
            {
                IFormFileCollection files = Request.Form.Files;
                List<string> result = await _fileService.UploadVideoFTPAsync(files);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // LẤY URL VIDEO FTP
        [HttpGet("ftp-video/{fileName}")]
        public async Task<IActionResult> FtpVideo(string fileName)
        {
            try
            {
                // Trả về url của video FTP
                string result = await _fileService.GetUrlFTPVideoAsync(fileName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}