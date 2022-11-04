using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueID : MonoBehaviour
{
    public string ID { get; private set; }

    private void Awake()
    {
        //? can use GetSceneIndex if we want to say that the item is from a specific scene
        ID = transform.position.sqrMagnitude + "-" + name + "-" + transform.GetSiblingIndex();
        Debug.Log("ID for " + name + " is " + ID);
    }
}
