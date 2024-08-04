using Aiba.Enums;
using Aiba.Model;
using Aiba.Model.Constants;
using Aiba.Plugin;
using Aiba.Services;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System.Web;

namespace Aiba.MediaInfoProviders
{
    public class ShenShiMangaInfoProvider : IMediaInfoProvider
    {
        public ShenShiMangaInfoProvider(ILogger<ShenShiMangaInfoProvider> logger,
            DecompressServiceFactory decompressServiceFactoryFactory)
        {
            _logger = logger;
            _decompressServiceFactory = decompressServiceFactoryFactory;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:128.0) Gecko/20100101 Firefox/128.0");
            _httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            _httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br, zstd");
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
            _httpClient.DefaultRequestHeaders.Add("Refer", ProviderUrl);
            _httpClient.DefaultRequestHeaders.Add("DNT", "1");
            var uri = new Uri(ProviderUrl);
            _httpClient.DefaultRequestHeaders.Add("Host", uri.Host);
        }

        private readonly DecompressServiceFactory _decompressServiceFactory;

        private readonly HttpClient _httpClient;
        private readonly ILogger<ShenShiMangaInfoProvider> _logger;
        public string ProviderName => "wnacg";
        public string ProviderUrl { get; } = "https://www.wnacg.com";

        public async Task<IEnumerable<MediaInfo>> SearchAsync(MediaTypeFlag type, string searchText,
            int page,
            CancellationToken cancellationToken)
        {
            if (!type.HasFlag(MediaTypeFlag.MANGA))
                return new List<MediaInfo>().AsEnumerable();
            List<MediaInfo> mediaInfos = [];
            HttpResponseMessage response;
            try
            {
                response =
                    await _httpClient.GetAsync(BuildSearchUrl(searchText, page), cancellationToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogError("search task cancelled");
                return [];
            }
            catch (Exception e)
            {
                _logger.LogError("search failed : {Exception}", e.ToString());
                return [];
            }

            string html;

            try
            {
                string? format = response.Content.Headers.ContentEncoding.ToString();
                format ??= "";
                IDecompressService decompressService = _decompressServiceFactory.GetDecompressService(format);
                Stream compressedStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                Stream stream = await decompressService.DecompressAsync(
                    compressedStream,
                    cancellationToken);
                html = await new StreamReader(stream).ReadToEndAsync(cancellationToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogError("decompress task cancelled");
                return [];
            }
            catch (Exception e)
            {
                _logger.LogError("decompress failed : {Exception}", e.ToString());
                return [];
            }

            HtmlDocument document = new();
            try
            {
                document.LoadHtml(html);
            }
            catch (Exception e)
            {
                _logger.LogError("parse html failed : {Exception}", e.ToString());
                return [];
            }

            IList<HtmlNode>? items = document.DocumentNode.QuerySelectorAll(".gallary_wrap > .cc > .gallary_item");
            if (items == null || items.Count == 0)
            {
                return [];
            }

            foreach (HtmlNode item in items)
            {
                string? image = item.QuerySelector(".pic_box img").Attributes["src"].Value;
                if (image != null)
                    image = "https:" + image;
                string? name = item.QuerySelector(".info > .title a").InnerText;
                string? url = item.QuerySelector(".info > .title a").Attributes["href"].Value;
                if (string.IsNullOrEmpty(url))
                {
                    continue;
                }

                url = "https://www.wnacg.com" + url;

                mediaInfos.Add(new MediaInfo
                {
                    ImageUrl = image ?? "",
                    Name = name ?? "unknown",
                    Url = url,
                    Type = MediaInfoType.MANGA,
                    ProviderName = ProviderName,
                    ProviderUrl = ProviderUrl
                });
            }

            return mediaInfos.AsEnumerable();
        }

        public async Task<MediaInfo> GetDetailInfoAsync(string url, CancellationToken cancellationToken)
        {
            var resultInfo = new MediaInfo
            {
                Url = url,
                ProviderUrl = ProviderUrl,
                ProviderName = ProviderName
            };

            HttpResponseMessage response;
            try
            {
                response =
                    await _httpClient.GetAsync(url, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogError("get media detail info task cancelled");
                return resultInfo;
            }
            catch (Exception e)
            {
                _logger.LogError("get media detail failed : {Exception}", e.ToString());
                return resultInfo;
            }

            string html;

            try
            {
                string? format = response.Content.Headers.ContentEncoding.ToString();
                format ??= "";
                IDecompressService decompressService = _decompressServiceFactory.GetDecompressService(format);
                Stream compressedStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                Stream stream = await decompressService.DecompressAsync(
                    compressedStream,
                    cancellationToken);
                html = await new StreamReader(stream).ReadToEndAsync(cancellationToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogError("decompress task cancelled");
                return resultInfo;
            }
            catch (Exception e)
            {
                _logger.LogError("decompress failed : {Exception}", e.ToString());
                return resultInfo;
            }

            HtmlDocument document = new();
            try
            {
                document.LoadHtml(html);
            }
            catch (Exception e)
            {
                _logger.LogError("parse html failed : {Exception}", e.ToString());
                return resultInfo;
            }

            HtmlNode? titleNode = document.DocumentNode.QuerySelector("#bodywrap > h2");
            if (titleNode != null)
            {
                resultInfo.Name = titleNode.InnerText.Trim();
            }

            HtmlNode? coverImageNode =
                document.DocumentNode.QuerySelector("#bodywrap > .asTB > .asTBcell.uwthumb > img");
            if (coverImageNode != null)
            {
                string? src = coverImageNode.Attributes["src"].Value;
                if (!string.IsNullOrEmpty(src))
                    resultInfo.ImageUrl = "https:" + src.Trim();
            }

            HtmlNode? genreNode = document.DocumentNode.QuerySelector(".asTBcell.uwconn > label");
            if (genreNode != null)
            {
                string? genre = genreNode.InnerText;
                genre ??= "";
                genre = genre.Replace("分類：", "");
                resultInfo.Genres = genre.Split('／').Select(x => x.Trim()).ToArray();
            }

            IList<HtmlNode>? tagNodes =
                document.DocumentNode.QuerySelectorAll(".asTBcell.uwconn > .addtags > .tagshow");
            if (tagNodes is { Count: > 0 })
            {
                resultInfo.Tags = tagNodes.Select(x => x.InnerText.Trim()).ToArray();
            }

            HtmlNode? descriptionNode = document.DocumentNode.QuerySelector(".asTBcell.uwconn > p");
            if (descriptionNode != null)
            {
                resultInfo.Description = descriptionNode.InnerText.Replace("簡介：", "").Trim();
            }

            HtmlNode? chapterNode = document.DocumentNode.QuerySelector(".asTBcell .uwthumb a.btn");
            if (chapterNode != null)
            {
                var info = new ChapterInfo
                {
                    ChapterName = resultInfo.Name,
                    ChapterUrl = ProviderUrl + chapterNode.Attributes["href"].Value
                };
                resultInfo.Chapters = [info];
            }

            return resultInfo;
        }

        private static string BuildSearchUrl(string searchText, int page = 1)
        {
            string encodedSearchText = HttpUtility.UrlEncode(searchText);
            return $"https://www.wnacg.com/search/?q={encodedSearchText}&f=_all&s=create_time_DESC&syn=yes&p={page}";
        }
    }
}