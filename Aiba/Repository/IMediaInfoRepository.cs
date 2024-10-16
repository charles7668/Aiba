using Aiba.Entities;
using System.Linq.Expressions;

namespace Aiba.Repository
{
    public interface IMediaInfoRepository
    {
        public Task AddMediaInfo(MediaInfoEntity mediaInfo);

        public Task<IEnumerable<MediaInfoEntity>> GetMediaInfosByLibraryAsync(LibraryEntity libraryEntity, int page,
            int countPerPage);

        public Task<IQueryable<MediaInfoEntity>> EnumerateMediaInfos(
            Expression<Func<MediaInfoEntity, bool>> queryExpression);

        public Task<MediaInfoEntity?> GetMediaInfo(int libraryId, string mediaPath);
        public Task<MediaInfoEntity?> GetMediaInfoAsync(Expression<Func<MediaInfoEntity, bool>> queryExpression);

        public Task<bool> HasMediaInfoByMediaUrl(string mediaUrl);

        public Task Remove(string userId, MediaInfoEntity entity);

        public Task<int> Count(string userId , LibraryEntity libraryEntity);
    }
}