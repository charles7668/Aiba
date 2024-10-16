using Aiba.Model;

namespace Aiba.Repository
{
    public interface IUnitOfWork
    {
        public Task<IEnumerable<MediaInfo>>
            GetMediaInfos(string userId, string libraryName, int page, int countPerPage);
        public Task<int> GetMediaInfosCount(string userId, string libraryName);
        public Task<MediaInfo?> GetMediaInfoAsync(string userId, string libraryName, string mediaPath);
        public Task AddLibraryInfoByUserIdAsync(string userId, LibraryInfo libraryInfo);
        public Task<IEnumerable<LibraryInfo>> GetLibraryInfosByUserIdAsync(string userId);
        public Task RemoveLibraryByUserIdAsync(string userId, LibraryInfo libraryInfo);
        public Task AddMediaInfoToLibraryAsync(string userId, string libraryName, MediaInfo mediaInfo);
        public Task<LibraryInfo?> GetLibraryInfo(string userId, string libraryName);
        public Task<bool> HasMediaInfoByMediaUrl(string mediaUrl);
        public Task RemoveMediaInfo(string userId, string libraryName, MediaInfo mediaInfo);
        public Task<UserSetting> GetUserSettingAsync(string userId);
        public Task UpdateUserSettingAsync(string userId, UserSetting userSetting);
    }
}