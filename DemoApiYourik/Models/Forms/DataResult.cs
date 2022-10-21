namespace DemoApiYourik.Models.Forms
{
#nullable disable
    public class DataResult
    {
        public int PageCount { get; set; }
        public IEnumerable<DataDto> Datas { get; set; }
    }
#nullable enable
}
