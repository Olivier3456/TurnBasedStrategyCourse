using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int health = 100;

    public event EventHandler OnDead;

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health < 0)
        {
            health = 0;
        }

        if (health == 0)
        {
            Die();
        }

        Debug.Log($"{transform.name} damaged! Health = {health}.");
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}
