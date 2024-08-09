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
            IQueryable<MediaInfoEntity> result = context.MediaInfos.Where(x => x.LibraryId == libraryEntity.Id)
                .Skip((page - 1) * countPerPage)
                .Take(countPerPage);
            return Task.FromResult<IEnumerable<MediaInfoEntity>>(result);
        }

        public async Task<MediaInfoEntity?> GetMediaInfo(int libraryId, string mediaPath)
        {
            MediaInfoEntity? mediaInfoEntity =
                await context.MediaInfos.FirstOrDefaultAsync(x => x.LibraryId == libraryId && x.Url == mediaPath);
            return mediaInfoEntity;
        }

        public Task<bool> HasMediaInfoByImagePath(string imagePath)
        {
            return Task.FromResult(context.MediaInfos.Any(x => x.ImageUrl == imagePath));
        }

        public Task Remove(string userId, MediaInfoEntity entity)
        {
            context.MediaInfos.Remove(entity);
            return Task.CompletedTask;
        }
    }
}