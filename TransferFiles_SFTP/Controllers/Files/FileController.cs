using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransferFiles_SFTP.Interfaces;

namespace TransferFiles_SFTP.Controllers.Files
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        public readonly ITransferDocumentService _service;
        public FileController(ITransferDocumentService service)
        {
            _service = service;
        }

        [HttpGet("GetDocumentFromLocalServer")]
        public async Task<IActionResult> Testing()
        {
            return Ok(await _service.TransferFromLocal());
        }
    }
}
