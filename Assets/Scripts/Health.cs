using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public int baseHealth = 3;

    [Header("Health")]
    [SerializeField] UISlider healthBar;
    public int CurrentHealth { get; private set; }
    public bool IsDead { get; private set; } = false;
    public Action died;
    public Action<int> changed;

    private void Start()
    {
        CurrentHealth = baseHealth;
        if (healthBar) { healthBar.SetMaxValue(CurrentHealth); }
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) { return; }
        CurrentHealth -= damage;
        //Debug.Log($"{gameObject.name} just took {damage} damage!");

        if (CurrentHealth <= 0) {
            died?.Invoke();
            IsDead = true; 
        }

        if (healthBar)
        {
            healthBar.SetValue(CurrentHealth);
        }

        changed?.Invoke(CurrentHealth);
    }
}
