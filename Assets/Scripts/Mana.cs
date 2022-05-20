using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{
    [SerializeField] float baseMana = 100;
    private float timeSinceLastDepletion = Mathf.Infinity;
    [SerializeField] float manaRecoveryCooldown = 1f;
    [SerializeField] float manaRecoveryRate = 10f;
    public float CurrentMana { get; private set; } 

    private void Start()
    {
        CurrentMana = baseMana;
    }

    public void SpendMana(float cost)
    {
        CurrentMana = Mathf.Clamp(CurrentMana - cost, 0, baseMana);
        Debug.Log($"Spent {cost} mana. Current mana {CurrentMana}.");
        timeSinceLastDepletion = 0;
    }

    private void Update()
    {
        timeSinceLastDepletion += Time.deltaTime;
        if (timeSinceLastDepletion > manaRecoveryCooldown && CurrentMana < baseMana)
        {
            CurrentMana += manaRecoveryRate * Time.deltaTime;
        }
    }
}
