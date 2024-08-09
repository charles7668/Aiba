namespace Aiba.Model.RequestParams
{
    public class RemoveMediaInfoRequest
    {
        public MediaInfo MediaInfo { get; set; } = new();
        public LibraryInfo LibraryInfo { get; set; } = new();
    }
}