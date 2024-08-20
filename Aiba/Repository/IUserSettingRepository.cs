using Aiba.Entities;

namespace Aiba.Repository
{
    public interface IUserSettingRepository
    {
        public Task<UserSettingEntity?> GetAsync(string userId);

        public Task UpdateAsync(UserSettingEntity userSetting);
    }
}