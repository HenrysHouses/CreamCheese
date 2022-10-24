/* 
 * Refactoring by 
 * Henrik
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardStateController : MonoBehaviour
{
    // * Public
    public bool isEncounterInstantiated = false;
    
    // Getters
    public Encounter_SO Encounter => _Encounter;
    public IMonster[] Enemies => _Enemies;
    public Transform getLane(int i) => _Lane[i];
    public Transform getGodLane() => _GodLane;
    [HideInInspector] public List<NonGod_Behaviour> playedCards;
    [HideInInspector] public God_Behaviour playedGodCard;
    public NonGod_Behaviour getCardInLane(int i) => playedCards[i];

    public bool isEnemyDefeated 
    {
        get 
        {
            if(getLivingEnemies().Length == 0)
                return true;
            else
                return false;
        }
    }

    // * Private
    [SerializeField] Transform EnemyParent;
    Encounter_SO _Encounter;
    IMonster[] _Enemies;
    [SerializeField] Transform[] _Lane;
    [SerializeField] Transform _GodLane;

    #region ClickedStuff

    IMonster clickedMonster;
    public void SetClickedMonster(IMonster clicked = null)
    {
        clickedMonster = clicked;
    }
    public IMonster GetClickedMonster()
    {
        return clickedMonster;
    }

    NonGod_Behaviour clickedCard;
    public void SetClickedCard(NonGod_Behaviour clicked = null)
    {
        clickedCard = clicked;
    }
    public NonGod_Behaviour GetClickedCard()
    {
        return clickedCard;
    }

    God_Behaviour clickedGod;
    public void SetClickedGod(God_Behaviour clicked = null)
    {
        clickedGod = clicked;
    }
    public God_Behaviour GetClickedGod()
    {
        return clickedGod;
    }

    PlayerController playerClicked;
    public void SetClickedPlayer(PlayerController clicked = null)
    {
        playerClicked = clicked;
    }
    public PlayerController GetClickedPlayer()
    {
        return playerClicked;
    }

    #endregion

    void spawnEncounter()
    {
        Encounter_SO[] possibleEncounters = EncounterLoader.LoadAllEncountersOf(GameManager.instance.nextCombatType);
        EncounterLoader.InstantiateRandomEncounter(possibleEncounters, EnemyParent, out _Encounter);
        isEncounterInstantiated = true;
    }

    public IMonster[] getLivingEnemies()
    {
        List<IMonster> livingEnemies = new List<IMonster>();
        for (int i = 0; i < _Enemies.Length; i++)
        {
            if(_Enemies[i].GetHealth() > 0)
                livingEnemies.Add(_Enemies[i]);
        }
        return livingEnemies.ToArray();
    }

    public EnemyIntent[] getAllIntents()
    {
        List<EnemyIntent> intents = new List<EnemyIntent>();
        foreach (var _i in _Enemies)
        {
            intents.Add(_i.GetIntent().GetID());
        }
        return intents.ToArray();
    }

    public EnemyIntent getEnemyIntent(int index)
    {
        return _Enemies[index].GetIntent().GetID();
    }

    public void placeCardOnLane(Transform targetlane, Card_Behaviour card)
    {
        if(_GodLane.Equals(targetlane))
        {
            // on god lane
            playedGodCard = card as God_Behaviour;
            playedGodCard.transform.parent.SetParent(_GodLane);
            playedGodCard.transform.parent.position = new Vector3();
            Debug.Log("god lane");
            return;
        }

        for (int i = 0; i < _Lane.Length; i++)
        {
            if(_Lane[i].Equals(targetlane))
            {
                Debug.Log(i);
                playedCards.Add(card as NonGod_Behaviour);
                
                Transform cardTransform = playedCards[i].transform.parent.parent; 
                
                
                cardTransform.SetParent(_Lane[i]);
                cardTransform.localPosition = new Vector3();
                return;
            }
        }
    }
}