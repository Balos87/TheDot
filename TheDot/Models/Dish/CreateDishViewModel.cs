using System.ComponentModel.DataAnnotations;

namespace TheDot.Models.Dish
{
    public class CreateDishViewModel
    {
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string DishName { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Range(0, 10000)]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public bool Popular { get; set; }
    }
}
