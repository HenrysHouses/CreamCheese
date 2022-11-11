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

    public GameObject deathEffect;
    
    [SerializeField]
    int maxHealth;
    int health;

    [SerializeField]
    private EventReference SoundSelectCard,death_SFX;

    int defendedFor;

    [SerializeField]
    Image image;
    [SerializeField]
    Text strengh;
    [SerializeField]
    TMP_Text healthTxt;
    [SerializeField]
    TMP_Text defendTxt;

    [SerializeField]
    Sprite attackIcon;

    [SerializeField]
    Sprite abilityIcon;

    [SerializeField]
    Image arrowImage;
    private bool locked;

    // Start is called before the first frame update
    void Awake()
    {
        health = maxHealth;

        healthTxt.text = "HP: " + health.ToString();

        image.enabled = false;
        strengh.enabled = false;

        defendTxt.enabled = false;
    }

    public int GetMaxHealth() { return maxHealth; }
    public int GetHealth() { return health; }

    public void DealDamage(int amount)
    {
        if (amount >= defendedFor)
        {
            health = health - (amount - defendedFor);
            defendedFor = 0;
            image.color = Color.white;
            defendTxt.enabled = false;
        }
        else
        {
            defendedFor -= amount;
        }
        defendTxt.text = defendedFor.ToString();

        if (health <= 0)
        {
            health = 0;
            // manager.EnemyDied(this);
            SoundPlayer.PlaySound(death_SFX,gameObject);
            deathEffect.SetActive(true);
            Destroy(this.gameObject);
        }

        healthTxt.text = "HP: " + health.ToString();
    }

    internal void DecideIntent(BoardStateController board)
    {
        enemyIntent.CancelIntent();

        enemyIntent.DecideIntent(board);
    }

    internal void LateDecideIntent(BoardStateController board)
    {
        enemyIntent.LateDecideIntent(board);

        UpdateUI();
    }

    public void Defend(int amount)
    {
        defendedFor += amount;
        image.color = Color.cyan;
        // defendTxt.text = defendedFor.ToString(); // ! was trying to access after being destroyed so i commented it out
        // defendTxt.enabled = true;
    }

    public void DeBuff(int amount)
    {
        if (enemyIntent.GetID() == EnemyIntent.AttackGod || enemyIntent.GetID() == EnemyIntent.AttackPlayer)
        {
            enemyIntent.SetCurrStrengh(enemyIntent.GetCurrStrengh() - amount);
            if (enemyIntent.GetCurrStrengh() < 0)
            {
                enemyIntent.SetCurrStrengh(0);
            }
        }

        UpdateUI();
    }

    public void Buff(int amount)
    {
        if (enemyIntent.GetID() == EnemyIntent.AttackGod || enemyIntent.GetID() == EnemyIntent.AttackPlayer)
        {
            enemyIntent.SetCurrStrengh(enemyIntent.GetCurrStrengh() + amount);
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (strengh)
        {
            strengh.text = enemyIntent.GetCurrStrengh().ToString();
            strengh.enabled = true;
        }
        if (image)
        {
            image.sprite = enemyIntent.GetCurrentIcon();
            image.enabled = true;
        }
    }

    public void Act(BoardStateController board)
    {
        enemyIntent.Act(board, this);
        StartCoroutine(WaitForAnims());
    }

    IEnumerator WaitForAnims()
    {
        yield return new WaitForSeconds(0.5f);

        TurnController.shouldWaitForAnims = false;
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
