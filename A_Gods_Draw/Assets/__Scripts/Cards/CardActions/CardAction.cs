// Written by Javier Villegas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[System.Serializable]
public abstract class CardAction : Action
{
    protected ActionCard_Behaviour currentCard;
    public EventReference action_SFX;
    public bool PlayOnPlacedOrTriggered_SFX;
    public ActionVFX _VFX;
    public Animator camAnim = Camera.main.GetComponent<Animator>();

    public void SetBehaviour(ActionCard_Behaviour beh)
    {
        currentCard = beh;
        UpdateNeededLanes(beh);
    }

    protected virtual void UpdateNeededLanes(ActionCard_Behaviour source) { }
    public override void Execute(BoardStateController board, int strengh, UnityEngine.Object source) { }
    public virtual void OnLanePlaced(BoardStateController board, ActionCard_Behaviour source) { }
    public abstract IEnumerator OnAction(BoardStateController board, ActionCard_Behaviour source);
    public virtual void OnPlay(BoardStateController board) { }
    public virtual void OnActionReady(BoardStateController board, ActionCard_Behaviour source) { }

    public virtual void Reset(BoardStateController board, Card_Behaviour source)
    {
        isReady = false;
    }

    public IEnumerator playTriggerVFX(GameObject source, Monster target)
    {
        if (_VFX is not null)
        {
            _VFX.isAnimating = true;
            float time = 0;

            ProceduralPathMesh[] meshes = source.GetComponentsInChildren<ProceduralPathMesh>();
            if (meshes.Length > 0)
                GameObject.Destroy(meshes[0].gameObject);

            GameObject _thisVFX = null;
            PathController _path = null;
            if (_VFX.trigger_VFX)
            {
                _thisVFX = GameObject.Instantiate(_VFX.trigger_VFX);
                _path = GameObject.FindGameObjectWithTag("VFXActionPath").GetComponent<PathController>();
                _thisVFX.transform.position = _path.GetEvenPathOP(time).pos;
                _path.startPoint.position = source.transform.position - (source.transform.forward * 0.1f);
                _path.endPoint.position = target.transform.position + (target.transform.forward * 0.1f);
                _path.recalculatePath();
            }

            while (time < 1)
            {
                time = Mathf.Clamp01(time + Time.deltaTime * _VFX.PathSpeed);
                if (_VFX.FollowPath && _thisVFX)
                {
                    _thisVFX.transform.position = _path.GetEvenPathOP(time).pos;
                    _thisVFX.transform.rotation = _path.GetEvenPathOP(time).rot;
                }
                yield return new WaitForEndOfFrame();
            }

            GameObject.Destroy(_thisVFX);

            if (_VFX.hit_VFX)
            {
                GameObject _hitVFX = GameObject.Instantiate(_VFX.hit_VFX);
                _hitVFX.transform.position = target.transform.position + (target.transform.up * 0.1f);
            }
            _VFX.isAnimating = false;
        }
    }

    public IEnumerator playTriggerVFX(GameObject source, Transform target,  Vector3 offset)
    {
        if(_VFX != null)
        {
            _VFX.isAnimating= true;
            float time = 0;
            
            ProceduralPathMesh[] meshes = source.GetComponentsInChildren<ProceduralPathMesh>();
            if(meshes.Length > 0)
                GameObject.Destroy(meshes[0].gameObject);

            GameObject _thisVFX = null;
            PathController _path = null;
            float animTime = 0;
            
            if(_VFX.trigger_VFX)
            {
                _thisVFX = GameObject.Instantiate(_VFX.trigger_VFX);
                _thisVFX.transform.position = source.transform.position + offset;
                Animator animator = _thisVFX.GetComponentInChildren<Animator>();
                animTime = animator.GetCurrentAnimatorStateInfo(0).length;
            }

            while(time < animTime)
            {
                time = Mathf.Clamp01(time + Time.deltaTime * _VFX.PathSpeed);
                if(_VFX.FollowPath && _thisVFX)
                {
                    _thisVFX.transform.position = _path.GetEvenPathOP(time).pos;
                    _thisVFX.transform.rotation = _path.GetEvenPathOP(time).rot;
                }
                yield return new WaitForEndOfFrame();
            }

            GameObject.Destroy(_thisVFX);
            
            if(_VFX.hit_VFX && target != null)
            {
                GameObject _hitVFX = GameObject.Instantiate(_VFX.hit_VFX);
                _hitVFX.transform.position = target.transform.position + (target.transform.up * 0.1f);
            }
            _VFX.isAnimating = false;
        }
    }
}

[System.Serializable]
public class ActionVFX
{
    public bool FollowPath = true;
    public bool isAnimating;
    public float PathSpeed = 1;    
    public GameObject trigger_VFX;
    public GameObject hit_VFX;

}