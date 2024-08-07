using Aiba.Entities;

namespace Aiba.Repository
{
    public class MediaInfoRepository(AppDBContext context) : IMediaInfoRepository
    {
        public async Task AddMediaInfo(MediaInfoEntity mediaInfo)
        {
            await context.MediaInfos.AddAsync(mediaInfo);
        }
    }
}