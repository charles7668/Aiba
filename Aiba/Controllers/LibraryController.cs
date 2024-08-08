using Aiba.Model;
using Aiba.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Aiba.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryController : Controller
    {
        public LibraryController(IUnitOfWork unitOfWork, ILogger<LibraryController> logger,
            SignInManager<IdentityUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _signInManager = signInManager;
        }

        private readonly ILogger<LibraryController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        [HttpPost]
        public async Task<IActionResult> AddLibrary(LibraryInfo libraryInfo)
        {
            string? id = _signInManager.UserManager.GetUserId(User);
            if (id == null)
            {
                return Unauthorized();
            }

            // check directory exist
            if (!Directory.Exists(libraryInfo.Path))
            {
                return BadRequest("Directory not exist");
            }

            _logger.LogInformation("LibraryController.AddLibraryInfoByUserId called by User : {UserId}", id);
            try
            {
                await _unitOfWork.AddLibraryInfoByUserIdAsync(id, libraryInfo);
            }
            catch (Exception e)
            {
                _logger.LogError("AddLibraryInfoByUserIdAsync failed : {Exception}", e.ToString());
                return StatusCode(500, e.Message);
            }

            return Ok();
        }

        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<LibraryInfo>>> GetLibraries()
        {
            string? id = _signInManager.UserManager.GetUserId(User);
            if (id == null)
            {
                return Unauthorized();
            }

            _logger.LogInformation("LibraryController.GetLibraryInfosByUserId called with userId: {userId}", id);
            IEnumerable<LibraryInfo> libraryInfos = await _unitOfWork.GetLibraryInfosByUserIdAsync(id);
            return Ok(libraryInfos);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveLibrary(LibraryInfo libraryInfo)
        {
            string? userId = _signInManager.UserManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();
            _logger.LogInformation("LibraryController.RemoveLibraryInfoByUserId called by User : {UserId}", userId);
            try
            {
                await _unitOfWork.RemoveLibraryByUserIdAsync(userId, libraryInfo);
            }
            catch (Exception e)
            {
                _logger.LogError("RemoveLibraryInfoByUserIdAsync failed : {Exception}", e.ToString());
                return StatusCode(500, e.Message);
            }

            return Ok();
        }
    }
}