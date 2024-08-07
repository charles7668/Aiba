using Aiba.Entities;

namespace Aiba.Repository
{
    public interface IMediaInfoRepository
    {
        public Task AddMediaInfo(MediaInfoEntity mediaInfo);

        public Task<IEnumerable<MediaInfoEntity>> GetMediaInfosByLibraryAsync(LibraryEntity libraryEntity);
    }
}