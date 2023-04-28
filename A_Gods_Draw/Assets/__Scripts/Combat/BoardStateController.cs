/* 
 * Refactoring by 
 * Henrik
 * 
 * Modified by Javier
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using DitzeGames.Effects;
using EnemyAIEnums;

public class BoardStateController : MonoBehaviour
{
    // * Serialized / Public
    public bool isEncounterInstantiated = false;
    [SerializeField] GameObject TargetingMesh;
    [SerializeField, ColorUsage(true, true)] Color AttackColor, AttackSecondaryColor;
    [SerializeField, ColorUsage(true, true)] Color DefendColor, DefendSecondaryColor;
    [SerializeField, ColorUsage(true, true)] Color UtilityColor, UtilitySecondaryColor;

    public PlayerController Player => _Player;
    [SerializeField] PlayerController _Player;
    public Encounter_SO Encounter => _Encounter;
    public Monster[] Enemies => _Enemies;
    public List<BoardTarget> ActiveExtraEnemyTargets;
    public List<BoardTarget> AllExtraEnemyTargets;
    public Transform getLane(int i) => _Lane[i];
    public Transform getGodLane() => _GodLane;
    [HideInInspector] public List<ActionCard_Behaviour> placedCards;
    [HideInInspector] public List<ActionCard_Behaviour> allPlayedCards;
    [HideInInspector] public List<CardPlayData> ExhaustedCards;
    public bool isGodPlayed => playedGodCard;
    [HideInInspector] public GodCard_Behaviour playedGodCard;
    [HideInInspector] public List<BoardElement> thingsInLane;
    public ActionCard_Behaviour getCardInLane(int i) => placedCards[i];

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
    [SerializeField] Battlefield[] Battlefields;
    public BattlefieldID ActiveBattleFieldType;
    Monster[] _Enemies;
    [SerializeField] Transform[] _Lane;
    [SerializeField] Transform _GodLane;

    // * Sounds
    [SerializeField] EventReference placeCard_SFX;

    // * particle effect
    [SerializeField] SlashParticles slashParticles;

    /// <summary>Spawns an encounter of difficulty set by the GameManager</summary>
    public void spawnEncounter()
    {
        Encounter_SO[] possibleEncounters = EncounterLoader.LoadAllEncountersOf(GameManager.instance.nextCombatType);
        Debug.Log(GameManager.instance.nextCombatType);
        _Enemies = EncounterLoader.InstantiateRandomEncounter(possibleEncounters, EnemyParent, out _Encounter);

        for(int i = 0; i < _Enemies.Length; i++)
        {

            _Enemies[i].Board = this;

        }
        
        foreach (var field in Battlefields)
        {
            if(field.type.Equals(_Encounter.battlefieldID))
                field.Mesh.SetActive(true);
        }
        ActiveBattleFieldType = _Encounter.battlefieldID;

        isEncounterInstantiated = true;
    }

    public void AddBoardTarget(BoardTarget _target)
    {
        ActiveExtraEnemyTargets.Add(_target);
        AllExtraEnemyTargets.Add(_target);
    }

    public void removeNullEnemies()
    {
        _Enemies = getLivingEnemies();
    }

    public Monster[] getLivingEnemies()
    {
        List<Monster> livingEnemies = new List<Monster>();
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

    public void PlayCard(ActionCard_Behaviour card)
    {
        allPlayedCards.Add(card);
    }

    public void placeCardOnLane(Card_Behaviour card)
    {
        Transform targetlane = null;

        if (card is GodCard_Behaviour)
        {
            targetlane = _GodLane;
        }
        else
        {
            if (thingsInLane.Count >= 4)
                return;
            targetlane = _Lane[thingsInLane.Count];
            SoundPlayer.PlaySound(placeCard_SFX, gameObject);
        }

        if (_GodLane.Equals(targetlane)) // Move card to the godlane
        {
            if(playedGodCard != null)
            {
                playedGodCard.DealDamage(1000000, gameObject);
            }

            AnimationCurve curve = new AnimationCurve( new Keyframe (0,0), new Keyframe (1,1));
            playedGodCard = card as GodCard_Behaviour;
            SoundPlayer.PlaySound(playedGodCard.CardSO.enterBattlefield_SFX, gameObject);
            SoundPlayer.PlaySound(playedGodCard.CardSO.otherSFX, gameObject);
            CameraEffects.ShakeOnce(0.5f,5);


            Transform cardTransform = playedGodCard.transform;

            cardTransform.parent.SetParent(_GodLane);
            cardTransform.GetComponent<Card_Selector>().enabled = false;
            
            cardTransform.parent.localPosition = new Vector3();
            cardTransform.localPosition = new Vector3();
            cardTransform.localPosition = new Vector3();
            cardTransform.parent.localRotation = new Quaternion();
            cardTransform.rotation = new Quaternion();
            cardTransform.parent.localScale = new Vector3(1.09843659f,1.09843659f,1.09843659f); // !!REMOVE THIS AFTER FINDING A PREFERABLE SIZE FOR THE CARDS

            GameObject spawn = Instantiate(playedGodCard.CardSO.God_Model, targetlane.position, transform.localRotation = new Quaternion(0, -0.577358961f, 0, 0.816490531f));
            playedGodCard.animator = spawn.GetComponentInChildren<Animator>();
            playedGodCard.CardSO.OnDialogue.AddListener(playedGodCard.animator.SetTrigger);
            spawn.transform.SetParent(cardTransform, true);
            return;
        }

        for (int i = 0; i < _Lane.Length; i++)
        {
            if (_Lane[i].Equals(targetlane))
            {
                // Debug.Log(i);

                ActionCard_Behaviour behaviour = card as ActionCard_Behaviour;

                placedCards.Add(behaviour);
                thingsInLane.Add(behaviour);

                Transform cardTransform = thingsInLane[i].transform;

                cardTransform.parent.SetParent(_Lane[i]);
                cardTransform.GetComponent<Card_Selector>().enabled = false;

                if(behaviour.CardSO.type != CardType.Buff)
                {
                    cardTransform.parent.localPosition = new Vector3();
                    cardTransform.localPosition = new Vector3();
                    cardTransform.localPosition = new Vector3();
                    cardTransform.parent.localRotation = new Quaternion();
                    cardTransform.rotation = new Quaternion();
                    cardTransform.parent.localScale = new Vector3(1.09843659f,1.09843659f,1.09843659f);  // !!REMOVE THIS AFTER FINDING A PREFERABLE SIZE FOR THE CARDS
                }

                for (int j = 0; j < behaviour.stats.numberOfTargets; j++) // should run this for each targetable action
                {
                    for (int k = 0; k < behaviour.AllTargets.Length; k++)
                    {   
                        if (behaviour.AllTargets[k] == null)
                            continue;

                        // Targeting instantiation
                        GameObject spawn = Instantiate(TargetingMesh, Vector3.zero, Quaternion.identity);
                        spawn.transform.SetParent(cardTransform, true);

                        // Targeting color
                        MeshRenderer renderer = spawn.GetComponent<MeshRenderer>();
                        // Debug.Log("target mesh color");
                        if(behaviour.GetCardType == CardType.Attack)
                        {
                            renderer.material.SetColor("_MainColor", AttackColor);
                            renderer.material.SetColor("_SecondColor", AttackSecondaryColor);

                        }
                        else if(behaviour.GetCardType == CardType.Defence)
                        {
                            renderer.material.SetColor("_MainColor", DefendColor);
                            renderer.material.SetColor("_SecondColor", DefendSecondaryColor);
                        }
                        else
                        {
                            renderer.material.SetColor("_MainColor", UtilityColor);
                            renderer.material.SetColor("_SecondColor", UtilitySecondaryColor);
                        }

                        // Targeting positions
                        ProceduralPathMesh Mesh = spawn.GetComponent<ProceduralPathMesh>();
                        Mesh.startPoint.position = behaviour.AllTargets[k].transform.position;
                        Mesh.endPoint.position = cardTransform.position;   
                        Mesh.GenerateMesh();
                        Mesh.enabled = false;
                    }
                }
                return;
            }
        }
    }

    internal void RemoveFromLane(BoardElement current)
    {
        thingsInLane.Remove(current);

        ActionCard_Behaviour currentCard = current as ActionCard_Behaviour;
        if (currentCard)
        {
            placedCards.Remove(currentCard);
        }
    }
}

/// <summary>Enemy board battlefield data container</summary>
[Serializable]
public class Battlefield
{
    /// <summary>The GameObject with the board mesh renderer</summary>
    public GameObject Mesh;
    public BattlefieldID type;
}

/// <summary>Possible enemy boards</summary>
public enum BattlefieldID
{
    Dragr,
    DraugrV2,
   // DraugrV3,
    TrollCave,
   // TrollTrees,
    Fenrir,
    GrassBoard,
    SandBoard,
    TrollV2
}