using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject mPlayer;
    public float mMoveTreshold = .1f;
    public float mSpeedDecreaseFactor = 1.0f;
    float moveVelocity = 0.0f;
    bool gotToPlayer = false;
    private float timeSinceLastLaunch = 0.0f;
    private float minTimeToCatchup = 0.5f;
    bool Started = false;

    // Start is called before the first frame update
    void Start()
    {
      
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Started && !gotToPlayer)
        {
            
            moveVelocity += 0.05f * Time.deltaTime;
            Debug.Log(moveVelocity);
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



    }
    private void FixedUpdate()
    {
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
}
