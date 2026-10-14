namespace Blueprint.Domain.Common.Entities;

public abstract class SoftDeletableEntity : AuditableEntity
{
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedOnUtc { get; protected set; }

    protected SoftDeletableEntity() : base() { }

    protected SoftDeletableEntity(Guid id) : base(id) { }

    public virtual void SoftDelete()
    {
        IsDeleted = true;
        DeletedOnUtc = DateTime.UtcNow;
        ModifiedOnUtc = DateTime.UtcNow;
    }

    public virtual void Restore()
    {
        IsDeleted = false;
        DeletedOnUtc = null;
        ModifiedOnUtc = DateTime.UtcNow;
    }
}