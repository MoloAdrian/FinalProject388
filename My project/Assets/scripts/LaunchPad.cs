using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    public Vector2 mDirection = new Vector2(0, 0);
    public float mForce = 100f;
    public float mAccuracyValue = 0;
    public bool mExtraJump = true;
    public bool mLastLP = false;
    public Vector2 mRandomOffset = new Vector2(0, 0);
    public Vector2 mVelocity = new Vector2(0, 0);
    public Vector2 mMovementRange = new Vector2(0, 0);
    public Vector3 mStartingPos = new Vector3(0, 0, 0);
    public float mJumpBarFactor = 1f;
    public bool mTriggered = false;
    public bool mCancelPlayerVel = false;

    [SerializeField] private Material mCooldownMat;
    [SerializeField] private Material mNormalMat;

    private float mCd = 0f;
    private float mMaxCD = 3f;
    private float mT = 0f;

    private void Update()
    {
        transform.position = mStartingPos + new Vector3(Mathf.Sin(mVelocity.x * mT) * mMovementRange.x, Mathf.Sin(mVelocity.y * mT) * mMovementRange.y, 0);

        mT += Time.deltaTime;

        //Update cooldown
        if (mCd != 0f)
        {
            mCd += Time.deltaTime;
            if (mCd >= mMaxCD)
            {
                mCd = 0f;
                gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Vector4(64/255.0f, 184/255.0f, 255/255.0f, 255/255.0f);

            }
        }
    }

    public void StartCooldown()
    {
        mCd += Time.deltaTime;
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Vector4(64 / 255.0f, 184 / 255.0f, 255 / 255.0f, 50 / 255.0f);
    }
    public bool IsInCooldown()
    {
        return mCd != 0f;
    }
}
