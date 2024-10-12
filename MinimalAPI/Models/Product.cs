using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Models
{
    public class Product
    {
        [Required(ErrorMessage = "Id can't be blank")]
        [Range (0, 100000, ErrorMessage = "Id should be between 0 and 100.000")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Product name can't be blank")]
        public string? ProductName { get; set; }

        public override string ToString()
        {
            return $"Product Id: {Id}, Name: {ProductName}";
        }
    }
}
