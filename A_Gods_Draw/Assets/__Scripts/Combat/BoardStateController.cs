/* 
 * Refactoring by 
 * Henrik
*/

using System;
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

    /// <param name="whatToSet">
    /// 0: cards in lane
    /// 1: godCard
    /// 2: player
    /// 3: enemies
    /// </param>
    public void SetClickable(int whatToSet, bool clickable = true)
    {
        switch (whatToSet)
        {
            case 0:
                foreach(NonGod_Behaviour card in playedCards)
                {
                    card.clickable = clickable;
                }
                break;
            case 1:
                playedGodCard.clickable = clickable;
                break;
            case 2:
                
                break;
            case 3:
                foreach (IMonster ene in Enemies)
                {
                    ene.clickable = clickable;
                }
                break;
            default:
                break;
        }
    }

    public void spawnEncounter()
    {
        Encounter_SO[] possibleEncounters = EncounterLoader.LoadAllEncountersOf(GameManager.instance.nextCombatType);
        _Enemies = EncounterLoader.InstantiateRandomEncounter(possibleEncounters, EnemyParent, out _Encounter);
        isEncounterInstantiated = true;
        Debug.Log("spawned?");
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
    internal void placeCardOnLane(NonGod_Behaviour card)
    {
        playedCards.Add(card as NonGod_Behaviour);

        Transform cardTransform = playedCards[^1].transform.parent.parent;

        cardTransform.SetParent(_Lane[^1]);
        cardTransform.localPosition = new Vector3();
        return;
    }
}