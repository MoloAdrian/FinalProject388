using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private Transform mLaunchPads;
    private int mLPIdx = 0;
    private GameObject mCurrentLP;
    private LevelGenerator mGenerator;
    private bool mBlockEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        //Spawn first launchpad
        mLaunchPads = gameObject.transform.GetChild(0);
        mGenerator = GameObject.Find("FlowManager").GetComponent<LevelGenerator>();
        if (mLPIdx < mLaunchPads.childCount)
        {
            //Set launchpad at random position and activate
            mCurrentLP = mLaunchPads.GetChild(mLPIdx++).transform.gameObject;
            mCurrentLP.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mBlockEnded)
            return;
        if(mCurrentLP.activeSelf == false)
        {
            if (mLPIdx < mLaunchPads.childCount)
            {
                //Set launchpad at random position and activate
                mCurrentLP = mLaunchPads.GetChild(mLPIdx++).transform.gameObject;
                mCurrentLP.SetActive(true);
            }
            else
            {
                float launchBonus = mCurrentLP.GetComponent<LaunchPad>().mAccuracyValue - 3;
                mGenerator.GenerateNewLevel(new Vector3(0, mCurrentLP.transform.position.y + launchBonus, 0));
                mBlockEnded = true;
            }
        }
    }
}
