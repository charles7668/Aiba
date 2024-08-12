using Aiba.Model;
using Aiba.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Aiba.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingController(
        ILogger<SettingController> logger,
        IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<UserSetting>> GetSetting()
        {
            string? userId = userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();
            logger.LogInformation("GetUserSetting called by {UserId}", userId);
            UserSetting userSetting;
            try
            {
                userSetting = await unitOfWork.GetUserSettingAsync(userId);
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetUserSetting failed");
                throw;
            }

            return userSetting;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSetting(UserSetting setting)
        {
            string? userId = userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();
            logger.LogInformation("UpdateUserSetting called by {UserId}", userId);
            try
            {
                await unitOfWork.UpdateUserSettingAsync(userId, setting);
            }
            catch (Exception e)
            {
                logger.LogError(e, "UpdateUserSetting failed");
                throw;
            }

            return Ok();
        }
    }
}