using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Entities
{
    [Table("Notes")]
    public class Note : MyEntityBase
    {
        [DisplayName("Not Başlığı"), Required, StringLength(60)]
        public string Title { get; set; }

        [DisplayName("Not Metni"), Required, StringLength(2000)]
        public string Text { get; set; }

        [DisplayName("Tamamlandı")]
        public bool IsCompleted { get; set; }

        [DisplayName("İşlem Tarihi"), Required, DataType(DataType.DateTime)]
        public DateTime CompletedOn { get; set; }

        [DisplayName("Kategori")]
        public int CategoryId { get; set; }

        public virtual TodoUser Owner { get; set; }
        public virtual Category Category { get; set; }
    }
}
