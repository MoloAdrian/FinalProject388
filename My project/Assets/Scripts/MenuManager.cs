using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mMainMenu;
    [SerializeField] private GameObject mOptionsMenu;
    [SerializeField] private Slider mMusic;
    [SerializeField] private Slider mSound;
    [SerializeField] private Animator mLevelChange;
    private int lvl = 0;

    //---------Button Options---------//
    //Quit game
    public void QuitGame()
    {
        mLevelChange.SetTrigger("FadeOut");
        lvl = -1;
    }

    //Open options menu
    public void OpenOptions()
    {
        mMainMenu.SetActive(false);
        mOptionsMenu.SetActive(true);
    }
    
    //Open main menu
    public void OpenMain()
    {
        mMainMenu.SetActive(true);
        mOptionsMenu.SetActive(false);
    }

    //Open game
    public void OpenGame()
    {
        mLevelChange.SetTrigger("FadeOut");
        lvl = 1;
    }

    //Update sound on prefs
    public void UpdateSounds()
    {
        PlayerPrefs.SetFloat("soundsVol", mSound.value);
    }

    //Update music on prefs
    public void UpdateMusic()
    {
        PlayerPrefs.SetFloat("musicVol", mMusic.value);
    }

    //Changes level
    public void ChangeLevel()
    {
        if (lvl == -1)
            Application.Quit();
    }
}
