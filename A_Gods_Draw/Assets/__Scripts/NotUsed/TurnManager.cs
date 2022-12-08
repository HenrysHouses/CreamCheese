// Written by Javier

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;
// using UnityEngine.SceneManagement;
// using FMODUnity;


// public class TurnManager : MonoBehaviour
// {
//     bool enemiesInitialized;
//     [SerializeField]
//     private EventReference SoundSelectCard, SoundDrawCards,SoundClickEnemy;

//     public bool turnEnd;

//     // [SerializeField]
//     // UIManager ui;

//     [SerializeField]
//     Transform[] lanes;
//     [SerializeField]
//     Transform godLane;

//     [SerializeField]
//     BoardStateController board;
//     public  Transform monsterParent;

//     short currentLane = 0;

//     Card_Behaviour playedCard;
//     Card_Behaviour selectedCard;

//     [SerializeField]
//     EndTurnButton endTurn;
//     [SerializeField] SceneTransition sceneTransition;
//     [HideInInspector] public UnityEvent OnSelectedAttackCard;
//     [HideInInspector] public UnityEvent OnDeSelectedAttackCard;

//     public bool encounterLoaded;

//     bool hasPlayedAGod = false;
//     public enum State
//     {
//         StartTurn, PlayerTurn, GodAction, Action, EnemiesTurn, EndTurn, WaitForAnims
//     }

//     State currentState = State.StartTurn;

//     //----------Temporary
//     bool endWait;
//     //-------------------

//     public Encounter_SO[] LoadNextEncounter()
//     {
//         Encounter_SO[] loaded = Resources.LoadAll<Encounter_SO>("Encounters/" + GameManager.instance.nextCombatType.ToString());
//         if(loaded == null)
//             throw new UnityException("Was not able to load any encounters in: \"Assets/Resources/Encounters/" + GameManager.instance.nextCombatType + "\"");
//         return loaded;
//     }
//     IEnumerator waitForEncounter()
//     {
//         while(!encounterLoaded)
//         {
//             yield return null;
//         }

//         foreach (IMonster enemy in board.Enemies)
//         {
//             enemy.Initialize(this, board.player);
//         }
//         enemiesInitialized = true;
//     }

//     void Start()
//     {
//         board.playedCards = new List<NonGod_Behaviour>();
//         OnSelectedAttackCard = new UnityEvent();
//         OnDeSelectedAttackCard = new UnityEvent();
//         currentLane = 0;

//         // deckManager.SetTurnManager(this);
//         StartCoroutine(waitForEncounter());
//     }


//     private void Update()
//     {
        
//         if(!sceneTransition.isTransitioning)
//         {
//             if(!encounterLoaded)
//             {
//                 Encounter_SO[] foundEncounters = LoadNextEncounter();
//                 Encounter_SO currEncounter = foundEncounters[UnityEngine.Random.Range(0,foundEncounters.Length-1)];
//                 encounterLoaded = true;
//                 for (int i = 0; i < currEncounter.enemies.Count; i++)
//                 {
//                     GameObject spawn = Instantiate(currEncounter.enemies[i].enemy,currEncounter.enemies[i].enemyPos,Quaternion.identity);
//                     spawn.transform.SetParent(monsterParent, false);
//                     board.Enemies.Add(spawn.GetComponent<IMonster>());
//                 }
//             }
//         }


//         if(!enemiesInitialized)
//             return;

//         switch (currentState)
//         {
//             case State.StartTurn:
//                 {
//                     endTurn.GetComponent<BoxCollider>().enabled = false;
//                     //SoundManager.Instance.Playsound(SoundDrawCards,gameObject);
//                     currentState = State.PlayerTurn;

//                     CardPathAnim[] triggerData = board.deckManager.drawCard(5, 0.25f, this);
//                     foreach (var trigger in triggerData)
//                     {
//                         if(trigger != null)
//                             trigger.OnCardCompletionTrigger.AddListener(board._hand.AddCard);
//                     }
//                     currentLane = 0;

//                     board.player.OnNewTurn();

//                     if (board.playedGodCard) { board.playedGodCard.OnTurnStart(); }

//                     foreach (IMonster enemy in board.Enemies)
//                     {
//                         enemy.DecideIntent(board);
//                     }
                    
//                 }
//                 break;

//             case State.PlayerTurn:
//                 {
//                     if (playedCard)
//                     {  
//                         int i = 0;
//                         foreach (Card_Behaviour a in board._hand.behaviour)
//                         {
//                             if (a == playedCard)
//                             {
                               
//                                 Animator anim = GetComponentInParent<Animator>();
//                                 if (anim)
//                                 {
//                                     OnDeSelectedAttackCard?.Invoke();
//                                     anim.SetBool("ShowCard", false);
//                                     anim.Play("Default");
//                                     Destroy(anim);
//                                     SoundManager.Instance.StopSound(SoundClickEnemy,gameObject);
//                                 }
//                                 break;
//                             }
//                             i++;
//                         }

//                         NonGod_Behaviour nonGodPlayed = playedCard as NonGod_Behaviour;
//                         if (nonGodPlayed)
//                         {
                            
//                             playedCard.gameObject.transform.position = lanes[currentLane].position;
//                             playedCard.gameObject.transform.rotation = lanes[currentLane].rotation;

//                             board.playedCards.Add(nonGodPlayed);

//                             currentLane++;

//                             if (currentLane >= lanes.Length) endTurn.GetComponentInChildren<Canvas>().enabled = true;

//                             foreach(IMonster enemy in board.Enemies)
//                             {
//                                 enemy.EnemyHideArrow();
//                             }

//                             if (board.player)
//                             {
//                                 board.player.PlayerHideArrow();
//                             }
//                         }
//                         else
//                         {
//                             God_Behaviour godPlayed = playedCard as God_Behaviour;
//                             godPlayed.transform.position = godLane.position;
//                             godPlayed.transform.rotation = godLane.rotation;

//                             godLane.GetComponent<GodPlacement>().SetGod(godPlayed);

//                             if (board.playedGodCard)
//                             {
//                                 godLane.GetComponent<GodPlacement>().GodShowArrow();
//                             }

//                             hasPlayedAGod = true;
//                         }

//                         board._hand.behaviour.Remove(playedCard);
//                         board._hand.RemoveCard(i);
//                         playedCard = null;
//                         selectedCard = null;
//                         //Debug.Log("Select another card");
//                     }

//                     if (turnEnd)
//                     { 
//                         currentState = State.Action; 
//                         turnEnd = false;
//                         endTurn.GetComponent<BoxCollider>().enabled = false;
//                        // endTurn.GetComponentInChildren<Canvas>().enabled = false;
//                     }
//                 }
//                 break;

//             case State.Action:
//                 {
//                     foreach (NonGod_Behaviour card in board.playedCards)
//                     {
//                         card.OnAction();
//                         board.deckManager.discardCard(card.GetCardSO());
//                     }
//                     CheckIfPlayerWon();
//                     currentState = State.EnemiesTurn;
//                 }
//                 break;

//             case State.EnemiesTurn:
//                 {
//                     foreach (IMonster enemy in board.Enemies)
//                     {
//                         enemy.Act();
//                         if (board.player.GetHealth() <= 0)
//                         {
//                             PlayerLost();
//                             break;
//                         }
//                     }

//                     currentState = State.EndTurn;
//                 }
//                 break;

//             case State.EndTurn:
//                 {
                    
//                     board._hand.RemoveAllCards();

//                     if (hasPlayedAGod)
//                         board.deckManager.discardAll(0.25f, board.playedGodCard.GetCardSO(), this);
//                     else
//                         board.deckManager.discardAll(0.25f, this);

//                     for (int i = board.playedCards.Count - 1; i >= 0; i--)
//                     {
//                         Destroy(board.playedCards[i].transform.parent.parent.gameObject);
//                     }

//                     for (int i = board._hand.behaviour.Count - 1; i >= 0; i--)
//                     {
//                         Destroy(board._hand.behaviour[i].transform.parent.parent.gameObject);
//                     }

//                     hasPlayedAGod = false;
//                     board._hand.behaviour.Clear();
//                     board.playedCards.Clear();
//                     currentState = State.WaitForAnims;
//                 }
//                 break;

//             case State.WaitForAnims:
//                 {
//                     if (endWait)
//                     {
//                         currentState = State.StartTurn;
//                         endWait = false;
//                     }
//                 }
//                 break;
//         }
//     }

//     public void FinishedAnimations()
//     {
//         endWait = true;
//     }

//     internal void GodDied()
//     {
//         board.playedGodCard.OnRetire(board.playedCards);

//         //board.deckManager.discardCard(board.playedGodCard.GetCardSO());
//         // board.deckManager.addCardToDiscard(board.playedGodCard.GetCardSO());

//         Destroy(board.playedGodCard.transform.parent.parent.gameObject);
//         board.playedGodCard = null;
//     }

//     public void EndTurn()
//     {
//         turnEnd = true;
//     }

//     public void SelectCard(Card_Behaviour card)
//     {
//         if(currentLane == lanes.Length)
//         {
//             return;
//         }

//         if (currentState == State.PlayerTurn)
//         {
//             selectedCard = card;

//             if (selectedCard is God_Behaviour God_)
//             {

//                 if (board.playedGodCard) { GodDied(); }
//                 board.playedGodCard = selectedCard as God_Behaviour;
//                 board.deckManager.removeCardFromHand(board.playedGodCard.GetCardSO());
//                 foreach (IMonster enemy in board.Enemies)
//                 {
//                     enemy.SetGod(board.playedGodCard);
//                 }
//             }
//             else if (selectedCard is NonGod_Behaviour notGod_)
//             {
//                 if(selectedCard is Attack_Behaviour attack_)
//                     OnSelectedAttackCard?.Invoke();

//                 //if (currentLane == board.playedCards.Count)
//                 //{
//                 //    selectedCard = null;
//                 //    return;
//                 //}
//             }

//             card.OnPlay(board);
//             board._hand.UpdateCards();
            
            
//         }
//     }

//     public void CancelSelection()
//     {
//         if (selectedCard != null)
//         {
//             selectedCard.DeSelected();


//             OnDeSelectedAttackCard?.Invoke();
//         }
//         selectedCard = null;
//     }

//     public Card_Behaviour CurrentlySelectedCard()
//     {
        
//         return selectedCard;
//     }

//     public void FinishedPlay(Card_Behaviour card)
//     {
//         playedCard = card;
//     }

//     public State GetState()
//     {
//         return currentState;
//     }

//     public List<NonGod_Behaviour> CurrentLane()
//     {
//         return board.playedCards;
//     }

//     public Transform[] GetTransforms()
//     {
//         return lanes;
//     }

//     public int GetNextPlace() //when playing buff cards it shows the place to the right of the chosen card
//     {
//         return currentLane + 1;
//     }

//     void CheckIfPlayerWon()
//     {
//         if (board.Enemies.Count == 0)
//         {
//             GoToNextScene();
//            // ui.ShowwinningPanel();

//             board._hand.RemoveAllCards();

//             board.deckManager.discardAll(0.25f);

//             for (int i = board.playedCards.Count - 1; i >= 0; i--)
//             {
//                 Destroy(board.playedCards[i].transform.parent.parent.gameObject);
//             }

//             for (int i = board._hand.behaviour.Count - 1; i >= 0; i--)
//             {
//                 Destroy(board._hand.behaviour[i].transform.parent.parent.gameObject);
//             }

//             hasPlayedAGod = false;
//             board._hand.behaviour.Clear();
//             board.playedCards.Clear();

//             if (board.playedGodCard)
//             {
//                 board.deckManager.addCardToDiscard(board.playedGodCard.GetCardSO());
//             }

//             this.gameObject.SetActive(false);
//         }
//     }

//     public void EnemyDied(IMonster enemy)
//     {
//         board.Enemies.Remove(enemy);
//     }

//     public void PlayerLost()
//     {
//         //Play audio
//         GameManager.instance.newGame();
//         // ui.ShowLoosingPanel();
//         board._hand.RemoveAllCards();

//         board.deckManager.discardAll(0.25f);

//         for (int i = board.playedCards.Count - 1; i >= 0; i--)
//         {
//             Destroy(board.playedCards[i].transform.parent.parent.gameObject);
//         }

//         for (int i = board._hand.behaviour.Count - 1; i >= 0; i--)
//         {
//             Destroy(board._hand.behaviour[i].transform.parent.parent.gameObject);
//         }

//         hasPlayedAGod = false;
//         board._hand.behaviour.Clear();
//         board.playedCards.Clear();

//         if (board.playedGodCard)
//         {
//             board.deckManager.addCardToDiscard(board.playedGodCard.GetCardSO());
//         }

//         this.gameObject.SetActive(false);
//     }

//     public void GoToNextScene()
//     {
//        // Debug.Log("next scene");
//         if(sceneTransition != null)
//             sceneTransition.TransitionScene(false, "Map");
//         else
//             Debug.LogError("Can not transition to next scene, Missing Scene Transitioner Reference");
//     }

//     public BoardStateController GetCurrentBoard()
//     {
//         return board;
//     }

//     public void HandFull()
//     {
//         endTurn.GetComponent<BoxCollider>().enabled = true;
//     }
// }
