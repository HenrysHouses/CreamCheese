/*
 * Written by:
 * Henrik
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "Events/DynamicAnimationManager")]
public class AnimationManager_SO : ScriptableObject
{
    [SerializeField]
    List<DynamicAnimation> AnimationRequest = new List<DynamicAnimation>();
    public List<DynamicAnimation> requests => AnimationRequest;

    [System.NonSerialized]
    public UnityEvent AnimationRequestChangeEvent = new UnityEvent(); 
    int animNum;

    public void requestAnimation(string pathName, GameObject target, float delay = 0, float coolDown = 0, PathAnimatorController.pathAnimation animation = null)
    {
        target.name += "_pathAnim" + animNum;
        DynamicAnimation anim = new DynamicAnimation(pathName, target.name, delay, coolDown, animation);
        // Debug.Log(target.transform.position);
        AnimationRequest.Add(anim);
        AnimationRequestChangeEvent?.Invoke();
        animNum++;
    }

    public void requestAnimation(string pathName, GameObject[] targets, float delay = 0, float coolDown = 0, PathAnimatorController.pathAnimation[] animations = null)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].name += "_pathAnim" + animNum;
            DynamicAnimation anim;
            if(animations != null)
                anim = new DynamicAnimation(pathName, targets[i].name, delay, coolDown, animations[i]);
            else
                anim = new DynamicAnimation(pathName, targets[i].name, delay, coolDown, null);
            AnimationRequest.Add(anim);
            animNum++;
            Debug.Log("input: " + coolDown + " output: " + anim.coolDown);
        }
        AnimationRequestChangeEvent?.Invoke();
    }

    public void removeRequest(string targetName)
    {
        for (int i = 0; i < AnimationRequest.Count; i++)
        {
            if(AnimationRequest[i].target == targetName)
                AnimationRequest.RemoveAt(i);
        }
    }

    void OnEnable()
    {
        animNum = 0;
        if(AnimationRequestChangeEvent == null)
            AnimationRequestChangeEvent = new UnityEvent();
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        AnimationRequest.Clear();
    }
}


/// <summary>Contains name, target and animation parameters</summary>
[System.Serializable]
public class DynamicAnimation
{
    public string requestName;
    public string target;
    public PathAnimatorController.pathAnimation anim;
    public float coolDown;
    public float delay;

    public DynamicAnimation(string pathName, string targetName, float delay, float coolDown, PathAnimatorController.pathAnimation animation)
    {
        this.requestName = pathName;
        this.target = targetName;
        this.delay = delay;
        this.coolDown = coolDown;
        this.anim = animation;
    }
}