using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocky_Models
{
    // Детали запроса
    public class InquiryDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        // Для получения заголовка запроса
        public int InquiryHeaderId { get; set; }
        // Связка по внешнему ключу
        [ForeignKey("InquiryHeaderId ")]
        public InquiryHeader InquiryHeader { get; set; }

        [Required]
        // Для получения заголовка запроса
        public int ProductId { get; set; }
        // Связка по внешнему ключу
        [ForeignKey("ProductId ")]
        public Product Product { get; set; }



    }
}
