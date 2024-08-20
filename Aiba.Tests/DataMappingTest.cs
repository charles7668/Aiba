using Aiba.DataMapping;
using Aiba.Entities;
using Aiba.Enums;
using Aiba.Model;
using Aiba.Model.Extensions;

namespace Aiba.Tests
{
    [TestClass]
    public class DataMappingTest
    {
        [TestMethod]
        public void TestChapterInfoEntityMapping()
        {
            var chapterEntity = new ChapterEntity
            {
                Title = "test-title",
                Url = "http://test.com"
            };
            var chapterInfo = new ChapterInfo();
            ChapterEntityMapping.Map(chapterEntity, chapterInfo);
            Assert.AreEqual(chapterEntity.Title, chapterInfo.ChapterName);
            Assert.AreEqual(chapterEntity.Url, chapterInfo.ChapterUrl);
            chapterInfo.ChapterName = "new-test-title";
            chapterInfo.ChapterUrl = "http://new-test.com";
            ChapterEntityMapping.MapFrom(chapterEntity, chapterInfo);
            Assert.AreEqual(chapterInfo.ChapterName, chapterEntity.Title);
            Assert.AreEqual(chapterInfo.ChapterUrl, chapterEntity.Url);
        }

        [TestMethod]
        public void TestGenreEntityMapping()
        {
            var genreEntity = new GenreEntity
            {
                Name = "Test"
            };

            GenreEntityMapping.Map(genreEntity, out string genre);

            Assert.AreEqual("Test", genre);

            // test mapping from 
            GenreEntityMapping.MapFrom(genreEntity, "new-test");
            Assert.AreEqual("new-test", genreEntity.Name);
        }

        [TestMethod]
        public void TestLibraryMapping()
        {
            var libraryEntity = new LibraryEntity
            {
                Name = "test",
                Path = "test",
                Type = MediaTypeFlag.MANGA,
                ScannerName = "test"
            };
            var libraryInfo = new LibraryInfo();
            LibraryEntityMapping.Map(libraryEntity, libraryInfo);
            Assert.AreEqual(libraryEntity.Name, libraryInfo.Name);
            Assert.AreEqual(libraryEntity.Path, libraryInfo.Path);
            Assert.AreEqual(libraryEntity.Type, libraryInfo.Type);
            Assert.AreEqual(libraryEntity.ScannerName, libraryInfo.ScannerName);
            libraryInfo.Name = "new-test";
            libraryInfo.Path = "new-test";
            libraryInfo.Type = MediaTypeFlag.VIDEO;
            libraryInfo.ScannerName = "new-test";
            LibraryEntityMapping.MapFrom(libraryEntity, libraryInfo);
            Assert.AreEqual(libraryInfo.Name, libraryEntity.Name);
            Assert.AreEqual(libraryInfo.Path, libraryEntity.Path);
            Assert.AreEqual(libraryInfo.Type, libraryEntity.Type);
            Assert.AreEqual(libraryInfo.ScannerName, libraryEntity.ScannerName);
        }

        [TestMethod]
        public void TestMediaInfoMapping()
        {
            var mediaInfoEntity = new MediaInfoEntity
            {
                Name = "test",
                Author = new[] { "test" },
                Description = "test",
                ImageUrl = "test",
                Url = "test",
                Status = "test",
                Type = MediaTypeFlag.MANGA,
                ProviderName = "test",
                ProviderUrl = "test",
                Genres =
                [
                    new GenreEntity
                    {
                        Name = "test-genre"
                    }
                ],
                Tags =
                [
                    new TagEntity
                    {
                        Name = "test-tag"
                    }
                ],
                Chapters =
                [
                    new ChapterEntity
                    {
                        Title = "test-title",
                        Url = "test-url"
                    }
                ]
            };
            var mediaInfo = new MediaInfo();
            MediaInfoEntityMapping.Map(mediaInfoEntity, mediaInfo);
            Assert.AreEqual(mediaInfoEntity.Name, mediaInfo.Name);
            CollectionAssert.AreEqual(mediaInfoEntity.Author, mediaInfo.Author);
            Assert.AreEqual(mediaInfoEntity.Description, mediaInfo.Description);
            Assert.AreEqual(mediaInfoEntity.ImageUrl, mediaInfo.ImageUrl);
            Assert.AreEqual(mediaInfoEntity.Url, mediaInfo.Url);
            Assert.AreEqual(mediaInfoEntity.Status, mediaInfo.Status);
            Assert.AreEqual(mediaInfoEntity.Type.GetMediaTypeString(), mediaInfo.Type);
            Assert.AreEqual(mediaInfoEntity.ProviderName, mediaInfo.ProviderName);
            Assert.AreEqual(mediaInfoEntity.ProviderUrl, mediaInfo.ProviderUrl);

            Assert.AreEqual(mediaInfoEntity.Genres.Count, mediaInfo.Genres.Length);
            var genreList = mediaInfoEntity.Genres.ToList();
            for (int i = 0; i < mediaInfoEntity.Genres.Count; i++)
            {
                Assert.AreEqual(genreList[i].Name, mediaInfo.Genres[i]);
            }

            Assert.AreEqual(mediaInfoEntity.Tags.Count, mediaInfo.Tags.Length);
            var tagList = mediaInfoEntity.Tags.ToList();
            for (int i = 0; i < mediaInfoEntity.Tags.Count; i++)
            {
                Assert.AreEqual(tagList[i].Name, mediaInfo.Tags[i]);
            }

            Assert.AreEqual(mediaInfoEntity.Chapters.Count, mediaInfo.Chapters.Length);
            var chapterList = mediaInfoEntity.Chapters.ToList();
            for (int i = 0; i < mediaInfoEntity.Chapters.Count; i++)
            {
                Assert.AreEqual(chapterList[i].Title, mediaInfo.Chapters[i].ChapterName);
                Assert.AreEqual(chapterList[i].Url, mediaInfo.Chapters[i].ChapterUrl);
            }

            // assign new value
            mediaInfo.Name = "new-test";
            mediaInfo.Author = new[] { "new-test-author" };
            mediaInfo.Description = "new-test-description";
            mediaInfo.ImageUrl = "new-test-image-url";
            mediaInfo.Url = "new-test-url";
            mediaInfo.Status = "new-test-status";
            mediaInfo.Type = "manga";
            mediaInfo.ProviderName = "new-test-provider-name";
            mediaInfo.ProviderUrl = "new-test-provider-url";
            mediaInfo.Genres = new[] { "new-test-genre" };
            mediaInfo.Tags = new[] { "new-test-tag" };
            mediaInfo.Chapters = new[]
            {
                new ChapterInfo
                {
                    ChapterName = "new-test-title",
                    ChapterUrl = "new-test-url"
                }
            };

            MediaInfoEntityMapping.MapFrom(mediaInfoEntity, mediaInfo);

            Assert.AreEqual(mediaInfo.Name, mediaInfoEntity.Name);
            CollectionAssert.AreEqual(mediaInfo.Author, mediaInfoEntity.Author);
            Assert.AreEqual(mediaInfo.Description, mediaInfoEntity.Description);
            Assert.AreEqual(mediaInfo.ImageUrl, mediaInfoEntity.ImageUrl);
            Assert.AreEqual(mediaInfo.Url, mediaInfoEntity.Url);
            Assert.AreEqual(mediaInfo.Status, mediaInfoEntity.Status);
            Assert.AreEqual(mediaInfo.Type, mediaInfoEntity.Type.GetMediaTypeString());
            Assert.AreEqual(mediaInfo.ProviderName, mediaInfoEntity.ProviderName);
            Assert.AreEqual(mediaInfo.ProviderUrl, mediaInfoEntity.ProviderUrl);

            Assert.AreEqual(mediaInfo.Genres.Length, mediaInfoEntity.Genres.Count);
            genreList = mediaInfoEntity.Genres.ToList();
            for (int i = 0; i < mediaInfo.Genres.Length; i++)
            {
                Assert.AreEqual(mediaInfo.Genres[i], genreList[i].Name);
            }

            Assert.AreEqual(mediaInfo.Tags.Length, mediaInfoEntity.Tags.Count);
            tagList = mediaInfoEntity.Tags.ToList();
            for (int i = 0; i < mediaInfo.Tags.Length; i++)
            {
                Assert.AreEqual(mediaInfo.Tags[i], tagList[i].Name);
            }

            Assert.AreEqual(mediaInfo.Chapters.Length, mediaInfoEntity.Chapters.Count);
            chapterList = mediaInfoEntity.Chapters.ToList();
            for (int i = 0; i < mediaInfo.Chapters.Length; i++)
            {
                Assert.AreEqual(mediaInfo.Chapters[i].ChapterName, chapterList[i].Title);
                Assert.AreEqual(mediaInfo.Chapters[i].ChapterUrl, chapterList[i].Url);
            }
        }

        [TestMethod]
        public void TestTagEntityMapping()
        {
            var tagEntity = new TagEntity
            {
                Name = "Test"
            };

            TagEntityMapping.Map(tagEntity, out string tag);

            Assert.AreEqual("Test", tag);

            // test mapping from 
            TagEntityMapping.MapFrom(tagEntity, "new-test");
            Assert.AreEqual("new-test", tagEntity.Name);
        }
    }
}