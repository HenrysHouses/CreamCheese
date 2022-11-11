using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class God_Behaviour : Card_Behaviour
{
    int maxHealth;
    [SerializeField]
    int health;
    public int Health => health;

    GodCardAction action;

    GodPlacement godPlacement;

    int defendFor;

    protected new God_Card_SO card_so;
    public new God_Card_SO CardSO => card_so;

    public void Initialize(God_Card_SO card, CardElements elements)
    {
        this.card_so = card;
        maxHealth = card.health;
        health = maxHealth;

        action = GetAction(card.godAction);

        this.elements = elements;
    }


    public void SetPlace(GodPlacement place)
    {
        godPlacement = place;
    }

    protected override IEnumerator Play(BoardStateController board)
    {
        foreach (NonGod_Behaviour card in board.playedCards)
        {
            if (card.CardSO.correspondingGod == card_so.godAction)
            {
                card.Buff(card_so.strengh, true);
            }
        }

        action.OnPlay(board);

        //Wait for animations, etc
        yield return new WaitUntil(() => true /* action.IsReady() */);

        TurnController.shouldWaitForAnims = false;
    }
    public void OnRetire(BoardStateController board)
    {
        foreach (NonGod_Behaviour card in board.playedCards)
        {
            if (card.CardSO.correspondingGod == card_so.godAction)
            {
                card.DeBuff(card_so.strengh, true);
            }
        }
    }

    internal void Buff(NonGod_Behaviour nonGod_Behaviour)
    {
        nonGod_Behaviour.Buff(card_so.strengh, true);
    }

    public void DealDamage(int amount, UnityEngine.Object source)
    {
        //Debug.Log("God damaged, defended for: " + defendFor);

        if (amount > defendFor)
        {
            health -= amount + defendFor;
            card_so.StartDialogue(GodDialogueTrigger.Hurt, source);
            defendFor = 0;

            godPlacement.UpdateUI();
        }
        else
        {
            defendFor -= amount;
        }

        //Debug.Log("God damaged, health left: " + health);

        if (health <= 0)
        {
            health = 0;

            godPlacement.UpdateUI();
            godPlacement.RemoveGod();
            controller.GodDied(this);
        }
    }

    //public void CanBeDefendedBy(Defense_Behaviour defense_Behaviour)
    //{
    //    posibleDefender = defense_Behaviour;
    //}

    //private void OnMouseDown()
    //{
    //    if (posibleDefender)
    //    {
    //        posibleDefender.ItDefends(null, this);
    //    }
    //    posibleDefender = null;
    //}

    public void Defend(int amount)
    {
        defendFor += amount;
    }

    private void OnMouseOver()
    {
        if (onPlayerHand)
            godPlacement.godArrow.color = Color.magenta;
    }

    private void OnMouseExit()
    {
        if (onPlayerHand)
            godPlacement.godArrow.color = Color.white;
    }
    public int GetStrengh()
    {
        return card_so.strengh;
    }

    public virtual void OnTurnStart() { }

    protected override void OnBeingSelected()
    {
        controller.GodPlacement.SetGod(this);
        StartCoroutine(Play(controller.GetBoard()));
    }

    public override void OnAction()
    {
        action.Act(controller.GetBoard(), 0);
    }

    public override bool CardIsReady()
    {
        return true;
    }

}
