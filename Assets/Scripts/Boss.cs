using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] GameObject healthBar;
    [SerializeField] string bossName;
    protected void Awake()
    {
        var healthBarInstance = Instantiate(healthBar).GetComponent<UISlider>();
        healthBarInstance.SetSliderText(bossName);
        health.SetSlider(healthBarInstance);
    }
}
