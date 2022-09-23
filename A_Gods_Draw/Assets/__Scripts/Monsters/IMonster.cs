using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IMonster : MonoBehaviour
{
    [SerializeField]
    short minAttack, maxAttack;

    bool attacking;

    bool attackingPlayer;

    [SerializeField]
    int maxHealth;
    int health;

    int intentStrengh;
    int defendedFor;

    protected Attack_Behaviour attacker;

    PlayerController player;
    God_Behaviour god = null;

    [SerializeField]
    Image image;
    [SerializeField]
    Text strengh;

    [SerializeField]
    Sprite attackIcon;
    [SerializeField]
    Sprite abilityIcon;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        image.enabled = false;
        strengh.enabled = false;
    }

    public int GetMaxHealth() { return maxHealth; }
    public int GetHealth() { return health; }

    private void OnMouseDown()
    {
        if (attacker)
        {
            attacker.AddTarget(this);
        }
        attacker = null;
    }

    public void SetPlayer(PlayerController controller)
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
            health -= (amount - defendedFor);
    }

    public void DecideIntent(List<IMonster> enemies, List<TurnManager.LaneInfo> currentLane, PlayerController player, God_Behaviour currentGod)
    {
        if (!UsesAbility(enemies, currentLane, player, currentGod))
        {
            attacking = true;
            image.sprite = attackIcon;
            intentStrengh = UnityEngine.Random.Range(minAttack, maxAttack + 1);
            attackingPlayer = AttackingPlayer(player, currentGod);
        }
        else
        {
            image.sprite = abilityIcon;
            AbilityDecided(enemies, currentLane, player, currentGod);
        }

        strengh.text = intentStrengh.ToString();

        image.enabled = true;
        strengh.enabled = true;

    }

    public virtual void IsObjectiveTo(Attack_Behaviour attack_Behaviour)
    {
        attacker = attack_Behaviour;
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
                //god.dealdamage();
            }
        }
        else
        {
            DoAbility();
        }
        defendedFor = 0;
    }

    public virtual void OnTurnBegin() { }
    public virtual void PreAbilityDecide() { }
    protected virtual bool UsesAbility(List<IMonster> enemies, List<TurnManager.LaneInfo> currentLane, PlayerController player, God_Behaviour currentGod) { return false; }
    protected virtual void AbilityDecided(List<IMonster> enemies, List<TurnManager.LaneInfo> currentLane, PlayerController player, God_Behaviour currentGod) { }
    protected virtual bool AttackingPlayer(PlayerController player, God_Behaviour god) { return UnityEngine.Random.Range(0, 2) == 1; }
    protected virtual void DoAbility() { }
    public virtual void OnTurnEnd() { }
    
    
}
