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
    [SerializeField] GameObject heart;

    private void OnEnable()
    {
        health.changed += UpdateHearts;
        health.baseHealthChanged += UpdateBaseHearts;
    }

    private void Start()
    {
        UpdateBaseHearts();
    }

    private void UpdateBaseHearts()
    {
        foreach (Image heart in hearts)
        {
            Destroy(heart.gameObject);
        }
        hearts.Clear();
        for (int i = 0; i < health.BaseHealth; i++)
        {
            var heartInstance = Instantiate(heart, transform);
            hearts.Add(heartInstance.GetComponent<Image>());
        }
    }
    private void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth) { hearts[i].sprite = fullHeart; }
            else { hearts[i].sprite = emptyHeart; }
        }
    }

    private void OnDisable()
    {
        health.changed -= UpdateHearts;
        health.baseHealthChanged -= UpdateBaseHearts;
    }
}
