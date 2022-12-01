using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;

public struct ActionGroup
{
    public int nTargets;
    public List<CardAction> actions;

    public ActionGroup(int nOfTargets)
    {
        nTargets = nOfTargets;
        actions = new();
    }

    public CardAction this[int key]
    {
        get => actions[key];
        set => actions[key] = value;
    }

    public int Count => actions.Count;

    public void InitList()
    {
        if (actions == null)
            actions = new();
    }

    public void SetNTargets(int n)
    {
        nTargets = n;
    }

    internal void Add(CardAction act)
    {
        actions.Add(act);
    }
}

public class NonGod_Behaviour : Card_Behaviour
{
    public List<ActionGroup> actions = new();
    public List<ActionGroup> godBuffActions = new();
    public int TargetedActions()
    {

        int aux = 0;
        foreach (var group in actions)
        {
            aux++;
        }
        if (godBuffed)
        {
            foreach (var group in godBuffActions)
            {
                aux++;
            }
        }
        return aux;
    }
    CardType cardType;
    public CardType GetCardType => cardType;
    [SerializeField]
    EventReference SoundClick;

    bool cardIsReady = false;
    public int neededLanes = 1;

    bool godBuffed;

    public void BuffedByGod() { godBuffed = true; }

    protected new NonGod_Card_SO card_so;
    public new NonGod_Card_SO CardSO => card_so;

    Coroutine onSelectedRoutine;
    Coroutine actionRoutine;

    public void Initialize(NonGod_Card_SO card, CardElements elements)
    {
        this.card_so = card;

        actions = new();
        godBuffActions = new();

        for (int i = 0; i < card.targetActions.Count; i++)
        {
            actions.Add(new(card.targetActions[i].numOfTargets));
            actions[i].InitList();

            for (int j = 0; j < card.targetActions[i].Count; j++)
            {
                var act = GetAction(card.targetActions[i][j]);
                act.SetBehaviour(this);
                actions[i].Add(act);

                CardActionData currAction = card.targetActions[i].targetActions[j];

                act.action_SFX = currAction.action_SFX;
                act.PlayOnPlacedOrTriggered_SFX = currAction.PlayOnPlacedOrTriggered_SFX;
                
                act._VFX = currAction._VFX;
            }
        }

        for (int i = 0; i < card.onGodBuff.Count; i++)
        {
            godBuffActions.Add(new(card.onGodBuff[i].numOfTargets));
            godBuffActions[i].InitList();

            for (int j = 0; j < card.onGodBuff[i].Count; j++)
            {
                var act = GetAction(card.onGodBuff[i][j]);
                act.SetBehaviour(this);
                godBuffActions[i].Add(act);
                act.action_SFX = card.onGodBuff[i].targetActions[j].action_SFX;
                act.PlayOnPlacedOrTriggered_SFX = card.onGodBuff[i].targetActions[j].PlayOnPlacedOrTriggered_SFX;
            }
        }

        this.cardType = card.type;
        this.elements = elements;        
    }

    public void Buff(int value, bool isMult)
    {
        foreach (var target in actions)
        {
            foreach (var act in target.actions)
            {
                act.Buff(value, isMult);
            }
        }
        ChangeStrengh(actions[0][card_so.cardStrenghIndex].Strengh);
    }

    public void DeBuff(int value, bool isMult)
    {
        foreach (var target in actions)
        {
            foreach (var act in target.actions)
            {
                act.DeBuff(value, isMult);
            }
        }
        ChangeStrengh(actions[0][card_so.cardStrenghIndex].Strengh);
    }

    //public override void OnClick()
    //{
    //    if (manager.CurrentlySelectedCard() == this)
    //    {
    //        manager.CancelSelection();

    //        //Debug.Log("you clicked me, and im not being played");
    //        return;
    //    }
    //    if (manager.GetState() == TurnManager.State.PlayerTurn && !manager.CurrentlySelectedCard())
    //    {
    //        manager.SelectCard(this);
    //        //Debug.Log("you clicked me, and im being played");
    //    }

    //    //Debug.Log(manager.CurrentlySelectedCard().gameObject);
    //}

    public BoardElement[] getActionTargets(int action)
    {
        return actions[action].actions[0].targets.ToArray();
    }


    public void CheckForGod()
    {
        if (controller.GetBoard().playedGodCard)
            if (card_so.correspondingGod == controller.GetBoard().playedGodCard.CardSO.godAction)
            {
                controller.GetBoard().playedGodCard.Buff(this);
            }
    }

    protected override void OnBeingSelected()
    {
        if (onSelectedRoutine == null)
        {
            if (!CanBeSelected())
            {
                CancelSelection();
                return;
            }
            TurnController.shouldWaitForAnims = true;
            onSelectedRoutine = StartCoroutine(SelectingTargets());
        }
    }

    public void RemoveFromHand()
    {
        controller.Discard(this);
    }

    BoardElement target;

    IEnumerator SelectingTargets()
    {
        CheckForGod();

        foreach (var actionGroup in actions)
        {
            foreach (var act in actionGroup.actions)
            {
                act.SetCamera();
                act.SetClickableTargets(controller.GetBoard(), true);
            }
            for (int i = 0; i < actionGroup.nTargets; i++)
            {
                hasClickedTarget = false;
                missedClick = false;
                yield return new WaitUntil(() => hasClickedTarget);
                foreach (var act in actionGroup.actions)
                {
                    act.AddTarget(target);
                }
                target = null;
            }
            foreach (var act in actionGroup.actions)
            {
                act.OnActionReady(controller.GetBoard());
                act.ResetCamera();
                act.SetClickableTargets(controller.GetBoard(), false);
            }
        }
        if (godBuffed)
        {
            foreach (var actionGroup in godBuffActions)
            {
                foreach (var act in actionGroup.actions)
                {
                    act.SetCamera();
                    act.SetClickableTargets(controller.GetBoard(), true);
                }
                for (int i = 0; i < actionGroup.nTargets; i++)
                {
                    hasClickedTarget = false;
                    missedClick = false;
                    yield return new WaitUntil(() => hasClickedTarget);
                    foreach (var act in actionGroup.actions)
                    {
                        act.AddTarget(target);
                    }
                    target = null;
                }
                foreach (var act in actionGroup.actions)
                {
                    act.OnActionReady(controller.GetBoard());
                    act.ResetCamera();
                    act.SetClickableTargets(controller.GetBoard(), false);
                }
            }
        }

        cardIsReady = true;
        controller.GetBoard().PlayCard(this);
        TurnController.shouldWaitForAnims = false;
    }

    // private void setEnemyHighlight()
    // {
    //     int layer = 1 << 9;
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //     Debug.Log("something");

    //     if(Physics.Raycast(ray, out RaycastHit hit, 10000, layer))
    //     {
    //         IMonster monster = hit.collider.GetComponent<IMonster>();
    //         Debug.Log("hit: " + hit.collider.name);
            
    //         monster.setOutline(monster.outlineSize);
    //     }
    // }

    internal override void OnClickOnSelected()
    {
        base.OnClickOnSelected();
        BoardElement element = TurnController.PlayerClick();

        if (element)
        {
            target = element;
            hasClickedTarget = true;
        }
        else
        {
            MissClick();
            hasClickedTarget = false;
        }
    }

    bool hasClickedTarget = false;

    public override bool ShouldCancelSelection()
    {
        return this == null || missedClick;
    }

    bool AllActionsReady()
    {
        return cardIsReady && onPlayerHand;
    }

    public override void OnAction()
    {
        TurnController.shouldWaitForAnims = true;
        
        StartCoroutine(Play(controller.GetBoard()));
    }

    protected override IEnumerator Play(BoardStateController board)
    {
        foreach (var target in actions)
        {
            foreach (var action in target.actions)
            {
                StartCoroutine(action.OnAction(board, this));
                if (action.PlayOnPlacedOrTriggered_SFX)
                {
                    SoundPlayer.PlaySound(action.action_SFX, gameObject); 
                }
                yield return new WaitUntil(() => action.Ready());
            }
        }
        if (godBuffed)
            foreach (var target in godBuffActions)
            {
                foreach (var action in target.actions)
                {
                    StartCoroutine(action.OnAction(board));
                    if (action.PlayOnPlacedOrTriggered_SFX)
                    {
                        SoundPlayer.PlaySound(action.action_SFX, gameObject);
                    }
                    yield return new WaitUntil(() => action.Ready());
                }
            }

        yield return new WaitForSeconds(0.2f);

        foreach (var target in actions)
        {
            foreach (var action in target.actions)
            {
                action.Reset(board);
            }
        }
        if (godBuffed)
            foreach (var target in godBuffActions)
            {
                foreach (var action in target.actions)
                {
                    action.Reset(board);
                }
            }

        controller.Discard(this);
        Destroy(transform.parent.parent.gameObject);
        TurnController.shouldWaitForAnims = false;
    }

    public override void CancelSelection()
    {
        //if (this == null)
            //return true;

        base.CancelSelection();
        if (onSelectedRoutine != null)
            StopCoroutine(onSelectedRoutine);
        if (actionRoutine != null)
            StopCoroutine(actionRoutine);
        onSelectedRoutine = null;
        actionRoutine = null;
        target = null;

        StopAllCoroutines();

        foreach (var target in actions)
        {
            foreach (var action in target.actions)
            {
                action.Reset(controller.GetBoard());
            }
        }

        foreach (var target in godBuffActions)
        {
            foreach (var action in target.actions)
            {
                action.Reset(controller.GetBoard());
            }
        }

        RemoveGodBuff();
    }

    private void RemoveGodBuff()
    {
        if (godBuffed)
        {
            if (controller.GetBoard().playedGodCard)
                if (card_so.correspondingGod == controller.GetBoard().playedGodCard.CardSO.godAction)
                {
                    controller.GetBoard().playedGodCard.DeBuff(this);
                }
            godBuffed = false;
        }
    }

    bool missedClick = false;
    private bool HasMissedClick()
    {
        if (missedClick)
        {
            missedClick = false;
            return true;
        }
        return false;
    }
    public void MissClick()
    {
        missedClick = true;
        CancelSelection();
    }

    public override bool CardIsReady()
    {
        return AllActionsReady();
    }
    public override void OnPlacedInLane()
    {
        foreach (var target in actions)
        {
            foreach (var action in target.actions)
            {
                action.OnLanePlaced(controller.GetBoard());
            }
        }
        if (godBuffed)
        {
            foreach (var target in godBuffActions)
            {
                foreach (var action in target.actions)
                {
                    action.OnLanePlaced(controller.GetBoard());
                }
            }
        }

        controller.PlacedCard();

        missedClick = true;
    }

    public override bool CanBeSelected()
    {
        return base.CanBeSelected() && (controller.GetBoard().thingsInLane.Count + neededLanes <= 4);
    }
}
