using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMixerUpdated : MonoBehaviour
{
    [Tooltip(
        "1. Place this script on a gameobject of your choice.   " +
        "2. Drag the audio mixer and place in under _AudioMixer.  " +
        "3. Do the same for the sliders that will control the music and the sfx respectable.   " +
        "4. In the audio mixer, select the master volume group and add subgroups for music and sfx.  " +
        "5. Select one of the subgroups, in the inspector, right click the _Volume_ text and click on _Expose_.  " +
        "6. After that, in the audio mixer find _Exposed Paremeters_.   " +
        "7. Find the one for music and change it to _Music_.   " +
        "8. Find the one for the sfx and change it to _SFX_.   " +
        "9. In the _Audio Source_ component, under _Output_ put the designated subgroup for that sound. Don't put the Master group anywhere.   " +
        "10. If you need help, contact Ilian.")]
    public string Instructions = "Hover your mouse over this variable.";

    public AudioMixer audioMixer;

    public Slider musicSlider;
    public Slider sfxSlider;

    private const string MusicVolumeKey = "Music";
    private const string SFXVolumeKey = "SFX";

    private float systemVolume = -1f;

    void Start()
    {
        // Load saved values or use defaults
        float musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.75f);
        float sfxVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 0.75f);

        // Set sliders to match saved values
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        // Apply the volumes to the Audio Mixer
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);

        // Add listeners to the sliders
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void Update()
    {
        #if UNITY_ANDROID
        AndroidVolume();
        #elif Unity_IOS
        Debug.log("Sorry, it doesn't support IOS devices");
        #endif
    }

    public void SetMusicVolume(float sliderValue)
    {
        // Convert slider value to dB (-80 dB to 0 dB range)
        float dB = Mathf.Log10(sliderValue) * 20;

        // Save the value
        PlayerPrefs.SetFloat(MusicVolumeKey, sliderValue);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float sliderValue)
    {
        float dB = Mathf.Log10(sliderValue) * 20;

        PlayerPrefs.SetFloat(SFXVolumeKey, sliderValue);
        PlayerPrefs.Save();
    }

    void AndroidVolume()
    {
        /// try-catch block prevents app crashes in the event of something going wrong
        try
        {
            using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer") //access unity player class
                .GetStatic<AndroidJavaObject>("currentActivity")) //gets the running android activity
            using (AndroidJavaObject audioManager = activity.Call<AndroidJavaObject>("getSystemService", "audio")) //lets you interact with the system audio
            {
                int currentVolume = audioManager.Call<int>("getStreamVolume", 3); //current volume of the media stream
                int maxVolume = audioManager.Call<int>("getStreamMaxVolume", 3);

                float normalized = (float)currentVolume / maxVolume; 

                if (!Mathf.Approximately(normalized, systemVolume))
                {
                    float dB = Mathf.Log10(Mathf.Clamp(normalized, 0.0001f, 1f)) * 20f;
                    audioMixer.SetFloat("MasterVolume", dB);

                    systemVolume = normalized;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Could not read Android volume: " + e.Message);
        }
    }
}
