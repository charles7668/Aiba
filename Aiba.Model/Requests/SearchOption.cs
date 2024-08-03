using Aiba.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace Aiba.Model.Requests
{
    public class SearchOption
    {
        public string SearchType { get; set; } = MediaInfoType.ALL;

        [Required]
        public string SearchText { get; set; } = string.Empty;

        public int Page { get; set; } = 1;
    }
}