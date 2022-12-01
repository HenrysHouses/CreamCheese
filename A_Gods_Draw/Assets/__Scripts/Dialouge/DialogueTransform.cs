/* 
 * Written by 
 * Henrik
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTransform : MonoBehaviour
{
    public string TransformName;

    public bool hasDialogue()
    {
        if(transform.childCount > 0)
            return true;
        return false;
    }
}
