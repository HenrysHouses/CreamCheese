using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public int health;
    int intentStrengh;
    int defendedFor;
    bool attacking;

    Attack_Behaviour attacker;

    PlayerController player;
    God_Behaviour god = null;
    Enemy toDefend;

    [SerializeField]
    Image image;
    [SerializeField]
    Text strengh;

    [SerializeField]
    Sprite attackIcon;
    [SerializeField]
    Sprite defendIcon;

    // Start is called before the first frame update
    void Start()
    {
        image.enabled = false;
        strengh.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayer(PlayerController controller)
    {
        player = controller;
    }
    public void SetGod(God_Behaviour beh = null)
    {
        god = beh;
    }

    private void OnMouseDown()
    {
        if (attacker)
        {
            attacker.AddTarget(this);
            //Debug.Log(attacker + " is going to attack this");
        }
        attacker = null;
    }

    public void DealDamage(int amount)
    {
        if (!attacking)
        {
            if (amount > intentStrengh + defendedFor)
                health -= (amount - intentStrengh - defendedFor);
        }
        else
        {
            if (amount > defendedFor)
                health -= (amount - defendedFor);
        }
    }

    public void DecideIntent(List<Enemy> enemies)
    {
        image.enabled = true;
        strengh.enabled = true;

        intentStrengh = UnityEngine.Random.Range(3, 40);
        strengh.text = intentStrengh.ToString();

        attacking = UnityEngine.Random.Range(0, 2) == 1;
        if (attacking)
        {
            image.sprite = attackIcon;
        }
        else
        {
            image.sprite = defendIcon;
            enemies[UnityEngine.Random.Range(0, enemies.Count)].Defend(intentStrengh);
        }
    }

    public void IsObjectiveTo(Attack_Behaviour attack_Behaviour)
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
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                player.DealDamage(intentStrengh);
            }
            else
            {
                //god.dealdamage();
            }
        }
        defendedFor = 0;
    }
}
