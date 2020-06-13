using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{

    public AudioMixer mixer;

    public Slider Master;
    public Slider Footsteps;
    public Slider Effects;
    public Slider Music;

    void Start()
    {
        Master.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        Footsteps.value = PlayerPrefs.GetFloat("FootstepsVolume", 1f);
        Effects.value = PlayerPrefs.GetFloat("EffectsVolume", 1f);
        Music.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    public void SetLevelMaster()
    {
        float sliderValue = Master.value;
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }

    public void SetLevelFootsteps()
    {
        float sliderValue = Footsteps.value;
        mixer.SetFloat("FootstepsVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("FootstepsVolume", sliderValue);
    }

    public void SetLevelEffects()
    {
        float sliderValue = Effects.value;
        mixer.SetFloat("EffectsVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("EffectsVolume", sliderValue);
    }

    public void SetLevelMusic()
    {
        float sliderValue = Music.value;
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void SetLevelMasterControler(float value)
    {
        Master.value += value;
        float sliderValue = Master.value;
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }

    public void SetLevelFootstepsControler(float value)
    {
        Footsteps.value += value;
        float sliderValue = Footsteps.value;
        mixer.SetFloat("FootstepsVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("FootstepsVolume", sliderValue);
    }

    public void SetLevelEffectsControler(float value)
    {
        Effects.value += value;
        float sliderValue = Effects.value;
        mixer.SetFloat("EffectsVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("EffectsVolume", sliderValue);
    }

    public void SetLevelMusicControler(float value)
    {
        Music.value += value;
        float sliderValue = Music.value;
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }
}