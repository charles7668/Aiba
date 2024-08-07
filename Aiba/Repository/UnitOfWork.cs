using Aiba.Entities;
using Aiba.Model;

namespace Aiba.Repository
{
    public class UnitOfWork(AppDBContext context) : IUnitOfWork
    {
        private ILibraryRepository LibraryRepository { get; } = new LibraryRepository(context);

        public async Task AddLibraryInfoByUserIdAsync(string userId, LibraryInfo libraryInfo)
        {
            await LibraryRepository.AddLibraryEntityByUserIdAsync(userId, new LibraryEntity
            {
                Name = libraryInfo.Name,
                Path = libraryInfo.Path,
                Type = libraryInfo.Type
            });
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LibraryInfo>> GetLibraryInfosByUserIdAsync(string userId)
        {
            IEnumerable<LibraryEntity> entities = await LibraryRepository.GetLibraryEntitiesByUserIdAsync(userId);
            IEnumerable<LibraryInfo> result = entities.Select(x => new LibraryInfo
            {
                Name = x.Name,
                Path = x.Path,
                Type = x.Type
            });
            return result;
        }

        public async Task RemoveLibraryByUserIdAsync(string userId, LibraryInfo libraryInfo)
        {
            await LibraryRepository.RemoveLibraryEntityByUserIdAsync(userId, new LibraryEntity
            {
                Name = libraryInfo.Name
            });
            await context.SaveChangesAsync();
        }
    }
}