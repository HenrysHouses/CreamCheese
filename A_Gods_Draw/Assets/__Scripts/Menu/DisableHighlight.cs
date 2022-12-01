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
    [SerializeField] GameObject highlight;
    bool shouldDisable;

    // Update is called once per frame
    void Update()
    {
        if(shouldDisable)
            highlight.SetActive(false);

        if(shouldDisable == false && highlight.activeSelf)
            shouldDisable = true;
    }

    public void StayEnabled()
    {
        shouldDisable = false;
    }
}