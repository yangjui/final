using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class BGM : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider audioSlider;

    public void SlideVolume()
    {
        float volume = Mathf.Log10(audioSlider.value) * 20;
        mixer.SetFloat("BGM", volume);
    }
}
