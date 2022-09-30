using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public abstract class IMonster : MonoBehaviour
{
    [SerializeField]
    int maxHealth;
    int health;

    [SerializeField]
    short minAttack, maxAttack;

    bool attacking;

    bool attackingPlayer;


    protected int intentStrengh;
    int defendedFor;

    protected Attack_Behaviour attacker;

    TurnManager manager;

    PlayerController player;
    God_Behaviour god = null;

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

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        healthTxt.text = "HP: " + health.ToString();

        image.enabled = false;
        strengh.enabled = false;
    }

    public int GetMaxHealth() { return maxHealth; }
    public int GetHealth() { return health; }

    private void OnMouseDown()
    {
        if (manager.CurrentlySelectedCard())
        {
            if (attacker)
            {
                attacker.AddTarget(this);
            }
            attacker = null;
            HideArrow();
        }
    }

    public void Initialize(TurnManager man, PlayerController controller)
    {
        manager = man;
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
            manager.EnemyDied(this);
            Destroy(this.gameObject);
        }

        healthTxt.text = "HP: " + health.ToString();
    }

    internal void DecideIntent(BoardState board)
    {
        if (!UsesAbility(board))
        {
            attacking = true;
            image.sprite = attackIcon;
            intentStrengh = UnityEngine.Random.Range(minAttack, maxAttack + 1);
            attackingPlayer = AttackingPlayer(board);
            Debug.Log("Is attacking player?: " + attackingPlayer + " for: " + intentStrengh + " damage");
        }
        else
        {
            attacking = false;
            image.sprite = abilityIcon;
            AbilityDecided(board);
        }

        strengh.text = intentStrengh.ToString();

        image.enabled = true;
        strengh.enabled = true;
    }

    public virtual void IsObjectiveTo(Attack_Behaviour attack_Behaviour)
    {
        attacker = attack_Behaviour;
        ShowArrow();
        //Debug.Log(this + " can be attacked by " + attack_Behaviour);
    }

    public void Defend(int amount)
    {
        defendedFor += amount;
    }

    public void Act()
    {
        if (attacking)
        {
            if (attackingPlayer)
            {
                player.DealDamage(intentStrengh);
            }
            else
            {
                god.DealDamage(intentStrengh);
            }
        }
        else
        {
            DoAbility();
        }
        defendedFor = 0;
    }

    public void ShowArrow()
    {
        arrowImage.enabled = true;
    }

    public void HideArrow()
    {
        arrowImage.enabled = false;
    }

    public virtual void OnTurnBegin() { }
    public virtual void PreAbilityDecide() { }
    protected virtual bool UsesAbility(BoardState board) { return false; }
    protected virtual void AbilityDecided(BoardState board) { }
    protected virtual bool AttackingPlayer(BoardState board) { return !board.currentGod || UnityEngine.Random.Range(0, 2) == 1; }
    protected virtual void DoAbility() { }
    public virtual void OnTurnEnd() { }
    
    
}
