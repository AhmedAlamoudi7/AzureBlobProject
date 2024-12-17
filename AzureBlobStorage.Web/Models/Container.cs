using System.ComponentModel.DataAnnotations;

namespace AzureBlobStorage.Web.Models
{
    public class Container
    {
        [Required]
        public string Name { get; set; }
    }
}
