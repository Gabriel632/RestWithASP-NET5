using Microsoft.AspNetCore.Http;
using RestWithASPNET5.Data.VO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestWithASPNET5.Business
{
    public interface IFileBussines
    {
        public byte[] GetFile(string fileName);
        public Task<FileDetailVO> SaveFileToDisk(IFormFile file);
        public Task<List<FileDetailVO>> SaveFileToDisk(IList<IFormFile> files);
    }
}
