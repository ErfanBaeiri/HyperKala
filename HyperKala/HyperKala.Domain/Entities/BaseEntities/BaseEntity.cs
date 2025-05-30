using System.ComponentModel.DataAnnotations;

namespace HyperKala.Domain.Entities.BaseEntities
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
