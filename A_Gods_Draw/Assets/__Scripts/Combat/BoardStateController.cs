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

public class BoardStateController : MonoBehaviour
{
    // * Serialized / Public
    public bool isEncounterInstantiated = false;
    [SerializeField] GameObject TargetingMesh;
    [SerializeField] Color AttackColor;
    [SerializeField] Color DefendColor;
    [SerializeField] Color UtilityColor;

    public PlayerController Player => _Player;
    public Encounter_SO Encounter => _Encounter;
    public Monster[] Enemies => _Enemies;
    public Transform getLane(int i) => _Lane[i];
    public Transform getGodLane() => _GodLane;
    [HideInInspector] public List<ActionCard_Behaviour> placedCards;
    [HideInInspector] public List<ActionCard_Behaviour> allPlayedCards;
    [HideInInspector] public List<Card_SO> ExhaustedCards;
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
    Monster[] _Enemies;
    [SerializeField] Transform[] _Lane;
    [SerializeField] Transform _GodLane;
    [SerializeField] PlayerController _Player;

    // * Sounds
    [SerializeField] EventReference placeCard_SFX;

    // * particle effect
    [SerializeField] SlashParticles slashParticles;

    /// <param name="whatToSet">
    /// 0: cards in lane
    /// 1: godCard
    /// 2: player
    /// 3: enemies
    /// </param>
    // public void SetClickable(int whatToSet, bool clickable = true)
    // {
    //     switch (whatToSet)
    //     {
    //         case 0:
    //             foreach (NonGod_Behaviour card in placedCards)
    //             {
    //                 card.clickable = clickable;
    //             }
    //             break;
    //         case 1:
    //             if (playedGodCard)
    //                 playedGodCard.clickable = clickable;
    //             break;
    //         case 2:

    //             break;
    //         case 3:
    //             foreach (IMonster ene in getLivingEnemies())
    //             {
    //                 ene.clickable = clickable;
    //             }
    //             break;
    //         default:
    //             break;
    //     }
    // }

    /// <summary>Spawns an encounter of difficulty set by the GameManager</summary>
    public void spawnEncounter()
    {
        Encounter_SO[] possibleEncounters = EncounterLoader.LoadAllEncountersOf(GameManager.instance.nextCombatType);
        Debug.Log(GameManager.instance.nextCombatType);
        _Enemies = EncounterLoader.InstantiateRandomEncounter(possibleEncounters, EnemyParent, out _Encounter);
        
        foreach (var field in Battlefields)
        {
            if(field.type.Equals(_Encounter.battlefieldID))
                field.Mesh.SetActive(true);
        }

        isEncounterInstantiated = true;
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

            playedGodCard = card as GodCard_Behaviour;
            SoundPlayer.PlaySound(playedGodCard.CardSO.enterBattlefield_SFX, gameObject);


            Transform cardTransform = playedGodCard.transform;

            cardTransform.parent.SetParent(_GodLane);
            cardTransform.parent.localPosition = new Vector3();
            cardTransform.localPosition = new Vector3();
            cardTransform.localPosition = new Vector3();
            cardTransform.parent.localRotation = new Quaternion();
            cardTransform.rotation = new Quaternion();
            cardTransform.GetComponent<Card_Selector>().enabled = false;
            cardTransform.parent.localScale = new Vector3(1.09843659f,1.09843659f,1.09843659f); // !!REMOVE THIS AFTER FINDING A PREFERABLE SIZE FOR THE CARDS

            GameObject spawn = Instantiate(playedGodCard.CardSO.God_Model, targetlane.position, transform.localRotation = new Quaternion(0, -0.577358961f, 0, 0.816490531f));
            spawn.transform.SetParent(cardTransform, true);

            // Debug.LogWarning("REMOVE SIZE HERE");
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
                cardTransform.parent.localPosition = new Vector3();
                cardTransform.localPosition = new Vector3();
                cardTransform.localPosition = new Vector3();
                cardTransform.parent.localRotation = new Quaternion();
                cardTransform.rotation = new Quaternion();
                cardTransform.GetComponent<Card_Selector>().enabled = false;
                cardTransform.parent.localScale = new Vector3(1.09843659f,1.09843659f,1.09843659f);  // !!REMOVE THIS AFTER FINDING A PREFERABLE SIZE FOR THE CARDS


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
                            renderer.material.SetColor("_MainColor", AttackColor);
                        else if(behaviour.GetCardType == CardType.Defence)
                            renderer.material.SetColor("_MainColor", DefendColor);
                        else
                            renderer.material.SetColor("_MainColor", UtilityColor);

                        // Targeting positions
                        ProceduralPathMesh Mesh = spawn.GetComponent<ProceduralPathMesh>();
                        Mesh.startPoint.position = cardTransform.position;                    
                        Mesh.endPoint.position = behaviour.AllTargets[k].transform.position;
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
    Fenrir
}