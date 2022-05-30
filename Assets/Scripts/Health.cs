using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public int BaseHealth { get; private set; } = 3;
    [SerializeField] public float iSeconds = 1;
    private float timeSinceDamaged = Mathf.Infinity;

    public UISlider healthBar { get; private set; }
    public int CurrentHealth { get; private set; }
    public bool IsDead { get; private set; } = false;
    public Action died;
    public Action<int> changed;
    public Action baseHealthChanged;
    public bool gameOver = false;
    

    private void Start()
    {
        BossSpawner.allBossesKilled += () => gameOver = true;
        CurrentHealth = BaseHealth;
    }

    private void Update()
    {
        timeSinceDamaged += Time.deltaTime;
    }

    public void SetSlider(UISlider slider)
    {
        healthBar = slider;
        healthBar.SetMaxValue(BaseHealth);
    }
    public void SetBaseHealth(int value)
    {
        if (IsDead || gameOver) { return; }
        BaseHealth = value;
        if (healthBar) { healthBar.SetMaxValue(BaseHealth); }
        baseHealthChanged?.Invoke();
        
    }

    public void GainHealth(int value)
    {
        if (IsDead || gameOver) { return; }
        CurrentHealth += value;
        if (healthBar) { healthBar.SetValue(CurrentHealth); }
        changed?.Invoke(CurrentHealth);
    }
    public void TakeDamage(int damage)
    {
        if (IsDead || gameOver || timeSinceDamaged < iSeconds) { return; }
        CurrentHealth -= damage;
        //Debug.Log($"{gameObject.name} just took {damage} damage!");

        if (CurrentHealth <= 0) {
            died?.Invoke();
            IsDead = true;
            if (healthBar) { Destroy(healthBar.gameObject); }
        }

        if (healthBar)
        {
            healthBar.SetValue(CurrentHealth);
        }

        changed?.Invoke(CurrentHealth);
        timeSinceDamaged = 0;
    }

    private void OnDisable()
    {
        BossSpawner.allBossesKilled -= () => gameOver = true;
    }
}
