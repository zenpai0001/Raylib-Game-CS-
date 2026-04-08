using System;
using System.Collections.Generic;

public interface IDamagable<T>
{
    public int Health { get; set; }
    public int damageAmount { get; set; }

    public void TakeDamage(int damageAmount)
    {
        Health -= damageAmount;
        if (Health <= 0)
        {
            // Handle death logic here, such as removing the object from the game or triggering a death animation.
            Console.WriteLine("Object has been destroyed!");
        }
    }
}