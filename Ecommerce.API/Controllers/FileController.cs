using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly string _filePath;
        public FileController(string filePath)
        {
            _filePath = filePath;
        }
        [HttpGet]
        public FileContentResult DowloadFileConfiguration()
        {
            return File(System.IO.File.ReadAllBytes(_filePath), "application/octet-stream", "template_configuration.xlsx");
        }
    }
}
