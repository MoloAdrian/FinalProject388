using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject mPlayer;
    public float mMoveTreshold = .1f;
    public float mSpeedDecreaseFactor = 1.0f;
    float moveVelocity = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //compute the difference in height with the player
        float mHeightDiff = mPlayer.transform.position.y + 4 - transform.position.y;

        //compute the speed in which we need to follow the player
        //if (mHeightDiff >= 5.0f)
        //{
        //    mHeightDiff *= 10.0f;
        //}
        if (mHeightDiff <= 3.0f)
        {
            mHeightDiff /= 5.0f;
        }
        moveVelocity += mHeightDiff /150.0f * Time.deltaTime;
        if (moveVelocity > mHeightDiff / 3 && moveVelocity >= 0.0f)
        {
            moveVelocity -= 3*(moveVelocity - mHeightDiff) * Time.deltaTime;
        }

        moveVelocity = Mathf.Clamp(moveVelocity, -2, 30);
        //transform.position += new Vector3(0, mHeightDiff, 0) * Time.deltaTime;



    }
    private void FixedUpdate()
    {
        transform.position += new Vector3(0, moveVelocity, 0);
    }
}
