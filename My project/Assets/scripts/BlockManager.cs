using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private Transform mLaunchPads;
    private LaunchPad mLastLP;
    private LevelGenerator mGenerator;
    private bool mBlockEnded = false;
    bool mFlipX = false;
    private Transform mWallPosition;

    // Start is called before the first frame update
    void Start()
    {
        mWallPosition = GameObject.Find("Cube (2)").transform;

        //Spawn first launchpad
        mLaunchPads = gameObject.transform.GetChild(0);
        mLastLP = mLaunchPads.transform.GetChild(mLaunchPads.transform.childCount - 1).gameObject.GetComponent<LaunchPad>();
        mGenerator = GameObject.Find("FlowManager").GetComponent<LevelGenerator>();

        //If flipped, flip all the object first
        if (mFlipX)
            gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);

        //Disable camera anchor so that objects don't follow the camera anymore
        //Camera anchor only adjusts them at the start to fit the resolution.
        for(int i = 1; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<CameraAnchor>().enabled = false;

        //Update launchpads
        for (int i = 0; i < mLaunchPads.childCount; i++)
        {
            //If flipped flip direction and create random position with offsets
            Transform child = mLaunchPads.GetChild(i);
            LaunchPad pad = child.gameObject.GetComponent<LaunchPad>();
            if (mFlipX)
                pad.mDirection.x = -pad.mDirection.x;
            Vector3 newPos = child.position;
            newPos += new Vector3(Random.Range(-pad.mRandomOffset.x, pad.mRandomOffset.x), Random.Range(-pad.mRandomOffset.y, pad.mRandomOffset.y), 0);

            //Set limits for random position
            if (newPos.x > 0 && newPos.x >= mWallPosition.position.x - 1)
                newPos.x = mWallPosition.position.x - 1.15f;
            else if (newPos.x < 0 && newPos.x <= -(mWallPosition.position.x) + 1)
                newPos.x = (-mWallPosition.position.x) + 1.15f;

            //Set limits for range too
            if (pad.mMovementRange.x + newPos.x >= mWallPosition.position.x - 1 || pad.mMovementRange.x - newPos.x <= mWallPosition.position.x + 1)
                pad.mMovementRange.x = (mWallPosition.position.x - 1.15f) - Mathf.Abs(newPos.x);
            
            //Set random position and disable camera anchor
            child.position = newPos;
            child.gameObject.GetComponent<CameraAnchor>().enabled = false;
            pad.mStartingPos = newPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If block ended, don't update
        if (mBlockEnded)
            return;

        //If last launchpad was triggered, end block by generating new block
        if(mLastLP.mTriggered == true)
        {
            float launchBonus = mLastLP.GetComponent<LaunchPad>().mAccuracyValue - 3;
            mGenerator.GenerateNewLevel(new Vector3(0, mLastLP.transform.position.y + launchBonus, 0));
            mBlockEnded = true;
        }
    }

    //Set to flip the X of the block
    public void FlipX()
    {
        mFlipX = true;
    }
}
