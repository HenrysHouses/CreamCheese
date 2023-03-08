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
    public virtual void CardPlaced(BoardStateController board, ActionCard_Behaviour source) { } //This used to be OnLandePlaced
    public void OnLanePlaced(BoardStateController _board, ActionCard_Behaviour _source) //Modified so we can update the queued damage on enemies
    {
        CardPlaced(_board, _source);
        Debug.Log("Is this why?");
        UpdateQueuedDamage(_source);
    }

    public (bool, int) CardAttackTotal(ActionCard_Behaviour _source)
    {

        int _damageTotal = 0;
        bool _dealsDamage = false;
        foreach (CardAction _action in _source.stats.actionGroup.actions)
        {

            Debug.Log(_action.GetType().Name);

            if(_action is AttackCardAction)
            {

                _dealsDamage = true;
                _damageTotal += _source.stats.strength;

            }
            else if(_action is EarthquakeCardAction)
            {

                _dealsDamage = true;
                _damageTotal += _source.stats.strength;

            }
            else if(_action is LeachCardAction)
            {

                _dealsDamage = true;
                _damageTotal += _source.stats.strength;

            }
            else if(_action is SplashDMGCardAction)
            {

                _dealsDamage = true;
                _damageTotal += (_source.stats.strength / 2); //This has to be updated if the cardAction changes the math for damage

            }
            
        }

        Debug.Log("Total Damage is: " + _damageTotal);

        return (_dealsDamage, _damageTotal);

    }

    public void UpdateQueuedDamage(ActionCard_Behaviour _source, bool _buffUpdate = false)
    {

        Debug.Log("Updating queued damage");

        (bool, int) _damageInfo = CardAttackTotal(_source);
        if(_damageInfo.Item1)
        {

            foreach (Monster _monster in _source.AllTargets)
            {

                _monster.UpdateQueuedDamage(_source, _damageInfo.Item2, _buffUpdate);
                
            }

        }

    }

    public abstract IEnumerator OnAction(BoardStateController board, ActionCard_Behaviour source);
    public virtual void OnPlay(BoardStateController board) { }
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
            DestroyOrder order = null;
            if (_VFX.trigger_VFX)
            {
                _thisVFX = GameObject.Instantiate(_VFX.trigger_VFX);
                _path = GameObject.FindGameObjectWithTag("VFXActionPath").GetComponent<PathController>();
                _thisVFX.transform.position = _path.GetEvenPathOP(time).pos;
                _path.startPoint.position = source.transform.position - (source.transform.forward * 0.1f);
                _path.endPoint.position = target.transform.position + (target.transform.forward * 0.1f);
                _path.recalculatePath();
                
                order = _thisVFX.GetComponent<DestroyOrder>();
                order.destroyVFX();

                if(_VFX.FollowPath && _thisVFX)
                {
                    while (time < 1)
                    {
                        if(_thisVFX == null)
                            time = 1;
                        else
                        {
                            time = Mathf.Clamp01(time + Time.deltaTime * _VFX.PathSpeed);
                            _thisVFX.transform.position = _path.GetEvenPathOP(time).pos;
                            _thisVFX.transform.rotation = _path.GetEvenPathOP(time).rot;
                            yield return new WaitForEndOfFrame();
                        }
                    }
                }
                else
                    yield return new WaitUntil(() => _thisVFX == null);
            }

            if(order)
            {
                order.StopAllParticles();
                order.StopAllAnimations();
            }
            if (_VFX.hit_VFX)
            {
                GameObject _hitVFX = GameObject.Instantiate(_VFX.hit_VFX);
                _hitVFX.transform.position = target.transform.position + (target.transform.up * 0.1f);
                GameObject.Destroy(_hitVFX, _VFX.HitVFXLifeSpan);
            }
            _VFX.isAnimating = false;
        }
    }

    public IEnumerator playTriggerVFX(GameObject source, Transform target,  Vector3 offset)
    {
        if(_VFX != null)
        {
            _VFX.isAnimating = true;
            float time = 0;
            
            ProceduralPathMesh[] meshes = source.GetComponentsInChildren<ProceduralPathMesh>();
            if(meshes.Length > 0)
                GameObject.Destroy(meshes[0].gameObject);

            GameObject _thisVFX = null;
            PathController _path = null;
            DestroyOrder order = null;
            float animTime = 0;
            
            if(_VFX.trigger_VFX)
            {
                _thisVFX = GameObject.Instantiate(_VFX.trigger_VFX);
                _thisVFX.transform.position = source.transform.position + offset;
                Animator animator = _thisVFX.GetComponentInChildren<Animator>();
                order = _thisVFX.GetComponent<DestroyOrder>();
                
                if(_VFX.FollowPath)
                {
                    _path = GameObject.FindGameObjectWithTag("VFXActionPath").GetComponent<PathController>();
                    _thisVFX.transform.position = _path.GetEvenPathOP(time).pos;
                    _path.startPoint.position = source.transform.position - (source.transform.forward * 0.1f);
                    _path.endPoint.position = target.transform.position + (target.transform.forward * 0.1f);
                    _path.recalculatePath();
                }

                if(animator)
                    animTime = animator.GetCurrentAnimatorStateInfo(0).length;

                
                if(order)
                    order.destroyVFX();
            }

            if(_VFX.FollowPath && _thisVFX)
            {
                while(time < animTime)
                {
                    Debug.Log(_path);
                    time = Mathf.Clamp01(time + Time.deltaTime * _VFX.PathSpeed);
                    _thisVFX.transform.position = _path.GetEvenPathOP(time).pos;
                    _thisVFX.transform.rotation = _path.GetEvenPathOP(time).rot;
                    yield return new WaitForEndOfFrame();
                }
            }
            else
                yield return new WaitUntil(() => _thisVFX == null); 
        
            if(order)
            {
                order.StopAllParticles();
                order.StopAllAnimations();
            }
            
            if(_VFX.hit_VFX && target != null)
            {
                GameObject _hitVFX = GameObject.Instantiate(_VFX.hit_VFX);
                _hitVFX.transform.position = target.transform.position + (target.transform.up * 0.1f);
                Debug.Log(target.name);
                GameObject.Destroy(_hitVFX, _VFX.HitVFXLifeSpan);
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
    public float HitVFXLifeSpan = 2f;
}