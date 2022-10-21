using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using FMODUnity;

public abstract class IMonster : MonoBehaviour
{
    public Intent GetIntent() => enemyIntent;
    protected Intent enemyIntent;
    
    [SerializeField]
    int maxHealth;
    int health;

    [SerializeField]
    private EventReference SoundSelectCard;

    int defendedFor;

    protected Attack_Behaviour attacker;

    // TurnManager manager;

    PlayerController player;
    God_Behaviour god = null;
    public God_Behaviour getGod => god;

    [SerializeField]
    Image image;
    [SerializeField]
    Text strengh;
    [SerializeField]
    TMP_Text healthTxt;

    [SerializeField]
    Sprite attackIcon;
    [SerializeField]
    Sprite abilityIcon;

    [SerializeField]
    Image arrowImage;
    private bool locked;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        healthTxt.text = "HP: " + health.ToString();

        image.enabled = false;
        strengh.enabled = false;

        if(enemyIntent == null)
            Debug.LogError("Monster Intent Refactoring is not done");

    }

    public int GetMaxHealth() { return maxHealth; }
    public int GetHealth() { return health; }

    private void OnMouseDown()
    {
        // if (manager.CurrentlySelectedCard())
        // {
        //     if (attacker)
        //     {
        //         attacker.AddTarget(this);
        //         SoundManager.Instance.Playsound(SoundSelectCard, gameObject);
        //     }
        //     attacker = null;
        //     EnemyHideArrow();
        // }
    }

    public void Initialize(PlayerController controller)
    {
        player = controller;
    }

    public void SetGod(God_Behaviour beh = null)
    {
        god = beh;
    }

    public void DealDamage(int amount)
    {
        if (amount > defendedFor)
        {
            health = health - (amount - defendedFor);
            defendedFor = 0;
        }
        else
        {
            defendedFor -= amount;
        }

        if (health <= 0)
        {
            health = 0;
            // manager.EnemyDied(this);
            Destroy(this.gameObject);
        }

        healthTxt.text = "HP: " + health.ToString();
    }

    internal void DecideIntent(BoardStateController board)
    {
        enemyIntent.DecideIntent(board);
    }

    internal void LateDecideIntent(BoardStateController board)
    {
        enemyIntent.LateDecideIntent(board);

        strengh.text = enemyIntent.GetCurrStrengh().ToString();

        image.enabled = true;
        strengh.enabled = true;
    }

    public void IsObjectiveTo(Attack_Behaviour attack_Behaviour)
    {
        attacker = attack_Behaviour;
        if (player)
        {
            EnemyShowArrow();
        }
        //Debug.Log(this + " can be attacked by " + attack_Behaviour);
    }

    public void Defend(int amount)
    {
        defendedFor += amount;
    }

    public void Act(BoardStateController board)
    {
        enemyIntent.Act(board);
        //waitforanimations
    }

    private void OnMouseOver()
    {
        arrowImage.color = Color.red;
    }

    private void OnMouseExit()
    {
        arrowImage.color = Color.white;
    }

    public void EnemyShowArrow()
    {
        if (player)
        {
            arrowImage.enabled = true;
        }
    }

    public void EnemyHideArrow()
    {
        arrowImage.enabled = false;
    }
}
