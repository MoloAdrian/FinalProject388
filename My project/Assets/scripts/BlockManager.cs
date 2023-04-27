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

    // Start is called before the first frame update
    void Start()
    {
        //Spawn first launchpad
        mLaunchPads = gameObject.transform.GetChild(0);
        mLastLP = mLaunchPads.transform.GetChild(mLaunchPads.transform.childCount - 1).gameObject.GetComponent<LaunchPad>();
        mGenerator = GameObject.Find("FlowManager").GetComponent<LevelGenerator>();

        if (mFlipX)
            gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);

        for (int i = 0; i < mLaunchPads.childCount; i++)
        {
            LaunchPad pad = mLaunchPads.GetChild(i).gameObject.GetComponent<LaunchPad>();
            if (mFlipX)
                pad.mDirection.x = -pad.mDirection.x;
            Vector3 newPos = mLaunchPads.GetChild(i).position;
            newPos += new Vector3(Random.Range(-pad.mRandomOffset.x, pad.mRandomOffset.x), Random.Range(-pad.mRandomOffset.y, pad.mRandomOffset.y), 0);
            mLaunchPads.GetChild(i).position = newPos;
            pad.mStartingPos = newPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mBlockEnded)
            return;
        if(mLastLP.mTriggered == true)
        {
            float launchBonus = mLastLP.GetComponent<LaunchPad>().mAccuracyValue - 3;
            mGenerator.GenerateNewLevel(new Vector3(0, mLastLP.transform.position.y + launchBonus, 0));
            mBlockEnded = true;
        }
    }

    public void FlipX()
    {
        mFlipX = true;
    }
}
