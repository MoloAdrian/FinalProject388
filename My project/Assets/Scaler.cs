using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    public float mResolutionFactor;
    // Start is called before the first frame update
    void Start()
    {
//#if UNITY_ANDROID
        mResolutionFactor =  1080f/1920f - (Screen.width/Screen.height);
//#endif
    }
}
