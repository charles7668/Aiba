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
            _httpClient.DefaultRequestHeaders.Add("Refer", "https://www.wnacg.com");
            _httpClient.DefaultRequestHeaders.Add("DNT", "1");
            _httpClient.DefaultRequestHeaders.Add("Host", "www.wnacg.com");
        }

        private readonly DecompressServiceFactory _decompressServiceFactory;

        private readonly HttpClient _httpClient;
        private readonly ILogger<ShenShiMangaInfoProvider> _logger;
        public string Name => "wnacg";

        public async Task<IEnumerable<MediaInfo>> SearchAsync(MediaTypeFlag type, string searchText,
            CancellationToken cancellationToken)
        {
            if (!type.HasFlag(MediaTypeFlag.MANGA))
                return new List<MediaInfo>().AsEnumerable();
            List<MediaInfo> mediaInfos = [];
            for (int page = 1; page <= 10; ++page)
            {
                HttpResponseMessage response;
                try
                {
                    response =
                        await _httpClient.GetAsync(BuildSearchUrl(searchText, page), cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    _logger.LogError("search task cancelled");
                    break;
                }
                catch (Exception e)
                {
                    _logger.LogError("search failed : {Exception}", e.ToString());
                    break;
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
                    break;
                }
                catch (Exception e)
                {
                    _logger.LogError("decompress failed : {Exception}", e.ToString());
                    break;
                }

                HtmlDocument document = new();
                try
                {
                    document.LoadHtml(html);
                }
                catch (Exception e)
                {
                    _logger.LogError("parse html failed : {Exception}", e.ToString());
                    break;
                }

                IList<HtmlNode>? items = document.DocumentNode.QuerySelectorAll(".gallary_wrap > .cc > .gallary_item");
                if (items == null || items.Count == 0)
                {
                    break;
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
                        Type = MediaInfoType.MANGA
                    });
                }
            }

            return mediaInfos.AsEnumerable();
        }

        private static string BuildSearchUrl(string searchText, int page = 1)
        {
            string encodedSearchText = HttpUtility.UrlEncode(searchText);
            return $"https://www.wnacg.com/search/?q={encodedSearchText}&f=_all&s=create_time_DESC&syn=yes&p={page}";
        }
    }
}