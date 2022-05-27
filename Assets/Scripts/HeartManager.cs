using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;
    [SerializeField] List<Image> hearts;
    [SerializeField] Health health;

    private void OnEnable()
    {
        health.changed += UpdateHearts;
    }

    private void UpdateHearts(int currentHealth)
    {
        int fullHearts = 3;
        switch (currentHealth)
        {
            case 3:
                fullHearts = 3;
                break;
            case 2:
                fullHearts = 2;
                break;
            case 1:
                fullHearts = 1;
                break;
            default:
                fullHearts = 0;
                break;
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < fullHearts) { hearts[i].sprite = fullHeart; }
            else { hearts[i].sprite = emptyHeart; }
        }
    }

    private void OnDisable()
    {
        health.changed -= UpdateHearts;
    }
}
