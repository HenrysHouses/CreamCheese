using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathAnimatorController : MonoBehaviour
{

    public PathController path;
    public AnimationCurve _speedCurve = new AnimationCurve();
    public float Multiplier = 1;

    [HideInInspector]
    public bool isAnimating;
    [SerializeField]
    bool DbugPositions;

    public GameObject testAnimationobj;

    public class AnimatorMovement
    {
        public GameObject AnimationTarget;
        public Transform AnimationTransform;
        public float t;
        public AnimationCurve speedCurve;
        public float speedMultiplier;
        public int index;
        public UnityEvent CompletionTrigger;

        public AnimatorMovement()
        {
            CompletionTrigger = new UnityEvent();
            AnimationTransform = new GameObject("AnimationHolder").transform;
        }
    }

    List<AnimatorMovement> _Animations = new List<AnimatorMovement>();

    /// <summary>Starts an animation and returns its index from animations on the path</summary>
    /// <param name="animator">Data class for the animation</param>
    /// <returns>Animation index</returns>
    public int CreateAnimation(AnimatorMovement animator)
    {
        animator.AnimationTransform.SetParent(transform);

        if(animator.speedMultiplier > 0)
            animator.AnimationTransform.position = path.controlPoints[0].position;
        else if(animator.speedMultiplier < 0)
        {
            int n = path.controlPoints.Count -1;
            animator.AnimationTransform.position = path.controlPoints[n].position;
            animator.t = 1;
        }
        else
            Debug.LogWarning("animation has 0 in start speed");
        _Animations.Add(animator);

        int index = _Animations.IndexOf(animator);
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
                    currentSpeed = _Animations[index].speedCurve.Evaluate(_timer) * _Animations[index].speedMultiplier;
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
            _Animations[index].AnimationTransform.rotation = OP.rot;
            _timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        completeAnimation(index);
    }

    void completeAnimation(int index)
    {
        _Animations[index].CompletionTrigger?.Invoke();
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
                _Animations = new List<AnimatorMovement>();
    }

    public void debugTestCompletion()
    {
        Debug.Log("TestAnimation Complete");
    }
}