namespace Aiba.Model.RequestParams
{
    public class AddMediaInfoRequest
    {
        public LibraryInfo LibraryInfo { get; set; } = new();
        public MediaInfo MediaInfo { get; set; } = new();
    }
}