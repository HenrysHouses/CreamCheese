using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAnimationHelper : MonoBehaviour
{
    [SerializeField] PathController Path;
    [SerializeField] Animator animator;

    /// <summary>Use an animator on this value to animate this object on the path</summary>
    public float pathPosition;
    [field:SerializeField] public bool isAnimating {get; private set;}
    public bool useRotation {get; private set;}
    [SerializeField] bool flipRotation;
    public bool isLooping {get; private set;}
    public void Set(PathController path, bool useRotation, bool loop)
    {
        this.useRotation = useRotation;
        Path = path; 
        isLooping = loop;
    }

    public void startAnimating(string animation)
    {
        if(isAnimating)
            return;

        isAnimating = true;
        animator.Play(animation);
        StartCoroutine(Animate());     
    }

    public void stopAnimating()
    {
        isAnimating = false;
        animator.StopPlayback();
        StopAllCoroutines();
    }

    IEnumerator Animate()
    {
        animator.Update(Time.deltaTime);
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        float timePassed = 0;

        while (isAnimating)
        {
            OrientedPoint OP = Path.GetEvenPathOP(pathPosition);
            transform.position = OP.pos;

            if(useRotation)
                transform.rotation = OP.rot;

            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        
            if(timePassed >= animationDuration && !isLooping)
                isAnimating = false;
        
            Debug.Log("animating heheheheh: " + timePassed + " / " + animationDuration + ", " + isLooping);
        }

        animator.StopPlayback();
    }
}
