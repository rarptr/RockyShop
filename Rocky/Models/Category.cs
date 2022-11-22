using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rocky.Models
{
    public class Category
    {

        // Первичный ключ
        [Key]
        public int Id { get; set; }
        // Обязательное заполнение поля
        [Required]
        public string Name { get; set; }
        // Аннотация данных
        [DisplayName("Display Order")]
        // Обязательное заполнение поля
        [Required]
        // Допустимый диапазон числа
        [Range(1, int.MaxValue, ErrorMessage ="Display Order for category must be greater than 0")]
        public int DisplayOrder { get; set; }

    }
}
