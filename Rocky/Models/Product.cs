using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocky.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ShortDesc { get; set; }

        public string Description { get; set; }
        [Range(1, int.MaxValue)]
        public double Price { get; set; }
        public string Image { get; set; }

        // Указываем ключ явно
        [Display(Name = "Category Type")]
        public int CategoryId { get; set; }
        // Связь между Product и Category создается автоматически
        // вместе с ключем IdCategory у Product
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        // Указываем ключ явно
        [Display(Name = "Application Type")]
        public int ApplicationTypeId { get; set; }

        [ForeignKey("ApplicationTypeId")]
        public virtual ApplicationType ApplicationType { get; set; }
    }
}
