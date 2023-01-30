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

    public PlayerController Player => _Player;
    public Encounter_SO Encounter => _Encounter;
    public IMonster[] Enemies => _Enemies;
    public Transform getLane(int i) => _Lane[i];
    public Transform getGodLane() => _GodLane;
    [HideInInspector] public List<NonGod_Behaviour> placedCards;
    [HideInInspector] public List<NonGod_Behaviour> allPlayedCards;
    public bool isGodPlayed => playedGodCard;
    [HideInInspector] public God_Behaviour playedGodCard;
    [HideInInspector] public List<BoardElement> thingsInLane;
    public NonGod_Behaviour getCardInLane(int i) => placedCards[i];

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
    IMonster[] _Enemies;
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
    public void SetClickable(int whatToSet, bool clickable = true)
    {
        switch (whatToSet)
        {
            case 0:
                foreach (NonGod_Behaviour card in placedCards)
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
                foreach (IMonster ene in getLivingEnemies())
                {
                    ene.clickable = clickable;
                }
                break;
            default:
                break;
        }
    }

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

    public void PlayCard(NonGod_Behaviour card)
    {
        allPlayedCards.Add(card);
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
            if (thingsInLane.Count >= 4)
                return;
            targetlane = _Lane[thingsInLane.Count];
            SoundPlayer.PlaySound(placeCard_SFX, gameObject);
        }

        if (_GodLane.Equals(targetlane)) // Move card to the godlane
        {

            playedGodCard = card as God_Behaviour;
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

                NonGod_Behaviour behaviour = card as NonGod_Behaviour;

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


                for (int j = 0; j < behaviour.actions.Count; j++) // should run this for each targetable action
                {
                    BoardElement[] targets = behaviour.getActionTargets(j);

                    for (int k = 0; k < targets.Length; k++)
                    {   
                        if (targets[k] == null)
                            continue;

                        // Targeting instantiation
                        GameObject spawn = Instantiate(TargetingMesh, Vector3.zero, Quaternion.identity);
                        spawn.transform.SetParent(cardTransform, true);

                        // Targeting color
                        MeshRenderer renderer = spawn.GetComponent<MeshRenderer>();
                        if(behaviour.GetCardType == CardType.Attack)
                            renderer.material.SetColor("_MainColor", AttackColor);
                        else
                            renderer.material.SetColor("_MainColor", DefendColor);
                        
                        // Targeting positions
                        ProceduralPathMesh Mesh = spawn.GetComponent<ProceduralPathMesh>();
                        Mesh.startPoint.position = cardTransform.position;                    
                        Mesh.endPoint.position = targets[k].transform.position;
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

        NonGod_Behaviour currentCard = current as NonGod_Behaviour;
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