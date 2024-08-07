using Aiba.Entities;

namespace Aiba.Repository
{
    public class MediaInfoRepository(AppDBContext context) : IMediaInfoRepository
    {
        public async Task AddMediaInfo(MediaInfoEntity mediaInfo)
        {
            await context.MediaInfos.AddAsync(mediaInfo);
        }

        public Task<IEnumerable<MediaInfoEntity>> GetMediaInfosByLibraryAsync(LibraryEntity libraryEntity)
        {
            return Task.FromResult<IEnumerable<MediaInfoEntity>>(
                context.MediaInfos.Where(x => x.LibraryId == libraryEntity.Id));
        }
    }
}