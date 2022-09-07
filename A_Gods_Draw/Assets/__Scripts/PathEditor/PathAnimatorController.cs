using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathAnimatorController : MonoBehaviour
{

    public PathController path;

    public class AnimatorMovement
    {
        public GameObject AnimationTarget;
        public Transform AnimationTransform;
        public float t;
        public float speed;
        public int index;
        public UnityEvent CompletionTrigger;

        public AnimatorMovement(float animationSpeed)
        {
            CompletionTrigger = new UnityEvent();
            AnimationTransform = new GameObject("AnimationHolder").transform;
            speed = animationSpeed;
        }
    }

    public float _speed;
    [SerializeField]
    bool isAnimating;

    List<AnimatorMovement> _Animations = new List<AnimatorMovement>();

    /// <summary>Starts an animation and returns its index from animations on the path</summary>
    /// <param name="animator">Data class for the animation</param>
    /// <returns>Animation index</returns>
    public int CreateAnimation(AnimatorMovement animator)
    {
        animator.AnimationTransform.SetParent(transform);

        if(_speed > 0)
            animator.AnimationTransform.position = path.controlPoints[0].position;
        else
        {
            int n = path.controlPoints.Count -1;
            animator.AnimationTransform.position = path.controlPoints[n].position;
            animator.t = 1;
        }
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

        while(state)
        {
            isAnimating = true;
            if(_Animations[index].speed > 0) {
                if(_Animations[index].t < 1) 
                    state = true;
                else
                    state = false;
            }
            else if(_Animations[index].speed < 0) {
                if(_Animations[index].t > 0) 
                    state = true;
                else
                    state = false;
            }

            _Animations[index].t = Mathf.Clamp(_Animations[index].t + Time.deltaTime * _Animations[index].speed, 0, 1);
            OrientedPoint OP = path.GetEvenPathOP(_Animations[index].t); 
            _Animations[index].AnimationTransform.position = OP.pos;
            _Animations[index].AnimationTransform.rotation = OP.rot;
            yield return new WaitForEndOfFrame();
        }
        completeAnimation(index);
    }

    void completeAnimation(int index)
    {
        Debug.Log("animation Complete: " + index);
        _Animations[index].CompletionTrigger.Invoke();
        Debug.Log(_Animations[index].CompletionTrigger.GetPersistentEventCount());
        Destroy(_Animations[index].AnimationTransform.gameObject);
        for (int i = 0; i < transform.childCount; i++)
        {
            if(!transform.GetChild(i).name.Equals("AnimationHolder"))
            {
                isAnimating = false;
                _Animations = new List<AnimatorMovement>();
            }        
        }
    }

    public void debugTestCompletion()
    {
        Debug.Log("TestAnimation Complete");
    }
}