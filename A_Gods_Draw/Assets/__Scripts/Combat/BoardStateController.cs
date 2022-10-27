/* 
 * Refactoring by 
 * Henrik
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BoardStateController : MonoBehaviour
{
    // * Public
    public bool isEncounterInstantiated = false;

    // Getters
    public PlayerController Player => _Player;
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
            if (getLivingEnemies().Length == 0)
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
    [SerializeField] PlayerController _Player; 

    // Sounds
    [SerializeField] EventReference placeCard_SFX;

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
                foreach (NonGod_Behaviour card in playedCards)
                {
                    card.clickable = clickable;
                }
                break;
            case 1:
                if (playedGodCard)
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
    }

    public IMonster[] getLivingEnemies()
    {
        List<IMonster> livingEnemies = new List<IMonster>();
        for (int i = 0; i < _Enemies.Length; i++)
        {
            if (_Enemies[i].GetHealth() > 0)
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

    public void placeCardOnLane(Card_Behaviour card, Transform targetlane = null)
    {
        SoundPlayer.Playsound(placeCard_SFX,gameObject);

        if (!targetlane)
        {
            

            if (card is God_Behaviour)
            {
                targetlane = _GodLane;
            }
            else
            {
                targetlane = _Lane[playedCards.Count];

            }


        }

        if (_GodLane.Equals(targetlane))
        {

            Debug.Log("god lane");
            playedGodCard = card as God_Behaviour;

            Transform cardTransform = playedGodCard.transform;

            cardTransform.parent.parent.SetParent(_GodLane);
            cardTransform.parent.parent.localPosition = new Vector3();
            cardTransform.parent.localPosition = new Vector3();
            cardTransform.localPosition = new Vector3();
            cardTransform.parent.parent.localRotation = new Quaternion();
            cardTransform.parent.rotation = new Quaternion();
            cardTransform.parent.GetComponent<Card_Selector>().enabled = false;
            cardTransform.parent.parent.localScale = new Vector3(1.5f,1.5f,1.5f); // !!REMOVE THIS AFTER FINDING A PREFERABLE SIZE FOR THE CARDS
            Debug.LogWarning("REMOVE SIZE HERE");
            return;
        }

        for (int i = 0; i < _Lane.Length; i++)
        {
            if (_Lane[i].Equals(targetlane))
            {
                Debug.Log(i);

                NonGod_Behaviour behaviour = card as NonGod_Behaviour;

                if (behaviour.CardSO.type == CardType.Buff)
                {
                    for (int j = 0; j < behaviour.GetStrengh(); j++)
                    {
                        //Instantiate coins
                    }
                    Destroy(behaviour.transform.parent.parent.gameObject);
                    return;
                }

                playedCards.Add(behaviour);

                Transform cardTransform = playedCards[i].transform;

                cardTransform.parent.parent.SetParent(_Lane[i]);
                cardTransform.parent.parent.localPosition = new Vector3();
                cardTransform.parent.localPosition = new Vector3();
                cardTransform.localPosition = new Vector3();
                cardTransform.parent.parent.localRotation = new Quaternion();
                cardTransform.parent.rotation = new Quaternion();
                cardTransform.parent.GetComponent<Card_Selector>().enabled = false;
                cardTransform.parent.parent.localScale = new Vector3(1.5f,1.5f,1.5f);  // !!REMOVE THIS AFTER FINDING A PREFERABLE SIZE FOR THE CARDS
                return;
            }
        }


    }
}