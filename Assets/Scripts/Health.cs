using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public int baseHealth = 3;
    [SerializeField] public float iSeconds = 1;
    private float timeSinceDamaged = Mathf.Infinity;

    public UISlider healthBar { get; private set; }
    public int CurrentHealth { get; private set; }
    public bool IsDead { get; private set; } = false;
    public Action died;
    public Action<int> changed;

    private void Start()
    {
        CurrentHealth = baseHealth;
    }

    private void Update()
    {
        timeSinceDamaged += Time.deltaTime;
    }

    public void SetSlider(UISlider slider)
    {
        healthBar = slider;
        healthBar.SetMaxValue(baseHealth);
    }

    public void TakeDamage(int damage)
    {
        if (IsDead || timeSinceDamaged < iSeconds) { return; }
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
        timeSinceDamaged = 0;
    }
}
