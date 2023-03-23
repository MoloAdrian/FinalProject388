using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class myDeltaTiime 
{
    static float mDeltaModifyer = 1.0f;
    static float GetMyDeltaTime()
    {
        return Time.deltaTime * mDeltaModifyer;
    }


}
