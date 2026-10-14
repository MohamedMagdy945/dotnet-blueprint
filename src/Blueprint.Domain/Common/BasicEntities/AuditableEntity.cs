namespace Blueprint.Domain.Common.BasicEntities;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedOnUtc { get; protected set; } = DateTime.UtcNow;
    public DateTime? ModifiedOnUtc { get; protected set; }

    protected AuditableEntity() : base() { }

    protected AuditableEntity(Guid id) : base(id) { }

    public void SetModified(DateTime utcNow)
    {
        ModifiedOnUtc = utcNow;
    }
}