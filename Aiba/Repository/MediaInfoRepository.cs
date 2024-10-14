using Aiba.Entities;
using Aiba.Model;
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

        public Task<bool> HasMediaInfoByImageUrl(string imagePath)
        {
            return Task.FromResult(context.MediaInfos.Any(x => x.Url == imagePath));
        }

        public Task Remove(string userId, MediaInfoEntity entity)
        {
            context.MediaInfos.Remove(entity);
            return Task.CompletedTask;
        }
    }
}