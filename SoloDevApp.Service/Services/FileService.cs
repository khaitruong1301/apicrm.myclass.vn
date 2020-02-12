﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SoloDevApp.Service.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface IFileService
    {
        Task<List<string>> UploadFileAsync(IFormFileCollection files);
        Task<List<string>> UploadImageAsync(IFormFileCollection files);
        Task<List<string>> UploadVideoAsync(IFormFileCollection files);
        Task<List<string>> UploadVideoFTPAsync(IFormFileCollection files);
        Task<string> GetUrlFTPVideoAsync(string fileName);
    }

    public class FileService : IFileService
    {
        private readonly IFtpSettings _ftpSettings;
        public FileService(IFtpSettings ftpSettings)
        {
            _ftpSettings = ftpSettings;
        }

        public async Task<string> GetUrlFTPVideoAsync(string fileName)
        {
            string link = _ftpSettings.UrlServer + fileName.Split(".")[0];
            var url = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    url = readStream.ReadToEnd();
                    url = url.Replace("<!--SCRIPT GENERATED BY SERVER! PLEASE REMOVE-->", "♥").Split('♥')[0].Trim();

                    response.Close();
                    readStream.Close();
                }

                return url;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string>> UploadFileAsync(IFormFileCollection files)
        {
            List<string> list = new List<string>();
            foreach (var file in files)
            {
                if (file != null && file.Length != 0)
                {
                    string filePath = await SaveFileAsync(file, "files");
                    list.Add(filePath);
                }
            }
            return list;
        }

        public async Task<List<string>> UploadImageAsync(IFormFileCollection files)
        {
            List<string> list = new List<string>();
            foreach (var file in files)
            {
                if (file != null && file.Length != 0)
                {
                    string filePath = await SaveFileAsync(file, "images");
                    list.Add(filePath);
                }
            }
            return list;
        }

        public async Task<List<string>> UploadVideoAsync(IFormFileCollection files)
        {
            List<string> list = new List<string>();
            foreach (var file in files)
            {
                if (file != null && file.Length != 0)
                {
                    string filePath = await SaveFileAsync(file, "videos");
                    list.Add(filePath);
                }
            }
            return list;
        }

        public async Task<List<string>> UploadVideoFTPAsync(IFormFileCollection files)
        {
            try
            {
                List<string> list = new List<string>();
                foreach (var file in files)
                {
                    if (file != null && file.Length != 0)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos", file.FileName);
                        // Lưu tạm video vào máy
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        string videoPath = "wwwroot/videos/" + file.FileName;
                        // clean force up stream
                        GC.Collect();
                        // Lưu video lên máy chủ FTP
                        SaveVideoFTP(videoPath, file.FileName);
                        list.Add(file.FileName);
                        // Xóa video lưu tạm sau khi hoàn thành lưu video lên máy FTP
                        if (File.Exists(videoPath))
                        {
                            File.Delete(videoPath);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            var folderName = Path.Combine("wwwroot", folder, DateTime.Now.ToString("yyyyMMdd"));
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            // Tạo folder nếu chưa tồn tại
            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);

            // Lấy tên file
            string fileName = Path.GetFileName(file.FileName);

            // Tạo đường dẫn tới file
            string path = Path.Combine(pathToSave, fileName);

            // Kiểm tra xem file bị trùng không
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{folder}/{DateTime.Now.ToString("yyyyMMdd")}/{fileName}";
        }

        private bool SaveVideoFTP(string path, string fileName)
        {
            //Chuyển file sang byte
            byte[] fileBytes = File.ReadAllBytes(path);
            try
            {
                //Create FTP Request.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_ftpSettings.IP + fileName);
                // Xóa video nếu đã tồn tại
                if (request.ContentLength != 0)
                {
                    request.Method = WebRequestMethods.Ftp.DeleteFile;
                    request.Credentials = new NetworkCredential(_ftpSettings.UserName, _ftpSettings.Password);
                    request.GetResponse();
                }

                // Thực hiện upload video
                request.Method = WebRequestMethods.Ftp.UploadFile;
                //Enter FTP Server credentials.
                request.Credentials = new NetworkCredential(_ftpSettings.UserName, _ftpSettings.Password);
                request.ContentLength = fileBytes.Length;
                request.UsePassive = true;
                request.UseBinary = true;
                request.ServicePoint.ConnectionLimit = fileBytes.Length;
                request.EnableSsl = false;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileBytes, 0, fileBytes.Length);
                    requestStream.Close();
                }

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                response.Close();
                return true;
            }
            catch (WebException ex)
            {
                throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
            }
        }

    }
}