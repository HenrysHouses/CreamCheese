using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    [SerializeField]
    UnityEvent turnEnd;

    private void OnMouseDown()
    {
        turnEnd.Invoke();
    }
}
