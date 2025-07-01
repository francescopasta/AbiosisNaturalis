using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public Slider sfxSlider;
    public Slider musicSlider;
    public AudioMixer mixer;

    private void Awake()
    {
        mixer.SetFloat("Music", 0);
        mixer.SetFloat("SFX", 0);
    }

    private void Update()
    {
        SetVolume("SFX", sfxSlider.value);
        SetVolume("Music", musicSlider.value);
    }

    public void SetVolume(string name, float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        float dB = Mathf.Lerp(-80f, 20f, volume);
        mixer.SetFloat(name, dB);
    }
}
