// Written by Javier Villegas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[System.Serializable]
public class BuffCardAction : CardAction
{
    bool multiplies;
    public EventReference cionDrop_SFX;
    List<GameObject> SpawnedCoins = new List<GameObject>();
    public BuffCardAction(){ multiplies = false;}
    public bool HasRelatedGod;
    
    void SpawnCoins(int amount, ActionCard_Behaviour card)
    {
        for (int i = 0; i < amount; i++)
        {
            // Debug.Log("spawning coin");
            GameObject spawn  = GameObject.Instantiate(Resources.Load<GameObject>("Prop_Coin_PRE_v1"));
            SpawnedCoins.Add(spawn);
            spawn.transform.SetParent(card.RootTransform, false);
            Vector3 offsetPos = spawn.transform.localPosition;
            offsetPos.x += Random.Range(-0.0262f, 0.0262f);
            offsetPos.y += Random.Range(-0.09240003f, 0.09240003f);
            offsetPos.z += -1.2137f + ((-1.2137f/amount) * i);
            spawn.transform.localPosition = offsetPos;
        }
        playSFX(card.gameObject);
    }

    public override void Reset(BoardStateController board, Card_Behaviour source)
    {
        foreach (var item in SpawnedCoins)
        {
            GameObject.Destroy(item);
        }
        base.Reset(board, source);
    }

    // public override void SetClickableTargets(BoardStateController board, bool to = true)
    // {
    //     board.SetClickable(0, to);
    // }

    public override IEnumerator OnAction(BoardStateController board, ActionCard_Behaviour source)
    {
        isReady = true;
        yield break;
    }
    
    public override void CardPlaced(BoardStateController board, ActionCard_Behaviour source)
    {
        foreach (ActionCard_Behaviour card in source.AllTargets)
        {
            card.Buff(source.stats.strength, multiplies);
            SpawnCoins(source.stats.strength, card);
            AddGlyphs(card, source.stats.actionGroup);

            if(HasRelatedGod)
                AddGlyphs(card, source.stats.godBuffActions);
        }
        board.RemoveFromLane(currentCard);
        currentCard.StartCoroutine(currentCard.GetComponent<Card_Loader>().DissolveCard(2, currentCard.transform.parent));
        // currentCard.transform.parent.parent.position += Vector3.down * 10;
        currentCard.RemoveFromHand();
    }

    private void AddGlyphs(ActionCard_Behaviour target, ActionGroup source)
    {
        List<CardActionEnum> AddedGlyphs = new List<CardActionEnum>();
        for (int i = 0; i < source.actionStats.Count; i++)
        {   
            CardActionEnum Glyph = source.actionStats[i].actionEnum;

            if(Glyph == CardActionEnum.Buff)
                continue;

            if(Glyph == CardActionEnum.Exhaust)
                continue;

            if(Glyph == CardActionEnum.Offering)
                continue;

            AddedGlyphs.Add(Glyph);
            CardAction act = CardAction.GetAction(Glyph);
            Debug.Log("adding: " + act.GetType() + " to: " + target.name);

            // act.action_SFX = _actionGroup.actionStats[i].action_SFX; // this should be read from a scriptable object for the target action
            // act.PlayOnPlacedOrTriggered_SFX = _actionGroup.actionStats[i].PlayOnPlacedOrTriggered_SFX;
            // act._VFX = _actionGroup.actionStats[i]._VFX;

            act.SetBehaviour(target); 
            target.stats.actionGroup.Add(act); 
            CardActionData _newAction = new CardActionData();
            _newAction.actionEnum = Glyph;
            target.stats.actionGroup.actionStats.Add(_newAction);
        }
        target.spawnTemporaryGlyphs(AddedGlyphs.ToArray(), false, true);
    }

    protected override void UpdateNeededLanes(ActionCard_Behaviour _Behaviour)
    {
        _Behaviour.neededLanes = 0;
    }

    public override void SetActionVFX()
    {
        _VFX = new ActionVFX(false, 0, "", "", 0);
    }
}
