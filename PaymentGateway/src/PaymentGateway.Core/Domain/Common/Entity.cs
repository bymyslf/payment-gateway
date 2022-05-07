namespace PaymentGateway.Core.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    public override bool Equals(object obj)
    {
        var other = obj as Entity;

        if (ReferenceEquals(other, null))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (Id == Guid.Empty || other.Id == Guid.Empty)
            return false;

        return Id == other.Id;
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (object.ReferenceEquals(a, b))
        {
            return true;
        }

        if ((object)a == null || (object)a == null)
        {
            return false;
        }
        
        
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        if (ReferenceEquals(a, b))
            return true;

        return a.Id == b.Id;
    }

    public static bool operator !=(Entity a, Entity b)
        => !(a == b);

    //http://stackoverflow.com/a/263416
    public override int GetHashCode()
        => unchecked((17 * 23 + Id.GetHashCode()));
}