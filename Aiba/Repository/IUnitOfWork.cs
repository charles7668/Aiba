using Aiba.Model;

namespace Aiba.Repository
{
    public interface IUnitOfWork
    {
        public Task<IEnumerable<MediaInfo>> GetMediaInfosFromLibrary(string userId, LibraryInfo libraryInfo);
        public Task AddLibraryInfoByUserIdAsync(string userId, LibraryInfo libraryInfo);
        public Task<IEnumerable<LibraryInfo>> GetLibraryInfosByUserIdAsync(string userId);
        public Task RemoveLibraryByUserIdAsync(string userId, LibraryInfo libraryInfo);
        public Task AddMediaInfoToLibraryAsync(string userId, LibraryInfo libraryInfo, MediaInfo mediaInfo);
    }
}