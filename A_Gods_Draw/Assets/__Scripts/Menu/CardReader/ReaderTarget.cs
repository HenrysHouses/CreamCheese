using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaderTarget : MonoBehaviour
{
    public Quaternion UpRightRotation;
    public Quaternion DefaultRotation => _defaultRotation;
    Quaternion _defaultRotation;
    public bool isBeingInspected;
    public bool isWaitingToReturn;
    public DisableHighlight information;

    void Start()
    {
        _defaultRotation = transform.localRotation;
    }
}
