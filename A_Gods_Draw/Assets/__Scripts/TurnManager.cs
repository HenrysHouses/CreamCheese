using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    bool turnEnd;
    [SerializeField]
    DeckManager_SO deckManager;

    [SerializeField]
    Transform[] lanes;
    [SerializeField]
    Transform godLane;

    Card_Behaviour playedCard;

    God_Behaviour currentGod;
    List<NonGod_Behaviour> lane;

    enum State
    {
        StartTurn, PlayerTurn, GodAction, Action, EnemiesTurn, EndTurn
    }

    State currentState = State.StartTurn;

    void Start()
    {
        lane = new List<NonGod_Behaviour>();
        //StartCoroutine(OnTurnStart());
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.StartTurn:
                {
                    currentState = State.PlayerTurn;
                    deckManager.drawCard(5);
                    if (currentGod) { currentGod.OnTurnStart(); }
                    for (int i = 0; i < lanes.Length; i++)
                    {
                        if (i >= deckManager.GetCurrentHand().Count)
                            break;
                        deckManager.GetCurrentHand()[0].cardObject.transform.position = lanes[i].position;

                        Debug.Log(i + " = " + lanes[i].position);

                        Debug.Log("card " + i + " = " + deckManager.GetCurrentHand()[i].cardObject.name);

                        //deckManager.GetCurrentHand().RemoveAt(0);
                    }
                }
                break;

            case State.PlayerTurn:
                {
                    //if (manager.clickedcardinhand???) SelectedCard(manager.clickedcardinhand)
                    if (turnEnd || deckManager.GetCurrentHand().Count == 0) { currentState = State.Action; turnEnd = false; }
                }
                break;

            case State.Action:
                {
                    foreach (NonGod_Behaviour card in lane)
                    {
                        card.OnAction();
                        deckManager.discardCard(card.GetCardSO());
                    }
                    lane.Clear();
                    currentState = State.EnemiesTurn;
                }
                break;

            case State.EnemiesTurn:
                {
                    /*
                     foreach (enemy)
                    {
                        Act
                    }
                    */
                    currentState = State.EndTurn;
                }
                break;

            case State.EndTurn:
                {
                    lane.Clear();
                    deckManager.discardAll();
                    currentState = State.StartTurn;
                }
                break;
        }
    }

    IEnumerator OnTurnStart()
    {
        //deckManager.drawCard(5);
        if (currentGod) { currentGod.OnTurnStart(); }

        while (/*there are cards in hand && */ !turnEnd)
        {
            yield return new WaitUntil(() => { return turnEnd || playedCard; });

            if (turnEnd)
            {
                break;
            }
            else
            {
                playedCard.OnPlay();
                playedCard = null;
            }

        }
        OnTurnEnd();
    }

    public void EndTurn()
    {
        turnEnd = true;
    }

    public void SelectedCard(Card_Behaviour card)
    {
        playedCard = card;

        God_Behaviour a = card as God_Behaviour;
        NonGod_Behaviour b = card as NonGod_Behaviour;

        if (a)
        {
            if (currentGod) { currentGod.OnRetire(lane); }
            currentGod = a;
        }
        else if (b)
        {
            lane.Add(b);
        }
        else
        {
            Debug.Log("WTF have you done");
        }

        playedCard.OnPlay();
        playedCard = null;
    }

    void OnTurnEnd()
    {
        CardsAct();
        EnemiesAct();
    }

    void CardsAct()
    {
        if (currentGod) { currentGod.OnAction(); }

        foreach (Card_Behaviour card in lane)
        {
            card.OnAction();
        }
    }

    void EnemiesAct()
    {

    }
}
