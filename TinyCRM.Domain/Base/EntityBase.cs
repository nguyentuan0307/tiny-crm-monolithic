using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinyCRM.Domain.Base
{
    public interface IEntityBase<TKey>
    {
        TKey Id { get; set; }
    }

    public abstract class EntityBase<TKey> : IEntityBase<TKey>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; } = default!;
    }

    public interface IAuditEntity
    {
        DateTime CreatedDate { get; set; }
        string? CreatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        string? UpdatedBy { get; set; }
    }

    public abstract class AuditEntity<TKey> : EntityBase<TKey>, IAuditEntity
    {
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}