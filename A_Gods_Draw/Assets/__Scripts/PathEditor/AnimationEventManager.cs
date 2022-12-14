/*
 * Written by:
 * Henrik
 *
 * Script Purpose:
 * Receive and handle animation requests for PathAnimatorController's / and maybe other animations later?
*/

using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>Receives and handles animation requests</summary>
public class AnimationEventManager : MonoBehaviour
{
    static AnimationEventManager instance;
    public static AnimationEventManager getInstance => instance;


    [SerializeField, Tooltip("Current Unhandled Requests")]

    /// <summary>invoked when new requests are added</summary>
    public event Action<string, animRequestData> OnAnimationRequestChange; 
    // public UnityEvent OnAnimationRequested;

    /// <summary>count of animations requested in this instance</summary>
    int animNum;

    private void Awake() 
    {
        animNum = 0;

        if(!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>Request a single animation</summary>
    /// <param name="pathName">The animator that should read this request</param>
    /// <param name="target">GameObject that will be animated</param>
    /// <param name="delay">time delay before animation starts</param>
    /// <param name="animationOverrideOptions">Overrides the settings that are not null</param>
    public void requestAnimation(string pathName, GameObject target, float delay = 0, PathAnimatorController.pathAnimation animationOverrideOptions = null)
    {
        target.name += "_pathAnim" + animNum;
        animRequestData anim = new animRequestData(pathName, target.name, delay, animationOverrideOptions);
        OnAnimationRequestChange?.Invoke(pathName, anim);
        // OnAnimationRequested?.Invoke();
        animNum++;
    }

    /// <summary>Request a ordered list of animations</summary>
    /// <param name="pathName">The animator that should read this request</param>
    /// <param name="targets">GameObject that will be animated in order</param>
    /// <param name="delay">time delay before animation starts</param>
    /// <param name="animationOverrideOptions">Overrides the settings that are not null</param>
    public void requestAnimation(string pathName, GameObject[] targets, float delay = 0, PathAnimatorController.pathAnimation[] animationOverrideOptions = null)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            animRequestData anim = null;
            targets[i].name += "_pathAnim" + animNum;
            float totalDelay = delay * i;
            if(animationOverrideOptions != null)
                anim = new animRequestData(pathName, targets[i].name, totalDelay, animationOverrideOptions[i]);
            else
                anim = new animRequestData(pathName, targets[i].name, totalDelay, null);
            animNum++;
            if(anim != null)
            {
                OnAnimationRequestChange?.Invoke(pathName, anim);
                // OnAnimationRequested?.Invoke();
            }
        }
    }
}


/// <summary>Contains request name, targeted GameObject and animation parameters</summary>
[System.Serializable]
public class animRequestData
{
    public string requestName;
    public string target;
    public PathAnimatorController.pathAnimation anim;
    public float delay;
    public bool requestAccepted;

    public animRequestData(string pathName, string targetName, float delay, PathAnimatorController.pathAnimation animation)
    {
        this.requestName = pathName;
        this.target = targetName;
        this.delay = delay;
        this.anim = animation;
    }
}