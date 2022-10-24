using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using FMODUnity;

public abstract class IMonster : BoardElement
{
    public Intent GetIntent() => enemyIntent;
    protected Intent enemyIntent;
    
    [SerializeField]
    int maxHealth;
    int health;

    [SerializeField]
    private EventReference SoundSelectCard;

    int defendedFor;

    TurnController controller;

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

    protected override void OnClick()
    {
        controller.GetBoard().SetClickedMonster(this);
    }

    public int GetMaxHealth() { return maxHealth; }
    public int GetHealth() { return health; }

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

    public void Defend(int amount)
    {
        defendedFor += amount;
    }

    public void Act()
    {
        enemyIntent.Act(controller.GetBoard());
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
    }

    public void EnemyHideArrow()
    {
        arrowImage.enabled = false;
    }
}
