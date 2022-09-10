using Microsoft.AspNetCore.Http;
using RestWithASPNET5.Data.VO;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RestWithASPNET5.Business.Implementations
{
    public class FileBussinesImplementation : IFileBussines
    {
        private readonly string _basePath;
        private readonly IHttpContextAccessor _context;

        public FileBussinesImplementation(IHttpContextAccessor context)
        {
            _basePath = Directory.GetCurrentDirectory() + "\\UploadDir\\";
            _context = context;
        }

        public byte[] GetFile(string fileName)
        {
            var filePath = _basePath + fileName;
            return File.ReadAllBytes(filePath);
        }

        public async Task<FileDetailVO> SaveFileToDisk(IFormFile file)
        {
            FileDetailVO fileDetail = new FileDetailVO();

            var fileType = Path.GetExtension(file.FileName);
            var baseUrl = _context.HttpContext.Request.Host;

            if (fileType.ToLower() == ".pdf" || 
                fileType.ToLower() == ".jpg" ||
                fileType.ToLower() == ".png" ||
                fileType.ToLower() == ".jpeg"
            )
            {
                var docName = Path.GetFileName(file.FileName);
                if (file != null && file.Length > 0)
                {
                    var destination = Path.Combine(_basePath, "", docName);
                    fileDetail.DocumentName = docName;
                    fileDetail.DocType = fileType;
                    fileDetail.DocUrl = Path.Combine(baseUrl + "/api/file/v1/" + fileDetail.DocumentName);

                    using var stream = new FileStream(destination, FileMode.Create);
                    await file.CopyToAsync(stream);
                }
            }

            return fileDetail;
        }

        public async Task<List<FileDetailVO>> SaveFileToDisk(IList<IFormFile> files)
        {
            List<FileDetailVO> list = new();

            foreach (var file in files)
            {
                list.Add(await SaveFileToDisk(file));
            }

            return list;
        }
    }
}
