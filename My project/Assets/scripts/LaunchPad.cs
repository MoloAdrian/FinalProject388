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
}
