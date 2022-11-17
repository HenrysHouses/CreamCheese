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

    public GameObject deathParticleVFX;
    [SerializeField] Renderer[] MonsterRenderers;
    public float outlineSize = 0.01f;
    bool outlineShouldTurnOff;

    [SerializeField]
    int maxHealth;
    int health;

    int weakened = 0;

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

        weakened = 0;

        healthTxt.text = "HP: " + health.ToString();

        image.enabled = false;
        strengh.enabled = false;

        defendTxt.enabled = false;
    }

    private void Update() 
    {
        UpdateOutline();
    }


    public int GetMaxHealth() { return maxHealth; }
    public int GetHealth() { return health; }

    public void DealDamage(int amount)
    {
        if (weakened > 0)
            amount++;

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
            if (deathParticleVFX != null)
                Instantiate(deathParticleVFX,image.transform.position,Quaternion.identity);
            Destroy(this.gameObject);
        }

        healthTxt.text = "HP: " + health.ToString();
    }

    internal void DecideIntent(BoardStateController board)
    {
        enemyIntent.CancelIntent();

        if (weakened > 0)
            weakened--;

        enemyIntent.DecideIntent(board);
    }

    internal void LateDecideIntent(BoardStateController board)
    {
        enemyIntent.LateDecideIntent(board);

        UpdateUI();
    }

    public void Weaken(int amount)
    {
        weakened += amount;
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

    public void setOutline(float size)
    {
        if(size > 0)
            outlineShouldTurnOff = false;

        foreach (var rend in MonsterRenderers)
        {
            if(rend.materials.Length > 1)
                rend.materials[1].SetFloat("_Size", size);            
        }
    }

    private void UpdateOutline()
    {
        if(outlineShouldTurnOff)
            setOutline(0);
        else
            outlineShouldTurnOff = true;
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
