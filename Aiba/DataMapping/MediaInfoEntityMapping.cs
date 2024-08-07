using Aiba.Entities;
using Aiba.Extensions;
using Aiba.Model;
using Aiba.Model.Extensions;

namespace Aiba.DataMapping
{
    public static class MediaInfoEntityMapping
    {
        public static void Map(MediaInfoEntity input, MediaInfo output)
        {
            output.Name = input.Name;
            output.Type = input.Type.GetMediaTypeString();
            output.Description = input.Description;
            output.Url = input.Url;
            output.ImageUrl = input.ImageUrl;
            output.Status = input.Status;
            output.ProviderName = input.ProviderName;
            output.ProviderUrl = input.ProviderUrl;
            output.Author = input.Author;
            List<string> genres = [];
            foreach (GenreEntity genre in input.Genres)
            {
                GenreEntityMapping.Map(genre, out string genreString);
                genres.Add(genreString);
            }

            output.Genres = genres.ToArray();
            List<string> tags = [];
            foreach (TagEntity tagEntity in input.Tags)
            {
                TagEntityMapping.Map(tagEntity, out string tag);
                tags.Add(tag);
            }

            output.Tags = tags.ToArray();
            List<ChapterInfo> chapters = [];
            foreach (ChapterEntity chapterEntity in input.Chapters)
            {
                var chapterInfo = new ChapterInfo();
                ChapterEntityMapping.Map(chapterEntity, chapterInfo);
                chapters.Add(chapterInfo);
            }

            output.Chapters = chapters.ToArray();
        }

        public static void MapFrom(MediaInfoEntity output, MediaInfo input)
        {
            output.Name = input.Name;
            output.Type = input.Type.GetFlag();
            output.Description = input.Description;
            output.Url = input.Url;
            output.ImageUrl = input.ImageUrl;
            output.Status = input.Status;
            output.ProviderName = input.ProviderName;
            output.ProviderUrl = input.ProviderUrl;
            output.Author = input.Author;
            List<GenreEntity> genres = [];
            foreach (string genre in input.Genres)
            {
                var genreEntity = new GenreEntity();
                GenreEntityMapping.MapFrom(genreEntity, genre);
                genres.Add(genreEntity);
            }

            output.Genres = genres.ToArray();
            List<TagEntity> tags = [];
            foreach (string tag in input.Tags)
            {
                var tagEntity = new TagEntity();
                TagEntityMapping.MapFrom(tagEntity, tag);
                tags.Add(tagEntity);
            }

            output.Tags = tags.ToArray();
            List<ChapterEntity> chapters = [];
            foreach (ChapterInfo chapterInfo in input.Chapters)
            {
                var chapterEntity = new ChapterEntity();
                ChapterEntityMapping.MapFrom(chapterEntity, chapterInfo);
                chapters.Add(chapterEntity);
            }

            output.Chapters = chapters.ToArray();
        }
    }
}