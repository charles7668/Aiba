using Aiba.Helpers;
using Aiba.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Aiba.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController(IUnitOfWork unitOfWork, ILogger<ImageController> logger) : Controller
    {
        [HttpGet("{mediaUrl}/{imagePath}")]
        [AllowAnonymous]
        [ResponseCache(Duration = 60 * 2, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> GetCoverImage(string mediaUrl, string imagePath)
        {
            imagePath = HttpUtility.UrlDecode(imagePath).TrimFileProtocol();
            mediaUrl = HttpUtility.UrlDecode(mediaUrl).TrimFileProtocol();

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            // check if the file is in mediainfo table
            try
            {
                // for security, check if the image path is in the media path
                // and media path is in the database
                if (!imagePath.StartsWith(mediaUrl))
                    return NotFound();
                if (!await unitOfWork.HasMediaInfoByImageUrl(mediaUrl.ToFileProtocol()))
                    return NotFound();
            }
            catch (Exception e)
            {
                logger.LogError("Error while checking image path in database: {Error}", e.Message);
                return NotFound();
            }

            FileStream image = System.IO.File.OpenRead(imagePath);
            string contentType = ImageHelper.GetImageContentType(Path.GetExtension(imagePath));
            return File(image, contentType);
        }
    }
}