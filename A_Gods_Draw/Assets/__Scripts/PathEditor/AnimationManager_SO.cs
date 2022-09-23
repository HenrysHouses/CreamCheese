/*
 * Written by:
 * Henrik
 *
 * Script Purpose:
 * Receive and handle animation requests for PathAnimatorController's / and maybe other animations later?
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>Receives and handles animation requests</summary>
[CreateAssetMenu(menuName = "Events/DynamicAnimationManager")]
public class AnimationManager_SO : ScriptableObject
{
    static AnimationManager_SO instance;
    public static AnimationManager_SO getInstance => instance;


    [SerializeField, Tooltip("Current Unhandled Requests")]
    List<animRequestData> AnimationRequest = new List<animRequestData>();
    public List<animRequestData> requests => AnimationRequest;

    /// <summary>invoked when new requests are added</summary>
    [System.NonSerialized]
    public UnityEvent AnimationRequestChangeEvent = new UnityEvent(); 
    /// <summary>count of animations requested in this instance</summary>
    int animNum;

    /// <summary>Request a single animation</summary>
    /// <param name="pathName">The animator that should read this request</param>
    /// <param name="target">GameObject that will be animated</param>
    /// <param name="delay">time delay before animation starts</param>
    /// <param name="coolDown">time cool down before a new animation can start</param>
    /// <param name="animationOverrideOptions">Overrides the settings that are not null</param>
    public void requestAnimation(string pathName, GameObject target, float delay = 0, float coolDown = 0, PathAnimatorController.pathAnimation animationOverrideOptions = null)
    {
        target.name += "_pathAnim" + animNum;
        animRequestData anim = new animRequestData(pathName, target.name, delay, coolDown, animationOverrideOptions);
        AnimationRequest.Add(anim);
        AnimationRequestChangeEvent?.Invoke();
        animNum++;
    }

    /// <summary>Request a ordered list of animations</summary>
    /// <param name="pathName">The animator that should read this request</param>
    /// <param name="targets">GameObject that will be animated in order</param>
    /// <param name="delay">time delay before animation starts</param>
    /// <param name="coolDown">time cool down before a new animation can start</param>
    /// <param name="animationOverrideOptions">Overrides the settings that are not null</param>
    public void requestAnimation(string pathName, GameObject[] targets, float delay = 0, float coolDown = 0, PathAnimatorController.pathAnimation[] animationOverrideOptions = null)
    {
        foreach (var item in targets)
        {
            Debug.Log(item);
        }

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].name += "_pathAnim" + animNum;
            animRequestData anim;
            if(animationOverrideOptions != null)
                anim = new animRequestData(pathName, targets[i].name, delay, coolDown, animationOverrideOptions[i]);
            else
                anim = new animRequestData(pathName, targets[i].name, delay, coolDown, null);
            AnimationRequest.Add(anim);
            animNum++;
        }
        AnimationRequestChangeEvent?.Invoke();
    }

    /// <summary>Removes a named animation</summary>
    /// <param name="targetName">name of the target gameObject that should be animated</param>
    public void removeRequest(string targetName)
    {
        for (int i = 0; i < AnimationRequest.Count; i++)
        {
            if(AnimationRequest[i].target == targetName)
                AnimationRequest.RemoveAt(i);
        }
    }

    // setup
    void OnEnable()
    {
        animNum = 0;
        if(AnimationRequestChangeEvent == null)
            AnimationRequestChangeEvent = new UnityEvent();

        if(!instance)
            instance = this;
        else
            Debug.LogWarning("There are multiple AnimationManager_SO objects. please remove duplicates");
    }

    // remove all requests when not used
    void OnDisable()
    {
        AnimationRequest.Clear();
    }
}


/// <summary>Contains request name, targeted GameObject and animation parameters</summary>
[System.Serializable]
public class animRequestData
{
    public string requestName;
    public string target;
    public PathAnimatorController.pathAnimation anim;
    public float coolDown;
    public float delay;
    public bool requestAccepted;

    public animRequestData(string pathName, string targetName, float delay, float coolDown, PathAnimatorController.pathAnimation animation)
    {
        this.requestName = pathName;
        this.target = targetName;
        this.delay = delay;
        this.coolDown = coolDown;
        this.anim = animation;
    }
}