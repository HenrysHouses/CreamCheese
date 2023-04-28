/*
 * Written by:
 * Henrik
 * 
 * Purpose:
 * Turns off a highlight or description when its not being hovered over
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableHighlight : MonoBehaviour
{
    [field:SerializeField] public GameObject highlight {get; private set;}
    bool shouldDisable;

    // Update is called once per frame
    void Update()
    {
        if(shouldDisable || GameManager.instance.PauseMenuIsOpen)
            highlight.SetActive(false);

        if(shouldDisable == false && highlight.activeSelf)
            shouldDisable = true;
    }

    public void StayEnabled()
    {
        shouldDisable = false;
    }
}