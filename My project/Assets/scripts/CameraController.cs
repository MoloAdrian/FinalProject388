using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{

    public GameObject mPlayer;
    public float mMoveTreshold = .1f;
    public float mSpeedDecreaseFactor = 1.0f;
    float moveVelocity = 0.0f;
    bool gotToPlayer = true;
    private float timeSinceLastLaunch = 0.0f;
    private float minTimeToCatchup = 0.5f;
    bool Started = true;

    public GameObject LoseScreen;

    float highest;

    public bool Lost = false;


    // Start is called before the first frame update
    void Start()
    {
        highest = mPlayer.transform.position.y;
        Lost = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Lost)
        {
            return;
        }
        if (Started && !gotToPlayer)
        {
            
            moveVelocity += 0.05f * Time.deltaTime;
            //Debug.Log(moveVelocity);
            moveVelocity = Mathf.Clamp(moveVelocity, 0, 0.02f);
            timeSinceLastLaunch += Time.deltaTime;
            if (timeSinceLastLaunch >= minTimeToCatchup)
            {
                if (transform.position.y >= mPlayer.transform.position.y + 3)
                {
                    transform.position = new Vector3(transform.position.x, mPlayer.transform.position.y + 3, transform.position.z);
                    //transform.parent = mPlayer.transform;
                    gotToPlayer = true;
                    moveVelocity = 0.0f;
                }
            }
        }



        if (mPlayer.transform.position.y > highest)
        {
            highest = mPlayer.transform.position.y;
        }

        if (mPlayer.transform.position.y < highest - 25)
        {
            Lose();
        }

    }
    private void FixedUpdate()
    {
        if (Lost)
        {
            return;
        }
        if (Started)
        {
            if (!gotToPlayer)
            {
                transform.position += new Vector3(0, moveVelocity, 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, mPlayer.transform.position.y + 3, transform.position.z);
            }
        }
    }

    public void PlayerLaunched()
    {
        transform.parent = null;
        gotToPlayer = false;
        timeSinceLastLaunch = 0.0f;
        Started = true;
    }

    public void Lose()
    {
        mPlayer.GetComponent<PlayerController>().lost = true;
        Lost = true;
        LoseScreen.SetActive(true);
        LoseScreen.transform.Find("Score").GetComponent<Text>().text = new string("Score: " + highest.ToString("F1"));
        float highscore = PlayerPrefs.GetFloat("High");
        if (highest > highscore)
        {
            PlayerPrefs.SetFloat("High", highest);
            highscore = highest;
            LoseScreen.transform.Find("newhs").gameObject.SetActive(true);
        }
        LoseScreen.transform.Find("HighScore").GetComponent<Text>().text = new string("High-Score: " + highscore.ToString("F1"));
    }
}
