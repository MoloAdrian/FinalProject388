using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private int mLevelsCreated = 1;
    [SerializeField] private List<GameObject> mEasyLevels;
    [SerializeField] private List<GameObject> mNormalLevels;
    [SerializeField] private List<GameObject> mHardLevels;

    [SerializeField] private GameObject mCurrentLevel;
    private GameObject mLastLevel;

    private int mEasyRate = 75;
    private int mNormalRate = 25;
    private int mHardRate = 0;

    public void GenerateNewLevel(Vector3 newPos)
    {
        if (mLastLevel)
            Destroy(mLastLevel);
        mLastLevel = mCurrentLevel;

        mCurrentLevel = Instantiate(mEasyLevels[Random.Range(0, mEasyLevels.Count - 1)], newPos, Quaternion.identity);
        return;

        int random = Random.Range(0, 100);
        if(random <= mEasyRate)
            mCurrentLevel = Instantiate(mEasyLevels[Random.Range(0, mEasyLevels.Count - 1)], newPos, Quaternion.identity);
        else if(random <= mNormalRate + mEasyRate)
            mCurrentLevel = Instantiate(mNormalLevels[Random.Range(0, mEasyLevels.Count - 1)], newPos, Quaternion.identity);
        else
            mCurrentLevel = Instantiate(mHardLevels[Random.Range(0, mEasyLevels.Count - 1)], newPos, Quaternion.identity);

        mLevelsCreated++;
        UpdateSpawnRates();
    }

    private void UpdateSpawnRates()
    {
        if(mLevelsCreated == 5)
        {
            mEasyRate = 50;
            mNormalRate = 40;
            mHardRate = 10;
        }
        else if (mLevelsCreated == 10)
        {
            mEasyRate = 30;
            mNormalRate = 50;
            mHardRate = 20;
        }
        else if (mLevelsCreated == 20)
        {
            mEasyRate = 15;
            mNormalRate = 45;
            mHardRate = 40;
        }
        else if (mLevelsCreated == 30)
        {
            mEasyRate = 5;
            mNormalRate = 50;
            mHardRate = 45;
        }
        else if (mLevelsCreated == 50)
        {
            mEasyRate = 0;
            mNormalRate = 40;
            mHardRate = 60;
        }
        else if (mLevelsCreated == 60)
        {
            mEasyRate = 0;
            mNormalRate = 20;
            mHardRate = 80;
        }
        else if (mLevelsCreated == 70)
        {
            mEasyRate = 0;
            mNormalRate = 0;
            mHardRate = 100;
        }
    }
}
