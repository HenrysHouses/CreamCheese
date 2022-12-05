/* 
 * Written by 
 * Henrik
*/

using UnityEngine;

/// <summary>Spawn transform for a DialogueBox</summary>
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
