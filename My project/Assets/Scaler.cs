using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{

    public Canvas canvas;
    public PlayerController playerController;
    float resolutionFactor;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        resolutionFactor = 1920.0f / Screen.currentResolution.;
        canvas.scaleFactor = resolutionFactor;
#endif
    }


    // Update is called once per frame
    void Update()
    {
    }
}
