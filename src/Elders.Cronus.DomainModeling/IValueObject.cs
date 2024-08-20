using System;
using System.Collections.Generic;
using System.Reflection;

namespace Elders.Cronus;

[Obsolete("Use record classes instead.")]
public interface IValueObject<T> : IEqualityComparer<T>, IEquatable<T> { }

/// <summary>
/// The class which implements ValueObject<T> has to be marked with sealed keyword.
/// </summary>
/// <typeparam name="T"></typeparam>
[Obsolete("Use record classes instead.")]
public abstract class ValueObject<T> : IValueObject<T> where T : ValueObject<T>
{
    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return Equals(obj as T);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            int startValue = 23;
            int multiplier = 77;

            int hashCode = startValue;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(this);

                if (value != null)
                    hashCode = hashCode * multiplier ^ value.GetHashCode();
            }
            return hashCode;
        }
    }

    public virtual bool Equals(T other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        var t = GetType();
        if (t != other.GetType())
            return false;

        FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        foreach (FieldInfo field in fields)
        {
            object value1 = field.GetValue(other);
            object value2 = field.GetValue(this);

            if (value1 is null)
            {
                if (value2 is not null)
                    return false;
            }
            else if (!value1.Equals(value2))
                return false;
        }
        return true;
    }

    public static bool operator ==(ValueObject<T> left, ValueObject<T> right)
    {
        if (left is null && right is null) return true;
        if (left is null)
            return false;
        else
            return left.Equals(right);
    }

    public static bool operator !=(ValueObject<T> left, ValueObject<T> right)
    {
        return !(left == right);

    }

    public bool Equals(T left, T right)
    {
        if (left is null && right is null) return true;
        if (left is null)
            return false;
        else
            return left.Equals(right);
    }

    public int GetHashCode(T obj)
    {
        return obj.GetHashCode();
    }
}
