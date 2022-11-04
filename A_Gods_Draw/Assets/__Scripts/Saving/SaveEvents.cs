using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveEvents : MonoBehaviour
{
    public static System.Action SaveInitiated;
    public static System.Action LoadInitiated;
    public static System.Action DeleteAllInitiated;

    public static void OnSaveInitiated()
    {
        SaveInitiated?.Invoke();
    }
}
