using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private Transform mLaunchPads;
    private GameObject mLastLP;
    private LevelGenerator mGenerator;
    private bool mBlockEnded = false;
    bool mFlipX = false;

    // Start is called before the first frame update
    void Start()
    {
        //Spawn first launchpad
        mLaunchPads = gameObject.transform.GetChild(0);
        mLastLP = mLaunchPads.transform.GetChild(mLaunchPads.transform.childCount - 1).gameObject;
        mGenerator = GameObject.Find("FlowManager").GetComponent<LevelGenerator>();

        if (mFlipX)
        {
            gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            for (int i = 0; i < mLaunchPads.childCount; i++)
                mLaunchPads.GetChild(i).gameObject.GetComponent<LaunchPad>().mDirection.x = -mLaunchPads.GetChild(i).gameObject.GetComponent<LaunchPad>().mDirection.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mBlockEnded)
            return;
        if(mLastLP.activeSelf == false)
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
