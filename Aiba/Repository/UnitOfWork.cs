using Aiba.DataMapping;
using Aiba.Entities;
using Aiba.Model;

namespace Aiba.Repository
{
    public class UnitOfWork(AppDBContext context) : IUnitOfWork
    {
        private ILibraryRepository LibraryRepository { get; } = new LibraryRepository(context);

        public async Task<IEnumerable<MediaInfo>> GetMediaInfosFromLibrary(string userId, LibraryInfo libraryInfo)
        {
            var libraryEntity = new LibraryEntity();
            LibraryEntityMapping.MapFrom(libraryEntity, libraryInfo);
            IEnumerable<MediaInfoEntity> mediaEntities =
                await LibraryRepository.GetMediasByUserIdAndLibraryNameAsync(userId, libraryEntity);
            List<MediaInfo> result = [];
            foreach (MediaInfoEntity mediaEntity in mediaEntities)
            {
                var info = new MediaInfo();
                MediaInfoEntityMapping.Map(mediaEntity, info);
                result.Add(info);
            }

            return result;
        }

        public async Task AddLibraryInfoByUserIdAsync(string userId, LibraryInfo libraryInfo)
        {
            var libraryEntity = new LibraryEntity();
            LibraryEntityMapping.MapFrom(libraryEntity, libraryInfo);
            await LibraryRepository.AddLibraryEntityByUserIdAsync(userId, libraryEntity);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LibraryInfo>> GetLibraryInfosByUserIdAsync(string userId)
        {
            IEnumerable<LibraryEntity> entities = await LibraryRepository.GetLibraryEntitiesByUserIdAsync(userId);
            IEnumerable<LibraryInfo> result = entities.Select(x =>
            {
                var info = new LibraryInfo();
                LibraryEntityMapping.Map(x, info);
                return info;
            });
            return result;
        }

        public async Task RemoveLibraryByUserIdAsync(string userId, LibraryInfo libraryInfo)
        {
            var libraryEntity = new LibraryEntity();
            LibraryEntityMapping.MapFrom(libraryEntity, libraryInfo);
            await LibraryRepository.RemoveLibraryEntityByUserIdAsync(userId, libraryEntity);
            await context.SaveChangesAsync();
        }
    }
}