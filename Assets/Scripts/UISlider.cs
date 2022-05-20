using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour
{
    private Slider slider;
    [SerializeField] float updateSpeed = 0.1f;

    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxValue(int value)
    {
        slider.maxValue = value;
        slider.value = value;
    }

    public void SetSliderText(string text)
    {
        GetComponentInChildren<Text>().text = text;
    }

    public virtual void SetValue(float value)
    {
        StartCoroutine(TransitionToValue(value));
    }

    private IEnumerator TransitionToValue(float value)
    {
        float currentSliderValue = slider.value;
        float elapsed = 0f;
        while (elapsed < updateSpeed)
        {
            elapsed += Time.deltaTime;
            slider.value = Mathf.Lerp(currentSliderValue, value, elapsed / updateSpeed);
            yield return null;
        }
        slider.value = value;
    }

}


