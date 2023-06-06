using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableContinueButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance.shouldGenerateNewMap)
            gameObject.SetActive(false);
    }
}
