﻿using Aiba.Entities;
using Aiba.Model;

namespace Aiba.Repository
{
    public interface IMediaInfoRepository
    {
        public Task AddMediaInfo(MediaInfoEntity mediaInfo);

        public Task<IEnumerable<MediaInfoEntity>> GetMediaInfosByLibraryAsync(LibraryEntity libraryEntity, int page,
            int countPerPage);

        public Task<MediaInfoEntity?> GetMediaInfo(int libraryId, string mediaPath);

        public Task<bool> HasMediaInfoByMediaUrl(string mediaUrl);

        public Task Remove(string userId, MediaInfoEntity entity);

        public Task<int> Count(string userId , LibraryEntity libraryEntity);
    }
}