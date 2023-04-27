using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mSoundVolume;
    [SerializeField] private AudioMixerGroup mMusicVolume;

    // Start is called before the first frame update
    void Start()
    {
        UpdateSoundVolume();
        UpdateMusicVolume();
    }

    // Update is called once per frame
    public void UpdateSoundVolume()
    {
        mSoundVolume.audioMixer.SetFloat("Sound Volume", Mathf.Log10(PlayerPrefs.GetFloat("SoundVolume")) * 20);
    }
    public void UpdateMusicVolume()
    {
        mMusicVolume.audioMixer.SetFloat("Music Volume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
    }
}
