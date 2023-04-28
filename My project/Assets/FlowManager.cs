using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FlowManager : MonoBehaviour
{

    static public bool isPaused = false;
    public Canvas canvas;
    [SerializeField] private GameObject mPauseMenu;
    [SerializeField] private GameObject mMainMenu;
    [SerializeField] private GameObject mOptionsMenu;
    [SerializeField] private Text mMusicText;
    [SerializeField] private Text mSoundText;
    [SerializeField] private Slider mMusicSlider;
    [SerializeField] private Slider mSoundSlider;
    [SerializeField] private PlayerController mPlayer;
    [SerializeField] private AudioManager mAudioManager;
    [SerializeField] private FadeScreen mFadeScreen;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        mMusicText.text = "Music: " + (int)(mMusicSlider.value * 100) + "%";
        mSoundText.text = "Sound: " + (int)(mSoundSlider.value * 100) + "%";
    }

    public void SetPause()
    {
        Time.timeScale = 0.0f;
        isPaused = true;
        mPauseMenu.SetActive(true);
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

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        mPauseMenu.SetActive(false);
        mPlayer.enabled = true;
    }
    public void Restart()
    {
        mFadeScreen.gameObject.SetActive(true);
        mFadeScreen.StartFade(1);
        for (int i = 0; i < mPauseMenu.transform.childCount; i++)
            mPauseMenu.transform.GetChild(i).GetComponent<Button>().enabled = false;
    }

    public void ToMenu()
    {
        mFadeScreen.gameObject.SetActive(true);
        mFadeScreen.StartFade(0);
        for (int i = 0; i < mMainMenu.transform.childCount; i++)
            mMainMenu.transform.GetChild(i).GetComponent<Button>().enabled = false;
        if (PlayerPrefs.GetInt("DoneTutorial") == 0)
            PlayerPrefs.SetInt("DoneTutorial", 1);
    }

    public void ToOptions()
    {
        mOptionsMenu.SetActive(true);
        mMainMenu.SetActive(false);
    }
    public void ToPauseMenu()
    {
        mOptionsMenu.SetActive(false);
        mMainMenu.SetActive(true);
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
