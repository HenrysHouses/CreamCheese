using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAnimator : MonoBehaviour
{

    public PathController path;

    class AnimatorMovement
    {
        public Transform transform;
        public float t;
        public float speed;
    }
    public float _speed;

    List<AnimatorMovement> movements = new List<AnimatorMovement>();

    public void CreateAnimation(GameObject target)
    {
        AnimatorMovement _animation = new AnimatorMovement();
        _animation.transform = new GameObject("AnimationHolder").transform;
        _animation.transform.SetParent(transform);
        if(_speed > 0)
            _animation.transform.position = path.controlPoints[0].position;
        else
        {
            int n = path.controlPoints.Count -1;
            _animation.transform.position = path.controlPoints[n].position;
            _animation.t = 1;
        }
        _animation.speed = _speed;
        movements.Add(_animation);
        target.transform.SetParent(_animation.transform);
        target.transform.position = new Vector3();

        int index = movements.IndexOf(_animation);
        StartCoroutine(Animate(index));
    }

    IEnumerator Animate(int index)
    {
        bool state = true;

        while(state)
        {
            if(movements[index].speed > 0) {
                if(movements[index].t < 1) {
                    state = true;
                }
            }
            else if(movements[index].speed < 0) {
                if(movements[index].t > 0) {
                    state = true;
                }
            }

            movements[index].t = Mathf.Clamp(movements[index].t + Time.deltaTime * movements[index].speed, 0, 1);
            OrientedPoint OP = path.GetEvenPathOP(movements[index].t); 
            movements[index].transform.position = OP.pos;
            movements[index].transform.rotation = OP.rot;
            yield return new WaitForEndOfFrame();
        }
    }
}