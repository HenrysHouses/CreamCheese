using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    bool turnEnd;
    bool cardOnPlay = false;
    [SerializeField]
    DeckManager_SO deckManager;

    [SerializeField]
    Transform[] lanes;
    [SerializeField]
    Transform godLane;
    [SerializeField]
    PlayerController player;
    [SerializeField]
    List<Enemy> enemies;

    short currentLane = 0;

    Card_Behaviour playedCard;

    God_Behaviour currentGod;
    List<NonGod_Behaviour> lane;

    public enum State
    {
        StartTurn, PlayerTurn, GodAction, Action, EnemiesTurn, EndTurn
    }

    State currentState = State.StartTurn;

    void Start()
    {
        lane = new List<NonGod_Behaviour>();
        deckManager.SetTurnManager(this);

        foreach (Enemy enemy in enemies)
        {
            enemy.SetPlayer(player);
        }
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

                    foreach (Enemy enemy in enemies)
                    {
                        enemy.DecideIntent();
                    }
                    
                }
                break;

            case State.PlayerTurn:
                {

                    if (playedCard)
                    {
                        playedCard.gameObject.transform.position = lanes[currentLane].position;
                        playedCard.gameObject.transform.rotation = lanes[currentLane].rotation;
                        currentLane++;
                        if (playedCard as NonGod_Behaviour)
                            lane.Add(playedCard as NonGod_Behaviour);
                        playedCard = null;
                        cardOnPlay = false;
                        Debug.Log("Select another card");
                    }

                    if (turnEnd || currentLane == lanes.Length) { currentState = State.Action; turnEnd = false; }
                }
                break;

            case State.Action:
                {
                    foreach (NonGod_Behaviour card in lane)
                    {
                        card.OnAction();
                        deckManager.discardCard(card.GetCardSO());
                        card.gameObject.SetActive(false);
                    }
                    lane.Clear();
                    currentState = State.EnemiesTurn;
                    currentLane = 0;
                }
                break;

            case State.EnemiesTurn:
                {
                    foreach (Enemy enemy in enemies)
                    {
                        enemy.Act();
                    }

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

    

    public void EndTurn()
    {
        turnEnd = true;
    }

    public void SelectedCard(Card_Behaviour card)
    {
        if (currentState == State.PlayerTurn)
        {
            God_Behaviour a = card as God_Behaviour;
            NonGod_Behaviour b = card as NonGod_Behaviour;

            cardOnPlay = true;

            StartCoroutine(card.OnPlay(enemies, lane, player, currentGod));

            if (a)
            {
                if (currentGod) { currentGod.OnRetire(lane); }
                currentGod = a;
                foreach (Enemy enemy in enemies)
                {
                    enemy.SetGod(currentGod);
                }
            }
            else if (b)
            {
            }
            else
            {
                Debug.Log("WTF have you done");
            }
        }
    }

    public void FinishedPlay(Card_Behaviour card)
    {
        playedCard = card;
    }

    public State GetState()
    {
        return currentState;
    }

    public bool IsACardSelected()
    {
        return cardOnPlay;
    }

    public List<NonGod_Behaviour> CurrentLane()
    {
        return lane;
    }
}
