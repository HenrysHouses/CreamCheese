/*
 * Written by:
 * Henrik
 *
 * Script Purpose: 
 * Reads requests from the animation manager, and animates the requested gameObjects through its path
 * Can be used for dynamic animations
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Collections;

/// <summary>Reads requests from the animation manager, and animates the requested gameObjects through its path</summary>
public class PathAnimatorController : MonoBehaviour
{
    // [SerializeField, Tooltip("Required to read animation requests")]
    // AnimationManager_SO manager_SO;

    /// <summary>Current animations on this path</summary>
    [SerializeField] List<pathAnimation> _Animations = new List<pathAnimation>();

    [SerializeField, Tooltip("Name of this path, Used to identify which path accepts requested animations")]
    string _pathName;
    public string AnimationName => _pathName;
    [Tooltip("The path used for the animation")] 
    public PathController path;
    [ReadOnly] float AnimLength; 
    [Tooltip("Variable animation speed through the path")]
    public AnimationCurve _speedCurve = new AnimationCurve();
    [Tooltip("Animation speed multiplier")]
    public float Multiplier = 1;

    public bool FreezeRotationX;
    public bool FreezeRotationY;
    public bool FreezeRotationZ;

    /// <summary>Prevents lists from updating while being used</summary>
    public bool isAnimating;
    /// <summary>Prevents lists from updating while being used</summary>
    public bool isReadingRequests;

    [Tooltip("The GameObject used in the test animation")]
    public GameObject testAnimationObj;

    void OnValidate()
    {
        calculateApproximateAnimLength();
    }

    void calculateApproximateAnimLength()
    {
        // calculate estimated animation length
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

    /// <summary>Data class for path animations, can be used to override the path's settings</summary>
    public class pathAnimation
    {
        /// <summary>The GameObject that will be animated</summary>
        public GameObject AnimationTarget;
        /// <summary>Transform that moves through the path</summary>
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
        public bool _Complete;

        public pathAnimation()
        {
            CompletionTrigger = new UnityEvent();
            AnimationTransform = new GameObject("AnimationHolder").transform;
        }

        public void completionTrigger()
        {
            CompletionTrigger?.Invoke();
            _Complete = true;
        }
    }


    void Start()
    {
        isAnimating = false;
        calculateApproximateAnimLength();
        // isReadingRequests = false;
    }

    void OnEnable()
    {
        if(AnimationName == "")
            Debug.LogWarning(this + ": Needs a path name in order to read requests from the animation manager");
        if(!AnimationManager_SO.getInstance)
            return;    
        // Listen to when the animation manager has new animation requests
        AnimationManager_SO.getInstance.OnAnimationRequestChange += checkUpdatedRequests;

    }

    void OnDisable()
    {
        // Remove the listener to avoid unintended behaviour
        AnimationManager_SO.getInstance.OnAnimationRequestChange -= checkUpdatedRequests;
    }

    /// <summary>Reads requests and waits for animation cool downs</summary>
    void checkUpdatedRequests(string id, animRequestData anim)
    {
        StartCoroutine(readRequests(id, anim));
    }

    IEnumerator readRequests(string id, animRequestData anim)
    {
        if(id.Equals(_pathName))
        {
            yield return new WaitForSeconds(anim.delay);
            StartCoroutine(CreateAnimation(anim));
            // prep remove accepted request
            yield return new WaitForSeconds(anim.coolDown);
        }
    }

    /// <summary>Get all the current settings for the animation</summary>
    /// <returns>Data class containing all the animation settings</returns>
    public pathAnimation getAnimation()
    {
        PathAnimatorController.pathAnimation animation = new PathAnimatorController.pathAnimation();
        animation.FreezeRotationX = FreezeRotationX;
        animation.FreezeRotationY = FreezeRotationY;
        animation.FreezeRotationZ = FreezeRotationZ;
        animation.length = AnimLength;
        animation.speedCurve = _speedCurve;
        animation.speedMultiplier = Multiplier;
        // animation.CompletionTrigger.AddListener(debugTestCompletion);
        return animation;
    }

    /// <summary>Fill missing animation settings for overridden animations</summary>
    /// <param name="anim">animation settings to override</param>
    /// <returns>Data class containing all the animation settings</returns>
    public pathAnimation fillMissingInAnimation(pathAnimation anim)
    {
        anim.FreezeRotationX = FreezeRotationX;
        anim.FreezeRotationY = FreezeRotationY;
        anim.FreezeRotationZ = FreezeRotationZ;
        
        if(anim.length == 0)
            anim.length = AnimLength;
        if(anim.speedCurve == null)
            anim.speedCurve = _speedCurve;
        if(anim.speedMultiplier == 0)
            anim.speedMultiplier = Multiplier;

        // anim.CompletionTrigger.AddListener(debugTestCompletion);

        return anim;
    }

    /// <summary>Starts an animation and sets its index from current animations on the path</summary>
    /// <param name="request">Data class for the animation request</param>
    public IEnumerator CreateAnimation(animRequestData request)
    {
        yield return new WaitForSeconds(request.delay);

        // Debug.Log(request.anim);

        // get animation settings if there is no overridden settings
        if(request.anim == null)
        {
            request.anim = getAnimation();
        }
        else
        {
            request.anim = fillMissingInAnimation(request.anim);
        }

        // Finds and moves the target that should be animated
        request.anim.AnimationTarget = GameObject.Find(request.target);
        request.anim.AnimationTarget.transform.SetParent(request.anim.AnimationTransform);
        request.anim.AnimationTarget.transform.position = new Vector3();  
        request.anim.AnimationTransform.SetParent(transform, false);

        // Decides if the animation is played regularly or in reverse.
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
        
        // Adds the animation to current animations
        _Animations.Add(request.anim);

        // Sets the animation index
        int index = _Animations.IndexOf(request.anim);
        request.anim.index = index;
    }

    /// <summary>Destroys the animation transform</summary>
    void destroyAnimator(int index)
    {
        Destroy(_Animations[index].AnimationTransform.gameObject);
    }

    // ? might be more efficient to not keep this in an update.
    // ! When this is not in a fixed update the animations become fps dependant
    // # could clean this code up for readability
    void FixedUpdate()
    {
        if(_Animations.Count > 0)
        {
            for (int i = 0; i < _Animations.Count; i++)
            {
                // Animation completion state. Gets set to false if its still ongoing. // ? should maybe be inverted but whatever
                bool state = true; 

                // while(state) // was used then this was not in update
                // {

                    float currentSpeed = 0;

                    // if animation should move in // # positive direction
                    if(_Animations[i].speedMultiplier > 0) {
                        if(_Animations[i].t < 1) 
                        {
                            state = true;
                            currentSpeed = _Animations[i].speedCurve.Evaluate(_Animations[i]._Time/_Animations[i].length) * _Animations[i].speedMultiplier; // ! Needs to change this to be more consistent
                            //if(DbugPositions)
                                //Debug.Log(_Animations[i].t);
                        }
                        else
                            state = false;
                    }
                    // if animation should move in // # negative direction
                    else if(_Animations[i].speedMultiplier < 0) {
                        if(_Animations[i].t > 0) 
                        {
                            state = true;
                            currentSpeed = _Animations[i].speedCurve.Evaluate(1-_Animations[i]._Time/_Animations[i].length) * _Animations[i].speedMultiplier;
                        }
                        else
                            state = false;
                    }

                    if(state)
                        isAnimating = true;
                    
                    // Update the animation's position on the path
                    _Animations[i].t = Mathf.Clamp(_Animations[i].t + currentSpeed, 0, 1);
                    OrientedPoint OP = path.GetEvenPathOP(_Animations[i].t); 
                    // ? Skip this animation's frame if the animation dont have a transform
                    if(!_Animations[i].AnimationTransform)
                        continue;

                    // Update the animation transform's new position
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
                    
                    // advance animation time
                    _Animations[i]._Time += Time.deltaTime;
                // } // end of while(state)
                if(!state)
                    completeAnimation(i);
            }
        }
    }

    /// <summary>Invoke the animation's Completion event, remove it from current animations</summary>
    /// <param name="index">Index of current animations on the controller</param>
    void completeAnimation(int index)
    {
        if(!_Animations[index]._Complete)
        {
            _Animations[index].completionTrigger();
        }
        
        // Destroys the animation transform        
        if(_Animations.Count > index && _Animations[index].AnimationTransform != null)
            Destroy(_Animations[index].AnimationTransform.gameObject);

        // Checks if there are still ongoing animations in order to clear the animation list
        for (int i = 0; i < transform.childCount; i++)
        {
            if(!transform.GetChild(i).name.Equals("AnimationHolder") && isAnimating)
            {
                isAnimating = false;
                StartCoroutine(refreshAnimationList());
            }        
        }
    }

    /// <summary>Clears the list of current animations if all animations are done</summary>
    IEnumerator refreshAnimationList()
    {
        yield return new WaitForEndOfFrame();
        if(!isAnimating)
                _Animations.Clear();
    }
    
    /// <returns>Estimated animation time</returns>
    public float getAnimLength() => AnimLength;

    public void debugTestCompletion()
    {
        //Debug.Log("TestAnimation Complete");
    }
}