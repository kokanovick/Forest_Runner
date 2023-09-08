using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Volume : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] string audioParameter;
    [SerializeField] float multiplier = 25;

    public void SetupSlider()
    {
        slider.onValueChanged.AddListener(SliderEdit);
        slider.minValue = .001f;
        slider.value = PlayerPrefs.GetFloat(audioParameter, slider.value);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(audioParameter, slider.value);
    }
    private void SliderEdit(float value)
    {
        audioMixer.SetFloat(audioParameter, Mathf.Log10(value)*multiplier);
    }
}
