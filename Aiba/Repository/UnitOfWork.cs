using Aiba.DataMapping;
using Aiba.Entities;
using Aiba.Model;

namespace Aiba.Repository
{
    public class UnitOfWork(AppDBContext context) : IUnitOfWork
    {
        private ILibraryRepository LibraryRepository { get; } = new LibraryRepository(context);
        private IMediaInfoRepository MediaInfoRepository { get; } = new MediaInfoRepository(context);
        private IUserSettingRepository UserSettingRepository { get; } = new UserSettingRepository(context);

        public async Task<IEnumerable<MediaInfo>> GetMediaInfos(string userId, string libraryName, int page,
            int countPerPage)
        {
            LibraryEntity? libraryEntity = await LibraryRepository.GetLibraryEntity(userId, libraryName);
            if (libraryEntity == null)
                throw new ArgumentException("library not found");
            IEnumerable<MediaInfoEntity> mediaEntities =
                await MediaInfoRepository.GetMediaInfosByLibraryAsync(libraryEntity, page, countPerPage);
            List<MediaInfo> result = [];
            foreach (MediaInfoEntity mediaEntity in mediaEntities)
            {
                var info = new MediaInfo();
                MediaInfoEntityMapping.Map(mediaEntity, info);
                result.Add(info);
            }

            return result;
        }

        public async Task<MediaInfo?> GetMediaInfo(string userId, string libraryName, string imagePath)
        {
            LibraryEntity? libraryEntity = await LibraryRepository.GetLibraryEntity(userId, libraryName);
            if (libraryEntity == null)
                return null;
            MediaInfoEntity? mediaInfoEntity = await MediaInfoRepository.GetMediaInfo(libraryEntity.Id, imagePath);
            if (mediaInfoEntity == null)
                return null;
            var mediaInfo = new MediaInfo();
            MediaInfoEntityMapping.Map(mediaInfoEntity, mediaInfo);
            return mediaInfo;
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

        public async Task AddMediaInfoToLibraryAsync(string userId, string libraryName, MediaInfo mediaInfo)
        {
            LibraryEntity? libraryEntity = await LibraryRepository.GetLibraryEntity(userId, libraryName);
            if (libraryEntity == null)
                throw new ArgumentException("library not found");
            MediaInfoEntity? mediaEntity = await MediaInfoRepository.GetMediaInfo(libraryEntity.Id, mediaInfo.Url);
            if (mediaEntity != null)
                throw new ArgumentException("media already exists");
            mediaEntity = new MediaInfoEntity();
            MediaInfoEntityMapping.MapFrom(mediaEntity, mediaInfo);
            mediaEntity.LibraryId = libraryEntity.Id;
            mediaEntity.Library = libraryEntity;
            await MediaInfoRepository.AddMediaInfo(mediaEntity);
            await context.SaveChangesAsync();
        }

        public async Task<LibraryInfo?> GetLibraryInfo(string userId, string libraryName)
        {
            LibraryEntity? libraryEntity = await LibraryRepository.GetLibraryEntity(userId, libraryName);
            if (libraryEntity == null)
                throw new ArgumentException("library not found");
            var libraryInfo = new LibraryInfo();
            LibraryEntityMapping.Map(libraryEntity, libraryInfo);
            return libraryInfo;
        }

        public Task<bool> HasMediaInfoByImageUrl(string imageUrl)
        {
            return MediaInfoRepository.HasMediaInfoByImageUrl(imageUrl);
        }

        public async Task RemoveMediaInfo(string userId, string libraryName, MediaInfo mediaInfo)
        {
            var mediaEntity = new MediaInfoEntity();
            MediaInfoEntityMapping.MapFrom(mediaEntity, mediaInfo);
            LibraryEntity? libraryEntity = await LibraryRepository.GetLibraryEntity(userId, libraryName);
            if (libraryEntity == null)
                return;
            mediaEntity = await MediaInfoRepository.GetMediaInfo(libraryEntity.Id, mediaEntity.Url);
            if (mediaEntity == null)
                return;
            await MediaInfoRepository.Remove(userId, mediaEntity);
            await context.SaveChangesAsync();
        }

        public async Task<UserSetting> GetUserSettingAsync(string userId)
        {
            UserSettingEntity? userSettingEntity = await UserSettingRepository.GetAsync(userId);
            if (userSettingEntity == null)
            {
                userSettingEntity = new UserSettingEntity
                {
                    UserId = userId
                };
                await UserSettingRepository.UpdateAsync(userSettingEntity);
            }

            var result = new UserSetting();
            UserSettingEntityMapping.Map(userSettingEntity, result);
            return result;
        }

        public async Task UpdateUserSettingAsync(string userId, UserSetting userSetting)
        {
            UserSettingEntity? userSettingEntity = await UserSettingRepository.GetAsync(userId);
            if (userSettingEntity == null)
            {
                userSettingEntity = new UserSettingEntity();
            }

            UserSettingEntityMapping.MapFrom(userSettingEntity, userSetting);
            userSettingEntity.UserId = userId;

            await UserSettingRepository.UpdateAsync(userSettingEntity);
            await context.SaveChangesAsync();
        }
    }
}