using Aiba.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aiba.Repository
{
    public class MediaInfoRepository(AppDBContext context) : IMediaInfoRepository
    {
        public async Task AddMediaInfo(MediaInfoEntity mediaInfo)
        {
            await context.MediaInfos.AddAsync(mediaInfo);
        }

        public Task<IEnumerable<MediaInfoEntity>> GetMediaInfosByLibraryAsync(LibraryEntity libraryEntity,
            int page,
            int countPerPage)
        {
            IQueryable<MediaInfoEntity> result = context.MediaInfos.Include(x => x.Chapters)
                .Where(x => x.LibraryId == libraryEntity.Id)
                .Skip((page - 1) * countPerPage)
                .Take(countPerPage);
            return Task.FromResult<IEnumerable<MediaInfoEntity>>(result);
        }

        public async Task<MediaInfoEntity?> GetMediaInfo(int libraryId, string mediaPath)
        {
            MediaInfoEntity? mediaInfoEntity =
                await context.MediaInfos.Include(x => x.Chapters)
                    .FirstOrDefaultAsync(x => x.LibraryId == libraryId && x.Url == mediaPath);
            return mediaInfoEntity;
        }

        public Task<bool> HasMediaInfoByMediaUrl(string mediaUrl)
        {
            return Task.FromResult(context.MediaInfos.Any(x => x.Url == mediaUrl));
        }

        public Task Remove(string userId, MediaInfoEntity entity)
        {
            context.MediaInfos.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<int> Count(string userId, LibraryEntity libraryEntity)
        {
            return Task.FromResult(context.MediaInfos.Count(x => x.LibraryId == libraryEntity.Id));
        }
    }
}