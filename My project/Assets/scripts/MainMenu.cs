using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mMainMenu;
    [SerializeField] private GameObject mOptionsMenu;
    [SerializeField] private Text mMusicText;
    [SerializeField] private Text mSoundText;
    [SerializeField] private Slider mMusicSlider;
    [SerializeField] private Slider mSoundSlider;
    [SerializeField] private AudioManager mAudioManager;

    // Start is called before the first frame update
    void Start()
    {
        mOptionsMenu.SetActive(true);
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            mMusicSlider.enabled = true;
            mMusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
            PlayerPrefs.SetFloat("MusicVolume", mMusicSlider.value);
        if (PlayerPrefs.HasKey("SoundVolume"))
        {
            mSoundSlider.enabled = true;
            mSoundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
        }
        else
            PlayerPrefs.SetFloat("SoundVolume", mSoundSlider.value);
        mOptionsMenu.SetActive(false);
    }

    // Update texts
    void Update()
    {
        mMusicText.text = "Music: " + (int)(mMusicSlider.value * 100) + "%";
        mSoundText.text = "Sound: " + (int)(mSoundSlider.value * 100) + "%";
    }

    //Button options
    public void GoToOptionsMenu()
    {
        mOptionsMenu.SetActive(true);
        mMainMenu.SetActive(false);
    }
    public void GoToMainMenu()
    {
        mMainMenu.SetActive(true);
        mOptionsMenu.SetActive(false);
    }
    public void GoToQuitGame()
    {
        Application.Quit();
    }
    public void GoToGamePlay()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void UpdateMusicValue()
    {
        PlayerPrefs.SetFloat("MusicVolume", mMusicSlider.value);
        mAudioManager.UpdateMusicVolume();
    }
    public void UpdateSoundValue()
    {
        PlayerPrefs.SetFloat("SoundVolume", mSoundSlider.value);
        mAudioManager.UpdateSoundVolume();
    }

}
