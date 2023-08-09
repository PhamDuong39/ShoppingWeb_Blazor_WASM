using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WASM.Shared.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "ProductName is required")]
        [StringLength(70, MinimumLength = 1,
            ErrorMessage = "Username must be between 1 and 50 characters")]
        public string ProductName { get; set; } = "";
        public string Description { get; set; } = "";

        public string ImageURL { get; set; } = "";
        public decimal Price { get; set; }

        [Column("Quantity")]
        public int Qty { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
    }
}
