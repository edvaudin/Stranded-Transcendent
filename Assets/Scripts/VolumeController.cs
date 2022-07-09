using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] string volumeParameter = "MasterVolume";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    [SerializeField] private float multiplier = 30f;
    [SerializeField] Toggle toggle;
    private bool toggleEventDisabled;

    private void Awake()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
        if (toggle != null) { toggle.onValueChanged.AddListener(HandleToggleValueChanged); }
    }

    private void HandleToggleValueChanged(bool enableSound)
    {
        if (toggleEventDisabled) { return; }
        slider.value = enableSound ? slider.maxValue : slider.minValue;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, slider.value);
    }
    private void HandleSliderValueChanged(float value)
    {
        mixer.SetFloat(volumeParameter, Mathf.Log10(value) * multiplier);
        if (toggle != null)
        {
            UnmuteIfVolume();
        }
    }

    private void UnmuteIfVolume()
    {
        toggleEventDisabled = true;
        toggle.isOn = slider.value > slider.minValue;
        toggleEventDisabled = false;
    }

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value);
    }
}
