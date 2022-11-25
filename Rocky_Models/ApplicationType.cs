using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rocky_Models
{
    public class ApplicationType
    {

        // Первичный ключ
        [Key]
        public int Id { get; set; }
        // Обязательное заполнение поля
        [Required]
        public string Name { get; set; }
    }
}
