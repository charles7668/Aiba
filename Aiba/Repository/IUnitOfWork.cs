﻿using Aiba.Model;

namespace Aiba.Repository
{
    public interface IUnitOfWork
    {
        public Task<IEnumerable<MediaInfo>>
            GetMediaInfos(string userId, string libraryName, int page, int countPerPage);

        public Task<MediaInfo?> GetMediaInfo(string userId, string libraryName, string imagePath);
        public Task AddLibraryInfoByUserIdAsync(string userId, LibraryInfo libraryInfo);
        public Task<IEnumerable<LibraryInfo>> GetLibraryInfosByUserIdAsync(string userId);
        public Task RemoveLibraryByUserIdAsync(string userId, LibraryInfo libraryInfo);
        public Task AddMediaInfoToLibraryAsync(string userId, string libraryName, MediaInfo mediaInfo);
        public Task<LibraryInfo?> GetLibraryInfo(string userId, string libraryName);
        public Task<bool> HasMediaInfoByImageUrl(string imageUrl);
        public Task RemoveMediaInfo(string userId, string libraryName, MediaInfo mediaInfo);
        public Task<UserSetting> GetUserSettingAsync(string userId);
        public Task UpdateUserSettingAsync(string userId, UserSetting userSetting);
    }
}