using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Example.Web.Models
{
    public class ExampleUserViewModel
    {
        [Required]
        public string Id { get; set; }

        public HttpPostedFileBase Avatar { get; set; }
    }
}