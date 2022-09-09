/*
 * Written by:
 * Henrik
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathAnimatorController : MonoBehaviour
{
    [SerializeField]
    AnimationManager_SO manager_SO;
    [SerializeField]
    string _pathName;
    public string PathName => _pathName;
    public PathController path;
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

    public class pathAnimation
    {
        public GameObject AnimationTarget;
        public Transform AnimationTransform;
        public float t;
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
        if(PathName == "")
            Debug.LogWarning(this + ": Needs a path name in order to read requests from the animation manager");
        if(!manager_SO)
            return;    
        manager_SO.AnimationRequestChangeEvent.AddListener(readRequests);
        Debug.Log("controller: " + manager_SO.AnimationRequestChangeEvent);
    }

    void OnDisable()
    {
        if(manager_SO)
            manager_SO.AnimationRequestChangeEvent.RemoveListener(readRequests);
    }

    void readRequests()
    {
        List<string> completedRequests = new List<string>();
        foreach (var request in manager_SO.requests)
        {
            if(request.requestName.Equals(PathName))
            {
                CreateAnimation(request);            
                completedRequests.Add(request.target);
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
        animation.speedCurve = _speedCurve;
        animation.speedMultiplier = Multiplier;
        // animation.index = CreateAnimation(animation);
        animation.CompletionTrigger.AddListener(debugTestCompletion);
        return animation;
    }

    /// <summary>Starts an animation and returns its index from animations on the path</summary>
    /// <param name="request">Data class for the animation request</param>
    /// <returns>Animation index</returns>
    public int CreateAnimation(DynamicAnimation request)
    {
        if(request.anim == null)
            request.anim = getAnimation();

        request.anim.AnimationTarget = GameObject.Find(request.target);
        // Debug.Log(request.target);
        request.anim.AnimationTarget.transform.SetParent(request.anim.AnimationTransform);
        request.anim.AnimationTarget.transform.position = new Vector3();  

        request.anim.AnimationTransform.SetParent(transform);

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

        StartCoroutine(Animate(index));
        return index;
    }

    void destroyAnimator(int index)
    {
        Destroy(_Animations[index].AnimationTransform.gameObject);
    }

    IEnumerator Animate(int index)
    {
        bool state = true;
        float _timer = 0;

        while(state)
        {
            isAnimating = true;
            
            float currentSpeed = 0;

            if(_Animations[index].speedMultiplier > 0) {
                if(_Animations[index].t < 1) 
                {
                    state = true;
                    currentSpeed = _Animations[index].speedCurve.Evaluate(_timer) * _Animations[index].speedMultiplier; // ! Needs to change this to be more consistent
                    if(DbugPositions)
                        Debug.Log(_timer);
                }
                else
                    state = false;
            }
            else if(_Animations[index].speedMultiplier < 0) {
                if(_Animations[index].t > 0) 
                {
                    state = true;
                    currentSpeed = _Animations[index].speedCurve.Evaluate(1-_timer) * _Animations[index].speedMultiplier;
                    if(DbugPositions)
                        Debug.Log(1-_timer);
                }
                else
                    state = false;
            }

            _Animations[index].t = Mathf.Clamp(_Animations[index].t + currentSpeed, 0, 1);
            OrientedPoint OP = path.GetEvenPathOP(_Animations[index].t); 
            _Animations[index].AnimationTransform.position = OP.pos;
            Vector3 euler = _Animations[index].AnimationTransform.eulerAngles; 
            if(!_Animations[index].FreezeRotationX)
            {
                _Animations[index].AnimationTransform.eulerAngles = new Vector3(OP.rot.eulerAngles.x, euler.y, euler.z);
                euler = _Animations[index].AnimationTransform.eulerAngles;
            }
            if(!_Animations[index].FreezeRotationY)
            {
                _Animations[index].AnimationTransform.eulerAngles = new Vector3(euler.x, OP.rot.eulerAngles.y, euler.z);
                euler = _Animations[index].AnimationTransform.eulerAngles;
            }
            if(!_Animations[index].FreezeRotationZ)
            {
                _Animations[index].AnimationTransform.eulerAngles = new Vector3(euler.x, euler.y, OP.rot.eulerAngles.z);
                euler = _Animations[index].AnimationTransform.eulerAngles;
            }
                
            _timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        completeAnimation(index);
    }


    void completeAnimation(int index)
    {
        _Animations[index].CompletionTrigger?.Invoke();
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