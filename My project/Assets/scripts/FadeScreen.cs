using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeScreen : MonoBehaviour
{
    private int mLevel;
    [SerializeField] private Image mBlackScreen;
    private bool mFadingIn = true;
    private float dt;

    private void Start()
    {
        dt = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(mFadingIn)
        {
            Color col = new Color(mBlackScreen.color.r, mBlackScreen.color.g, mBlackScreen.color.b, Mathf.Max(mBlackScreen.color.a - dt, 0f));
            mBlackScreen.color = col;
            if(mBlackScreen.color.a <= 0f)
            {
                mFadingIn = false;
                gameObject.SetActive(false);
            }
            return;
        }

        if(!mFadingIn && mBlackScreen.color.a > 0f)
        {
            Color col = new Color(mBlackScreen.color.r, mBlackScreen.color.g, mBlackScreen.color.b, Mathf.Min(mBlackScreen.color.a + dt, 1f));
            mBlackScreen.color = col;
            if (mBlackScreen.color.a >= 1f)
            {
                Time.timeScale = 1.0f;
                if (mLevel == -1)
                    Application.Quit();
                else
                    SceneManager.LoadScene(mLevel);
            }
        }
    }

    public void StartFade(int lvl)
    {
        mBlackScreen.color = new Color(mBlackScreen.color.r, mBlackScreen.color.g, mBlackScreen.color.b, mBlackScreen.color.a + dt);
        mLevel = lvl;
    }
}
