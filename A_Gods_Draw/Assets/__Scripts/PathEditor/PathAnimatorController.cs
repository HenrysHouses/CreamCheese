/*
 * Written by:
 * Henrik
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Collections;

public class PathAnimatorController : MonoBehaviour
{
    [SerializeField]
    AnimationManager_SO manager_SO;
    [SerializeField]
    string _pathName;
    public string AnimationName => _pathName;
    public PathController path;
    [ReadOnly]
    float AnimLength; 
    public float getAnimLength() => AnimLength;
    public AnimationCurve _speedCurve = new AnimationCurve();
    public float Multiplier = 1;

    public bool FreezeRotationX;
    public bool FreezeRotationY;
    public bool FreezeRotationZ;

    [HideInInspector]
    public bool isAnimating;
    [SerializeField]
    bool DbugPositions;

    public GameObject testAnimationObj;

    void OnValidate()
    {
        // calculate anim length
        int fps = 25;
        float meters = path.GetApproxLength();
        float speed = 0;

        for (int i = 0; i < 100; i++)
        {
            speed += _speedCurve.Evaluate(i) * Multiplier;
        }

        float averageSpeedPerFrame = speed/100;
        float speedPerSecond = averageSpeedPerFrame * fps;

        AnimLength = meters / speedPerSecond;
    }


    public class pathAnimation
    {
        public GameObject AnimationTarget;
        public Transform AnimationTransform;
        public float _Time;
        public float length;
        public float t;
        [Tooltip("X Axis: Time, Y Axis: Speed")]
        public AnimationCurve speedCurve;
        public float speedMultiplier;
        public int index;
        public bool FreezeRotationX, FreezeRotationY, FreezeRotationZ;
        public UnityEvent CompletionTrigger;

        public pathAnimation()
        {
            CompletionTrigger = new UnityEvent();
            AnimationTransform = new GameObject("AnimationHolder").transform;
        }
    }

    List<pathAnimation> _Animations = new List<pathAnimation>();


    void OnEnable()
    {
        if(AnimationName == "")
            Debug.LogWarning(this + ": Needs a path name in order to read requests from the animation manager");
        if(!manager_SO)
            return;    
        manager_SO.AnimationRequestChangeEvent.AddListener(checkUpdatedRequests);
        Debug.Log("controller: " + manager_SO.AnimationRequestChangeEvent);
    }

    void OnDisable()
    {
        manager_SO.AnimationRequestChangeEvent.RemoveListener(checkUpdatedRequests);
    }


    void checkUpdatedRequests()
    {
        StartCoroutine(readRequests());
    }

    IEnumerator readRequests()
    {
        List<string> completedRequests = new List<string>();
        foreach (var request in manager_SO.requests)
        {
            if(request.requestName.Equals(AnimationName))
            {
                StartCoroutine(CreateAnimation(request));            
                completedRequests.Add(request.target);
                yield return new WaitForSeconds(request.coolDown);
            }
        }
        foreach (var completed in completedRequests)
        {
            manager_SO.removeRequest(completed);
        }
    }

    public AnimationManager_SO getAnimManagerSO(){ return manager_SO; }

    public pathAnimation getAnimation()
    {
        PathAnimatorController.pathAnimation animation = new PathAnimatorController.pathAnimation();
        // animation.AnimationTarget = null
        animation.FreezeRotationX = FreezeRotationX;
        animation.FreezeRotationY = FreezeRotationY;
        animation.FreezeRotationZ = FreezeRotationZ;
        // animation.AnimationTarget.transform.SetParent(animation.AnimationTransform);
        // animation.AnimationTarget.transform.position = new Vector3(); 
        animation.length = AnimLength;
        animation.speedCurve = _speedCurve;
        animation.speedMultiplier = Multiplier;
        // animation.index = CreateAnimation(animation);
        animation.CompletionTrigger.AddListener(debugTestCompletion);
        return animation;
    }


    public pathAnimation fillMissingInAnimation(pathAnimation anim)
    {
        Debug.Log("filling animation");
        anim.FreezeRotationX = FreezeRotationX;
        anim.FreezeRotationY = FreezeRotationY;
        anim.FreezeRotationZ = FreezeRotationZ;
        
        if(anim.length == 0)
            anim.length = AnimLength;
        if(anim.speedCurve == null)
            anim.speedCurve = _speedCurve;
        if(anim.speedMultiplier == 0)
            anim.speedMultiplier = Multiplier;

        anim.CompletionTrigger.AddListener(debugTestCompletion);

        return anim;
    }

    /// <summary>Starts an animation and returns its index from animations on the path</summary>
    /// <param name="request">Data class for the animation request</param>
    /// <returns>Animation index</returns>
    public IEnumerator CreateAnimation(DynamicAnimation request)
    {
        yield return new WaitForSeconds(request.delay);

        Debug.Log(request.anim);
        if(request.anim == null)
            request.anim = getAnimation();
        else
            request.anim = fillMissingInAnimation(request.anim);



        request.anim.AnimationTarget = GameObject.Find(request.target);
        request.anim.AnimationTarget.transform.SetParent(request.anim.AnimationTransform);
        request.anim.AnimationTarget.transform.position = new Vector3();  

        request.anim.AnimationTransform.SetParent(transform, false);

        if(request.anim.speedMultiplier > 0)
            request.anim.AnimationTransform.position = path.controlPoints[0].position;
        else if(request.anim.speedMultiplier < 0)
        {
            int n = path.controlPoints.Count -1;
            request.anim.AnimationTransform.position = path.controlPoints[n].position;
            request.anim.t = 1;
        }
        else
            Debug.LogWarning("animation has 0 in start speed");
        _Animations.Add(request.anim);

        int index = _Animations.IndexOf(request.anim);
        request.anim.index = index;
    }

    void destroyAnimator(int index)
    {
        Destroy(_Animations[index].AnimationTransform.gameObject);
    }

    void FixedUpdate()
    {
        if(_Animations.Count > 0)
        {
            for (int i = 0; i < _Animations.Count; i++)
            {
                bool state = true;

                // while(state)
                // {
                    isAnimating = true;
                    
                    float currentSpeed = 0;

                    if(_Animations[i].speedMultiplier > 0) {
                        if(_Animations[i].t < 1) 
                        {
                            state = true;
                            currentSpeed = _Animations[i].speedCurve.Evaluate(_Animations[i]._Time/_Animations[i].length) * _Animations[i].speedMultiplier; // ! Needs to change this to be more consistent
                            if(DbugPositions)
                                Debug.Log(_Animations[i].t);
                        }
                        else
                            state = false;
                    }
                    else if(_Animations[i].speedMultiplier < 0) {
                        if(_Animations[i].t > 0) 
                        {
                            state = true;
                            currentSpeed = _Animations[i].speedCurve.Evaluate(1-_Animations[i]._Time/_Animations[i].length) * _Animations[i].speedMultiplier;
                            if(DbugPositions)
                                Debug.Log(1-_Animations[i].t);
                        }
                        else
                            state = false;
                    }

                    // float baseSpeed = _Animations[i].get1DivLength()  Time.deltaTime;

                    _Animations[i].t = Mathf.Clamp(_Animations[i].t + currentSpeed, 0, 1);
                    OrientedPoint OP = path.GetEvenPathOP(_Animations[i].t); 
                    if(!_Animations[i].AnimationTransform)
                        continue;
                    _Animations[i].AnimationTransform.position = OP.pos;
                    Vector3 euler = _Animations[i].AnimationTransform.eulerAngles; 
                    if(!_Animations[i].FreezeRotationX)
                    {
                        _Animations[i].AnimationTransform.eulerAngles = new Vector3(OP.rot.eulerAngles.x, euler.y, euler.z);
                        euler = _Animations[i].AnimationTransform.eulerAngles;
                    }
                    if(!_Animations[i].FreezeRotationY)
                    {
                        _Animations[i].AnimationTransform.eulerAngles = new Vector3(euler.x, OP.rot.eulerAngles.y, euler.z);
                        euler = _Animations[i].AnimationTransform.eulerAngles;
                    }
                    if(!_Animations[i].FreezeRotationZ)
                    {
                        _Animations[i].AnimationTransform.eulerAngles = new Vector3(euler.x, euler.y, OP.rot.eulerAngles.z);
                        euler = _Animations[i].AnimationTransform.eulerAngles;
                    }
                        
                    _Animations[i]._Time += Time.deltaTime;
                // }
                if(!state)
                    StartCoroutine(completeAnimation(i));
            }
        }
    }


    IEnumerator completeAnimation(int index)
    {
        _Animations[index].CompletionTrigger?.Invoke();
        

        yield return new WaitForEndOfFrame(); // ! card is deleted before it can be moved
        yield return new WaitForEndOfFrame();
        
        if(_Animations.Count > index && _Animations[index].AnimationTransform != null)
            Destroy(_Animations[index].AnimationTransform.gameObject);


        for (int i = 0; i < transform.childCount; i++)
        {
            if(!transform.GetChild(i).name.Equals("AnimationHolder") && !isAnimating)
            {
                isAnimating = false;
                StartCoroutine(refreshAnimationList());
            }        
        }
        
    }

    IEnumerator refreshAnimationList()
    {
        yield return new WaitForEndOfFrame();
        if(!isAnimating)
                _Animations.Clear();
    }

    public void debugTestCompletion()
    {
        Debug.Log("TestAnimation Complete");
    }
}