using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSUnlocker : MonoBehaviour
{
    void Start()
    {
        #if PLATFORM_ANDROID
        Application.targetFrameRate = (int)Math.Round(Screen.currentResolution.refreshRateRatio.value);
        #endif 
    }
}
