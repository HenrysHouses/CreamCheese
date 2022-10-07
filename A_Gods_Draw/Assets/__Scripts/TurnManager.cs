using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

public enum EncounterDiffeculty
{
    Easy,
    Medium,
    Hard,
    elites,
    Boss
}

public class TurnManager : MonoBehaviour
{
    public EncounterGenerator EG;
    bool enemiesInitialized;
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
    public  Transform monsterParent;

    short currentLane = 0;

    Card_Behaviour playedCard;
    Card_Behaviour selectedCard;

    [SerializeField]
    EndTurnButton endTurn;
    [SerializeField] SceneTransition sceneTransition;
    public UnityEvent OnSelectedAttackCard;
    public UnityEvent OnDeSelectedAttackCard;

    public bool encounterLoaded;

    bool hasPlayedAGod = false;
    public enum State
    {
        StartTurn, PlayerTurn, GodAction, Action, EnemiesTurn, EndTurn, WaitForAnims
    }

    State currentState = State.StartTurn;

    //----------Temporary
    bool endWait;
    //-------------------

   public Encounter_SO[] LoadNextEncounter()
    {
        return Resources.LoadAll<Encounter_SO>("Encounters/" + GameManager.instance.nextCombatType.ToString());
    }

    void Start()
    {
        board.lane = new List<NonGod_Behaviour>();
        OnSelectedAttackCard = new UnityEvent();
        OnDeSelectedAttackCard = new UnityEvent();
        currentLane = 0;

        // deckManager.SetTurnManager(this);
        StartCoroutine(waitForEncounter());
    }

    IEnumerator waitForEncounter()
    {
        while(!encounterLoaded)
        {
            yield return null;
        }

        foreach (IMonster enemy in board.enemies)
        {
            enemy.Initialize(this, board.player);
        }
        enemiesInitialized = true;
    }

    private void Update()
    {
        
        if(!sceneTransition.isTransitioning)
        {
            if(!encounterLoaded)
            {
                Debug.Log("Monster should be placed");
                Encounter_SO[] foundEncounters = LoadNextEncounter();
                Encounter_SO currEncounter = foundEncounters[UnityEngine.Random.Range(0,foundEncounters.Length)];
                encounterLoaded = true;
                for (int i = 0; i < currEncounter.enemies.Count; i++)
                {
                    GameObject spawn = Instantiate(currEncounter.enemies[i].enemy,currEncounter.enemies[i].enemyPos,Quaternion.identity);
                    spawn.transform.SetParent(monsterParent, false);
                    board.enemies.Add(spawn.GetComponent<IMonster>());
                }

            }
            
        }


        if(!enemiesInitialized)
            return;

        switch (currentState)
        {
            case State.StartTurn:
                {
                    endTurn.GetComponent<BoxCollider>().enabled = false;
                    //SoundManager.Instance.Playsound(SoundDrawCards,gameObject);
                    currentState = State.PlayerTurn;

                    board.deckManager.drawCard(5, this);

                    currentLane = 0;

                    board.player.OnNewTurn();

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
                        int i = 0;
                        foreach (Card_Behaviour a in board._hand.behaviours)
                        {
                            if (a == playedCard)
                            {
                                
                                Animator anim = GetComponentInParent<Animator>();
                                if (anim)
                                {
                                    OnDeSelectedAttackCard?.Invoke();
                                    anim.SetBool("ShowCard", false);
                                    anim.Play("Default");
                                    Destroy(anim);
                                }
                                break;
                            }
                            i++;
                        }

                        NonGod_Behaviour nonGodPlayed = playedCard as NonGod_Behaviour;
                        if (nonGodPlayed)
                        {
                            SoundManager.Instance.Playsound(SoundClickEnemy,gameObject);
                            playedCard.gameObject.transform.position = lanes[currentLane].position;
                            playedCard.gameObject.transform.rotation = lanes[currentLane].rotation;

                            board.lane.Add(nonGodPlayed);

                            currentLane++;

                            if (currentLane >= lanes.Length) endTurn.GetComponentInChildren<Canvas>().enabled = true;

                            foreach(IMonster enemy in board.enemies)
                            {
                                enemy.EnemyHideArrow();
                            }

                            if(board.player)
                            {
                                board.player.PlayerHideArrow();
                            }
                        }
                        else
                        {
                            God_Behaviour godPlayed = playedCard as God_Behaviour;
                            godPlayed.transform.position = godLane.position;
                            godPlayed.transform.rotation = godLane.rotation;

                            godLane.GetComponent<GodPlacement>().SetGod(godPlayed);

                            if (board.currentGod)
                            {
                                godLane.GetComponent<GodPlacement>().GodHideArrow();
                            }

                            hasPlayedAGod = true;
                        }

                        board._hand.behaviours.Remove(playedCard);
                        board._hand.RemoveCard(i);
                        playedCard = null;
                        selectedCard = null;
                        //Debug.Log("Select another card");
                    }

                    if (turnEnd)
                    { 
                        currentState = State.Action; 
                        turnEnd = false;
                        endTurn.GetComponent<BoxCollider>().enabled = false;
                        endTurn.GetComponentInChildren<Canvas>().enabled = false;
                    }
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

                    if (hasPlayedAGod)
                        board.deckManager.discardAll(board.currentGod.GetCardSO(), this);
                    else
                        board.deckManager.discardAll(this);

                    for (int i = board.lane.Count - 1; i >= 0; i--)
                    {
                        Destroy(board.lane[i].transform.parent.parent.gameObject);
                    }

                    for (int i = board._hand.behaviours.Count - 1; i >= 0; i--)
                    {
                        Destroy(board._hand.behaviours[i].transform.parent.parent.gameObject);
                    }

                    hasPlayedAGod = false;
                    board._hand.behaviours.Clear();
                    board.lane.Clear();
                    currentState = State.WaitForAnims;
                }
                break;

            case State.WaitForAnims:
                {
                    if (endWait)
                    {
                        currentState = State.StartTurn;
                        endWait = false;
                    }
                }
                break;
        }
    }

    public void FinishedAnimations()
    {
        endWait = true;
    }

    internal void GodDied()
    {
        board.currentGod.OnRetire(board.lane);

        //board.deckManager.discardCard(board.currentGod.GetCardSO());
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
            Debug.Log("selected start");
        
        if(currentLane == lanes.Length)
        {
            return;
        }

        if (currentState == State.PlayerTurn)
        {
            selectedCard = card;
            Debug.Log("selected attempt");
            God_Behaviour isGod = card as God_Behaviour;
            NonGod_Behaviour isNotGod = card as NonGod_Behaviour;


            if (isGod)
            {

                if (board.currentGod) { GodDied(); }
                board.currentGod = isGod;
                board.deckManager.removeCardFromHand(board.currentGod.GetCardSO());
                foreach (IMonster enemy in board.enemies)
                {
                    enemy.SetGod(board.currentGod);
                }
            }
            else if (isNotGod)
            {
                if(isNotGod is Attack_Behaviour attack_)
                    OnSelectedAttackCard?.Invoke();

                //if (currentLane == board.lane.Count)
                //{
                //    selectedCard = null;
                //    return;
                //}
            }

            card.OnPlay(board);
        }
    }

    public void CancelSelection()
    {
        if (selectedCard != null)
        {
            selectedCard.DeSelected();


            OnDeSelectedAttackCard?.Invoke();
        }
        selectedCard = null;
    }

    public Card_Behaviour CurrentlySelectedCard()
    {
        SoundManager.Instance.Playsound(SoundSelectCard, gameObject);
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

    public List<NonGod_Behaviour> CurrentLane()
    {
        return board.lane;
    }

    public Transform[] GetTransforms()
    {
        return lanes;
    }

    public int GetNextPlace() //when playing buff cards it shows the place to the right of the chosen card
    {
        return currentLane + 1;
    }

    void CheckIfPlayerWon()
    {
        if (board.enemies.Count == 0)
        {
            GoToNextScene();
           // ui.ShowwinningPanel();

            board._hand.RemoveAllCards();

            board.deckManager.discardAll();

            for (int i = board.lane.Count - 1; i >= 0; i--)
            {
                Destroy(board.lane[i].transform.parent.parent.gameObject);
            }

            for (int i = board._hand.behaviours.Count - 1; i >= 0; i--)
            {
                Destroy(board._hand.behaviours[i].transform.parent.parent.gameObject);
            }

            hasPlayedAGod = false;
            board._hand.behaviours.Clear();
            board.lane.Clear();

            if (board.currentGod)
            {
                board.deckManager.addCardToDiscard(board.currentGod.GetCardSO());
            }

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
        board._hand.RemoveAllCards();

        board.deckManager.discardAll();

        for (int i = board.lane.Count - 1; i >= 0; i--)
        {
            Destroy(board.lane[i].transform.parent.parent.gameObject);
        }

        for (int i = board._hand.behaviours.Count - 1; i >= 0; i--)
        {
            Destroy(board._hand.behaviours[i].transform.parent.parent.gameObject);
        }

        hasPlayedAGod = false;
        board._hand.behaviours.Clear();
        board.lane.Clear();

        if (board.currentGod)
        {
            board.deckManager.addCardToDiscard(board.currentGod.GetCardSO());
        }

        this.gameObject.SetActive(false);
    }

    public void GoToNextScene()
    {
       // Debug.Log("next scene");
        if(sceneTransition != null)
            sceneTransition.TransitionScene(false, "Map");
        else
            Debug.LogError("Can not transition to next scene, Missing Scene Transitioner Reference");
    }

    public BoardState GetCurrentBoard()
    {
        return board;
    }

    public void HandFull()
    {
        endTurn.GetComponent<BoxCollider>().enabled = true;
    }
}
