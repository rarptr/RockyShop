using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocky_Models
{
    // Заголовок запроса
    public class InquiryHeader
    {
        [Key]
        public int Id { get; set; }
        // Для получения реквизитов клиента
        // Строка, т.к. идентификатор - автогенерируемый уникальный ключ
        public string ApplicationUserId { get; set; }
        // Связка по внешнему ключу
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        // Время регистрации запроса
        public DateTime InquiryDate { get; set; }
        // Обязательное поле
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }


    }
}
