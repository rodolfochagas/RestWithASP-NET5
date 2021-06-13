using Microsoft.AspNetCore.Http;
using RestWithASPNET.Data.VO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithASPNET.Business.Implementations
{
    public class FileBusiness : IFileBusiness
    {
        private readonly string _basePath;
        private readonly IHttpContextAccessor _context;

        public FileBusiness(IHttpContextAccessor context)
        {
            _context = context;
            _basePath = Directory.GetCurrentDirectory() + "\\UploadDir\\";
        }

        public byte[] GetFile(string fileName)
        {
            var filePath = _basePath + fileName;
            return File.ReadAllBytes(filePath);
        }

        public async Task<FileDetailVO> SaveFileToDisk(IFormFile file)
        {
            var fileDetail = new FileDetailVO();
            var fileType = Path.GetExtension(file.FileName);
            var baseUrl = _context.HttpContext.Request.Host;

            if(fileType.ToLower() == ".pdf" || fileType.ToLower() == ".jpg" ||
                fileType.ToLower() == ".png" || fileType.ToLower() == ".jpeg"
                )
            {
                var docName = Path.GetFileName(file.FileName);
                if (file != null && file.Length > 0)
                {
                    fileDetail.DocumentName = docName;
                    fileDetail.DocumentType = fileType;
                    fileDetail.DocumentUrl = Path.Combine(baseUrl + "/api/file/v1/" + fileDetail.DocumentName);

                    var destination = Path.Combine(_basePath, "", docName);
                    using var stream = new FileStream(destination, FileMode.Create);
                    await file.CopyToAsync(stream);
                }
            }

            return fileDetail;
        }

        public async Task<List<FileDetailVO>> SaveFilesToDisk(IList<IFormFile> files)
        {
            var list = new List<FileDetailVO>();
            foreach(var file in files)
            {
                list.Add(await SaveFileToDisk(file));
            }
            return list;
        }


    }
}
