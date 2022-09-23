using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    public struct LaneInfo
    {
        public NonGod_Behaviour card;
        public bool active;

        public void Begin(bool active)
        {
            this.active = active;
        }

        public void SetCard(NonGod_Behaviour a)
        {
            card = a;
        }
    }

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
    List<IMonster> enemies;

    [SerializeField]
    Player_Hand _hand;

    short currentLane = 0;

    Card_Behaviour playedCard;

    God_Behaviour currentGod;
    List<LaneInfo> lane;

    public enum State
    {
        StartTurn, PlayerTurn, GodAction, Action, EnemiesTurn, EndTurn
    }

    State currentState = State.StartTurn;

    void Start()
    {
        lane = new List<LaneInfo>(4);

        foreach (LaneInfo a in lane)
        {
            a.Begin(true);
        }

        // deckManager.SetTurnManager(this);

        foreach (IMonster enemy in enemies)
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

                    foreach (IMonster enemy in enemies)
                    {
                        enemy.DecideIntent(enemies, lane, player, currentGod);
                    }
                    
                }
                break;

            case State.PlayerTurn:
                {

                    if (playedCard)
                    {
                        playedCard.gameObject.transform.position = lanes[currentLane].position;
                        playedCard.gameObject.transform.rotation = lanes[currentLane].rotation;

                        if (playedCard as NonGod_Behaviour)
                            lane[currentLane].SetCard(playedCard as NonGod_Behaviour);

                        currentLane++;

                        int i = 0;
                        foreach (Card_Behaviour a in _hand.behaviours)
                        {
                            if (a == playedCard)
                            {
                                break;
                            }
                            i++;
                        }
                        _hand.behaviours.Remove(playedCard);
                        _hand.RemoveCard(i);
                        playedCard = null;
                        cardOnPlay = false;
                        Debug.Log("Select another card");
                    }

                    if (turnEnd || currentLane == lanes.Length) { currentState = State.Action; turnEnd = false; }
                }
                break;

            case State.Action:
                {
                    foreach (LaneInfo laneInfo in lane)
                    {
                        if (laneInfo.active)
                            laneInfo.card.OnAction();
                        deckManager.discardCard(laneInfo.card.GetCardSO());
                        laneInfo.Begin(true);
                    }
                    currentState = State.EnemiesTurn;
                    currentLane = 0;
                }
                break;

            case State.EnemiesTurn:
                {
                    foreach (IMonster enemy in enemies)
                    {
                        enemy.Act();
                    }

                    currentState = State.EndTurn;
                }
                break;

            case State.EndTurn:
                {
                    _hand.RemoveAllCards();
                    deckManager.discardAll();
                    for (int i = lane.Count - 1; i >= 0; i--)
                    {
                        Destroy(lane[i].card.transform.parent.parent.gameObject);
                    }

                    for (int i = _hand.behaviours.Count - 1; i >= 0; i--)
                    {
                        Destroy(_hand.behaviours[i].transform.parent.parent.gameObject);
                    }

                    _hand.behaviours.Clear();
                    lane.Clear();
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
                foreach (IMonster enemy in enemies)
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
        List<NonGod_Behaviour> a = new();
        foreach (LaneInfo info in lane)
        {
            a.Add(info.card);
        }
        return a;
    }
}
