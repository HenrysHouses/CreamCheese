using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

[Serializable]
public struct BoardState
{
    public PlayerController player;
    public DeckManager_SO deckManager;
    public List<IMonster> enemies;
    public Player_Hand _hand;
    public God_Behaviour currentGod;
    public List<NonGod_Behaviour> lane;
}

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private EventReference SoundSelectCard, SoundDrawCards,SoundClickEnemy;

    bool turnEnd;

    [SerializeField]
    UIManager ui;

    [SerializeField]
    Transform[] lanes;
    [SerializeField]
    Transform godLane;

    [SerializeField]
    BoardState board;

    short currentLane = 0;

    Card_Behaviour playedCard;
    Card_Behaviour selectedCard;


    public enum State
    {
        StartTurn, PlayerTurn, GodAction, Action, EnemiesTurn, EndTurn
    }

    State currentState = State.StartTurn;

    void Start()
    {
        board.lane = new List<NonGod_Behaviour>();

        // deckManager.SetTurnManager(this);

        foreach (IMonster enemy in board.enemies)
        {
            enemy.Initialize(this, board.player);
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.StartTurn:
                {
                    SoundManager.Instance.Playsound(SoundDrawCards,gameObject);
                    currentState = State.PlayerTurn;

                    board.deckManager.drawCard(5);

                    if (board.currentGod) { board.currentGod.OnTurnStart(); }

                    foreach (IMonster enemy in board.enemies)
                    {
                        enemy.DecideIntent(board);
                    }
                    
                }
                break;

            case State.PlayerTurn:
                {

                    if (playedCard)
                    {
                        NonGod_Behaviour nonGodPlayed = playedCard as NonGod_Behaviour;
                        if (nonGodPlayed)
                        {
                            SoundManager.Instance.Playsound(SoundClickEnemy,gameObject);
                            playedCard.gameObject.transform.position = lanes[currentLane].position;
                            playedCard.gameObject.transform.rotation = lanes[currentLane].rotation;


                            board.lane.Add(nonGodPlayed);

                            if (board.currentGod)
                                nonGodPlayed.CheckForGod(board.currentGod);

                            currentLane++;
                        }
                        else
                        {
                            God_Behaviour godPlayed = playedCard as God_Behaviour;
                            godPlayed.transform.position = godLane.position;
                            godPlayed.transform.rotation = godLane.rotation;
                            godPlayed.SearchToBuff(board.lane);
                        }

                        int i = 0;
                        foreach (Card_Behaviour a in board._hand.behaviours)
                        {
                            if (a == playedCard)
                            {
                                Animator anim  = GetComponentInParent<Animator>();
                                a.GetComponentInParent<BoxCollider>().enabled = false;
                                if(anim)
                                {
                                    anim.SetBool("ShowCard",false);
                                    anim.Play("Default");
                                    Destroy(anim);

                                }
                                break;
                            }
                            i++;
                        }
                        board._hand.behaviours.Remove(playedCard);
                        board._hand.RemoveCard(i);
                        playedCard = null;
                        selectedCard = null;
                        Debug.Log("Select another card");
                    }

                    if (turnEnd) { currentState = State.Action; turnEnd = false; }
                }
                break;

            case State.Action:
                {
                    foreach (NonGod_Behaviour card in board.lane)
                    {
                        card.OnAction();
                        board.deckManager.discardCard(card.GetCardSO());
                    }
                    CheckIfPlayerWon();
                    currentState = State.EnemiesTurn;
                    currentLane = 0;
                }
                break;

            case State.EnemiesTurn:
                {
                    foreach (IMonster enemy in board.enemies)
                    {
                        enemy.Act();
                        if (board.player.GetHealth() <= 0)
                        {
                            PlayerLost();
                            break;
                        }
                    }

                    currentState = State.EndTurn;
                }
                break;

            case State.EndTurn:
                {
                    board._hand.RemoveAllCards();

                    if (board.currentGod != null)
                        board.deckManager.discardAll(board.currentGod.GetCardSO());

                    board.deckManager.discardAll();
                    for (int i = board.lane.Count - 1; i >= 0; i--)
                    {
                        Destroy(board.lane[i].transform.parent.parent.gameObject);
                    }

                    for (int i = board._hand.behaviours.Count - 1; i >= 0; i--)
                    {
                        Destroy(board._hand.behaviours[i].transform.parent.parent.gameObject);
                    }

                    board._hand.behaviours.Clear();
                    board.lane.Clear();
                    currentState = State.StartTurn;
                }
                break;
        }
    }

    internal void GodDied()
    {
        board.currentGod.OnRetire(board.lane);

        board.deckManager.addCardToDiscard(board.currentGod.GetCardSO());

        Destroy(board.currentGod.transform.parent.parent.gameObject);
        board.currentGod = null;
    }

    public void EndTurn()
    {
        turnEnd = true;
    }

    public void SelectCard(Card_Behaviour card)
    {
        if (currentState == State.PlayerTurn)
        {
            selectedCard = card;

            God_Behaviour isGod = card as God_Behaviour;
            NonGod_Behaviour isNotGod = card as NonGod_Behaviour;


            if (isGod)
            {
                if (board.currentGod) { board.currentGod.OnRetire(board.lane); }
                board.currentGod = isGod;
                foreach (IMonster enemy in board.enemies)
                {
                    enemy.SetGod(board.currentGod);
                }
            }
            else if (isNotGod)
            {
                if (currentLane == board.lane.Count)
                {
                    selectedCard = null;
                    return;
                }
            }
            else
            {
                Debug.Log("WTF have you done");
            }

            StartCoroutine(card.OnPlay(board));
        }
    }

    public void CancelSelection()
    {
        StopCoroutine(selectedCard.OnPlay(board));

        selectedCard = null;
    }

    public Card_Behaviour CurrentlySelectedCard()
    {
        return selectedCard;
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
        SoundManager.Instance.Playsound(SoundSelectCard,gameObject);
        return cardOnPlay;
    }

    public List<NonGod_Behaviour> CurrentLane()
    {
        return board.lane;
    }

    void CheckIfPlayerWon()
    {
        if (board.enemies.Count == 0)
        {
            ui.ShowwinningPanel();
            this.gameObject.SetActive(false);
        }
    }

    public void EnemyDied(IMonster enemy)
    {
        board.enemies.Remove(enemy);
    }

    public void PlayerLost()
    {
        //Play audio
        ui.ShowLoosingPanel();
        this.gameObject.SetActive(false);
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public BoardState GetCurrentBoard()
    {
        return board;
    }
}
