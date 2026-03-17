using UnityEngine;

public interface IDamage
{
    /// <summary>
    /// Reduces the health of the entity by the specified amount of damage.
    /// </summary>
    /// <remarks>If the entity's health reaches zero or below, it may trigger death or defeat logic depending
    /// on the implementation.</remarks>
    /// <param name="damage">The amount of damage to apply. Must be a non-negative value.</param>
    public void TakeDamage(int damage);
}
