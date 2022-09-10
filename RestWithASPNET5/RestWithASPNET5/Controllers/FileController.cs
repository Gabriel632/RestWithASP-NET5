using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET5.Business;
using RestWithASPNET5.Data.VO;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;

namespace RestWithASPNET5.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/[controller]/v{version:ApiVersion}")]
    public class FileController : ControllerBase
    {
        private readonly IFileBussines _fileBussines;

        public FileController(IFileBussines fileBussines)
        {
            _fileBussines = fileBussines;
        }

        [HttpGet("downloadfile/{fileName}")]
        [ProducesResponseType(200, Type = typeof(byte[]))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> GetFileAsync(string fileName)
        {
            byte[] buffer = _fileBussines.GetFile(fileName);
            if (buffer != null)
            {
                HttpContext.Response.ContentType = 
                    $"application/{Path.GetExtension(fileName).Replace(".", "")}";
                HttpContext.Response.Headers.Add("content-length", buffer.Length.ToString());
                await HttpContext.Response.Body.WriteAsync(buffer);
            }
            return new ContentResult();
        }

        [HttpPost("uploadfile")]
        [ProducesResponseType(200, Type = typeof(FileDetailVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> UploadOneFile([FromForm] IFormFile file)
        {
            FileDetailVO detail = await _fileBussines.SaveFileToDisk(file);
            return Ok(detail);
        }

        [HttpPost("uploadMultipleFiles")]
        [ProducesResponseType(200, Type = typeof(List<FileDetailVO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> UploadManyFiles([FromForm] List<IFormFile> files)
        {
            List<FileDetailVO> details = await _fileBussines.SaveFileToDisk(files);
            return Ok(details);
        }
    }
}
