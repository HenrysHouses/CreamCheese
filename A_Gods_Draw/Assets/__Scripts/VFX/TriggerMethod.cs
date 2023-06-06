using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerMethod : MonoBehaviour
{
    public UnityEvent OnTrigger;

    public void Invoke() => OnTrigger?.Invoke();
}
