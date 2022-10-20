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
            playedGodCard.transform.position = _GodLane.position;
            Debug.Log("god lane");
            return;
        }

        foreach (var cardLane in _Lane)
        {
            if(cardLane.Equals(targetlane))
            {
                // cardLane = card as God_Behaviour;
                cardLane.transform.position = _GodLane.position;
                return;
            }
        }
    }
}