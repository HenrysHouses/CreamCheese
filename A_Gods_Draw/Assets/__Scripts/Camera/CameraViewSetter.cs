using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CameraViewSetter : MonoBehaviour
{

    public bool shouldSetViewOnStart;
    public CameraView View;

    void Start()
    {
        if(shouldSetViewOnStart)
            CameraMovement.instance.SetCameraView(View);
    }

    public void setView(string view)
    {
        CameraMovement.instance.SetCameraView((CameraView) Enum.Parse(typeof(CameraView), view));
    }
}
