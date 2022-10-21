using System.ComponentModel.DataAnnotations;

namespace DemoApiYourik.Models.Forms
{
    public class AddDataForm
    {
        [Required]
        public string Value { get; set; }
    }
}
