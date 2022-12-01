using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public abstract class CardAction : Action
{
    public List<BoardElement> targets = new();

    protected NonGod_Behaviour current;

    protected int strengh;

    protected int neededLanes = 1;

    public int Strengh => strengh;

    public EventReference action_SFX;
    public bool PlayOnPlacedOrTriggered_SFX;
    public ActionVFX _VFX;

    public CardAction(int _min, int _max) : base(_min, _max) { strengh = _max; }

    public virtual void Buff(int amount, bool isMult)
    {
        if (isMult)
        {
            strengh *= amount;
        }
        else
        {
            strengh += amount;
        }
    }
    public virtual void DeBuff(int amount, bool isMult)
    {
        if (isMult)
        {
            strengh /= amount;
        }
        else
        {
            strengh -= amount;
        }
    }

    IEnumerator cor;

    public Animator camAnim = Camera.main.GetComponent<Animator>();

    public virtual void SetClickableTargets(BoardStateController board, bool to = true)
    {
        board.SetClickable(3, to);
    }

    public void SetBehaviour(NonGod_Behaviour beh)
    {
        current = beh;
        UpdateNeededLanes(beh);
    }

    protected virtual void UpdateNeededLanes(NonGod_Behaviour beh)
    {
    }

    public override void Execute(BoardStateController board, int strengh, UnityEngine.Object source) { }

    public virtual void OnLanePlaced(BoardStateController board)
    {

    }

    public abstract IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source);

    public virtual void OnPlay(BoardStateController board) { }

    internal bool Ready()
    {
        return isReady;
    }

    public abstract void Reset(BoardStateController board);
    public virtual void OnActionReady(BoardStateController board) { }
    public abstract void ResetCamera();
    public abstract void SetCamera();

    internal void AddTarget(BoardElement target)
    {
        targets.Add(target);
    }

    public bool CanBePlaced(BoardStateController cont)
    {
        return cont.thingsInLane.Count + neededLanes <= 4;
    }

    public IEnumerator playTriggerVFX(GameObject source, IMonster target)
    {
        _VFX.isAnimating= true;
        float time = 0;
        
        ProceduralPathMesh[] meshes = source.GetComponentsInChildren<ProceduralPathMesh>();
        if(meshes.Length > 0)
            GameObject.Destroy(meshes[0].gameObject);

        GameObject _thisVFX = null;
        PathController _path = null;
        if(_VFX.trigger_VFX)
        {
            _thisVFX = GameObject.Instantiate(_VFX.trigger_VFX);
            _path = GameObject.FindGameObjectWithTag("VFXActionPath").GetComponent<PathController>();
            _thisVFX.transform.position = _path.GetEvenPathOP(time).pos;
            _path.startPoint.position = source.transform.position - (source.transform.forward * 0.1f);
            _path.endPoint.position = target.transform.position + (target.transform.forward * 0.1f);
            _path.recalculatePath();
        }

        while(time < 1)
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
        
        if(_VFX.hit_VFX)
        {
            GameObject _hitVFX = GameObject.Instantiate(_VFX.hit_VFX);
            _hitVFX.transform.position = target.transform.position + (target.transform.up * 0.1f);
        }
        _VFX.isAnimating = false;
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