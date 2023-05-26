using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraShaker.Effects;

public class EarthquakeShake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CameraEffects.ShakeOnce(1f, 1.5f);
    }
}
