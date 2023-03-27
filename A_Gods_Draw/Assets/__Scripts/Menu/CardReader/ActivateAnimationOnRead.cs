using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAnimationOnRead : MonoBehaviour
{
    [SerializeField] ReaderTarget target;
    [SerializeField] Animator Animation;

    // Update is called once per frame
    void Update()
    {
        Animation.SetBool("IsOpen",target.isWaitingToReturn);
    }
}
