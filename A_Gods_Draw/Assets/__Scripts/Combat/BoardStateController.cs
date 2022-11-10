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

    [SerializeField] GameObject TargetingMesh;

    // Getters
    public PlayerController Player => _Player;
    public Encounter_SO Encounter => _Encounter;
    public IMonster[] Enemies => _Enemies;
    public Transform getLane(int i) => _Lane[i];
    public Transform getGodLane() => _GodLane;
    [HideInInspector] public List<NonGod_Behaviour> playedCards;
    [HideInInspector] public God_Behaviour playedGodCard;
    [HideInInspector] public List<BoardElement> thingsInLane;
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

    //particle effect
    [SerializeField] SlashParticles slashParticles;

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
        Debug.Log(GameManager.instance.nextCombatType);
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

    public void placeThingOnLane(BoardElement thing)
    {
        if (thingsInLane.Count >= _Lane.Length)
        {
            return;
        }

        Transform targetlane = _Lane[thingsInLane.Count];

        thing.transform.position = targetlane.position;
        thing.transform.parent = targetlane;

        thingsInLane.Add(thing);
    }

    public void placeCardOnLane(Card_Behaviour card)
    {
        

        Transform targetlane = null;

        if (card is God_Behaviour)
        {
            targetlane = _GodLane;

        }
        else
        {
            targetlane = _Lane[thingsInLane.Count];
            SoundPlayer.Playsound(placeCard_SFX, gameObject);

        }

        if (_GodLane.Equals(targetlane))
        {

            Debug.Log("god lane");
            playedGodCard = card as God_Behaviour;
            SoundPlayer.Playsound(playedGodCard.CardSO.enterBattlefield_SFX, gameObject);


            Transform cardTransform = playedGodCard.transform;

            cardTransform.parent.parent.SetParent(_GodLane);
            cardTransform.parent.parent.localPosition = new Vector3();
            cardTransform.parent.localPosition = new Vector3();
            cardTransform.localPosition = new Vector3();
            cardTransform.parent.parent.localRotation = new Quaternion();
            cardTransform.parent.rotation = new Quaternion();
            cardTransform.parent.GetComponent<Card_Selector>().enabled = false;
            cardTransform.parent.parent.localScale = new Vector3(1.5f, 1.5f, 1.5f); // !!REMOVE THIS AFTER FINDING A PREFERABLE SIZE FOR THE CARDS

            GameObject spawn = Instantiate(playedGodCard.CardSO.God_Model, targetlane.position, transform.localRotation = new Quaternion(0, -0.577358961f, 0, 0.816490531f));
            spawn.transform.SetParent(cardTransform, true);

            Debug.LogWarning("REMOVE SIZE HERE");
            return;
        }

        for (int i = 0; i < _Lane.Length; i++)
        {
            if (_Lane[i].Equals(targetlane))
            {
                Debug.Log(i);

                NonGod_Behaviour behaviour = card as NonGod_Behaviour;

                playedCards.Add(behaviour);
                thingsInLane.Add(behaviour);

                Transform cardTransform = thingsInLane[i].transform;

                cardTransform.parent.parent.SetParent(_Lane[i]);
                cardTransform.parent.parent.localPosition = new Vector3();
                cardTransform.parent.localPosition = new Vector3();
                cardTransform.localPosition = new Vector3();
                cardTransform.parent.parent.localRotation = new Quaternion();
                cardTransform.parent.rotation = new Quaternion();
                cardTransform.parent.GetComponent<Card_Selector>().enabled = false;
                cardTransform.parent.parent.localScale = new Vector3(1.5f, 1.5f, 1.5f);  // !!REMOVE THIS AFTER FINDING A PREFERABLE SIZE FOR THE CARDS


                for (int j = 0; j < behaviour.TargetedActions; j++) // should run this for each targetable action
                {
                    IMonster target = behaviour.getActionTarget(j);
                    if (target == null)
                        continue;

                    GameObject spawn = Instantiate(TargetingMesh, Vector3.zero, Quaternion.identity);
                    spawn.transform.SetParent(cardTransform, true);
                    ProceduralPathMesh Mesh = spawn.GetComponent<ProceduralPathMesh>();
                    Mesh.startPoint.position = cardTransform.position;

                    Debug.Log(target);
                    Mesh.endPoint.position = target.transform.position;
                }
                return;
            }
        }
    }
}