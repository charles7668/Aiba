using Aiba.DataMapping;
using Aiba.Entities;
using Aiba.Model;

namespace Aiba.Repository
{
    public class UnitOfWork(AppDBContext context) : IUnitOfWork
    {
        private ILibraryRepository LibraryRepository { get; } = new LibraryRepository(context);
        private IMediaInfoRepository MediaInfoRepository { get; } = new MediaInfoRepository(context);

        public async Task<IEnumerable<MediaInfo>> GetMediaInfosFromLibrary(string userId, LibraryInfo libraryInfo)
        {
            var libraryEntity = new LibraryEntity();
            LibraryEntityMapping.MapFrom(libraryEntity, libraryInfo);
            libraryEntity = await LibraryRepository.GetLibraryEntitiesByUserIdAndNameAsync(userId, libraryEntity);
            if (libraryEntity == null)
                throw new ArgumentException("library not found");
            IEnumerable<MediaInfoEntity> mediaEntities =
                await MediaInfoRepository.GetMediaInfosByLibraryAsync(libraryEntity);
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

        public async Task AddMediaInfoToLibraryAsync(string userId, LibraryInfo libraryInfo, MediaInfo mediaInfo)
        {
            var libraryEntity = new LibraryEntity();
            LibraryEntityMapping.MapFrom(libraryEntity, libraryInfo);
            libraryEntity = await LibraryRepository.GetLibraryEntitiesByUserIdAndNameAsync(userId, libraryEntity);
            if (libraryEntity == null)
                throw new ArgumentException("library not found");
            var mediaEntity = new MediaInfoEntity();
            MediaInfoEntityMapping.MapFrom(mediaEntity, mediaInfo);
            mediaEntity.LibraryId = libraryEntity.Id;
            mediaEntity.Library = libraryEntity;
            await MediaInfoRepository.AddMediaInfo(mediaEntity);
            await context.SaveChangesAsync();
        }

        public async Task<LibraryInfo> GetLibraryInfoByUserIdAndNameAsync(string userId, string name)
        {
            var libraryEntity = new LibraryEntity
            {
                UserId = userId,
                Name = name
            };
            libraryEntity = await LibraryRepository.GetLibraryEntitiesByUserIdAndNameAsync(userId, libraryEntity);
            if (libraryEntity == null)
                throw new ArgumentException("library not found");
            var libraryInfo = new LibraryInfo();
            LibraryEntityMapping.Map(libraryEntity, libraryInfo);
            return libraryInfo;
        }

        public Task<bool> HasMediaInfoByImagePath(string imagePath)
        {
            return MediaInfoRepository.HasMediaInfoByImagePath(imagePath);
        }
    }
}