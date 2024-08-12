using Aiba.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aiba.Repository
{
    public class UserSettingRepository(AppDBContext context) : IUserSettingRepository
    {
        public async Task<UserSettingEntity?> GetAsync(string userId)
        {
            return await context.UserSettings.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public Task UpdateAsync(UserSettingEntity userSetting)
        {
            context.UserSettings.Update(userSetting);
            return context.SaveChangesAsync();
        }
    }
}