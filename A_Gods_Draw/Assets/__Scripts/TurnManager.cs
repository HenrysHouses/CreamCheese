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
                        playedCard = null;
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

            StartCoroutine(card.OnPlay(enemies, lane, player, currentGod));

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
}
