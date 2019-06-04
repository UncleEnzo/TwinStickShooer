using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; private set; }
    public Stat damage;
    public Stat armor;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        damage -= armor.getValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        print(transform.name + " takes " + damage + " damage.");
        if (currentHealth <= 0)
        {
            die();
        }
    }

    public virtual void die()
    {
        // Die in some way
        // This method is meant to be overwritten
        print(transform.name + " died.");
    }
}
