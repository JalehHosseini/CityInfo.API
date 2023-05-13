using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers;

[Route("api/files")]
[ApiController]
public class FileController : ControllerBase
{
    FileExtensionContentTypeProvider fileExtensionContentTypeProvider;
    public FileController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
    {
        this.fileExtensionContentTypeProvider = fileExtensionContentTypeProvider;
    }

    [HttpGet("{fileId}")]
    public ActionResult GetFile(string fileId)
    {
        string pathToFile = "pic.rar";
        if (!System.IO.File.Exists(pathToFile))
        {
            return NotFound();
        }
        var bytes = System.IO.File.ReadAllBytes(pathToFile);
        if (!fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
        {
            contentType = "application/octet-stream";
        }
        return File(bytes, contentType, Path.GetFileName(pathToFile));
    }
}
