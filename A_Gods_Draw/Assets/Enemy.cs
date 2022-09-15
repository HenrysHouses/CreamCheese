using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public int health;

    Attack_Behaviour attacker;

    [SerializeField]
    Image image;
    [SerializeField]
    Text strengh;

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

    private void OnMouseDown()
    {
        if (attacker)
        {
            attacker.AddTarget(this);
            Debug.Log(attacker + " is going to attack this");
        }
        attacker = null;
    }

    public void DealDamage(int amount)
    {
        health -= amount;
    }

    public void DecideIntent()
    {
        image.enabled = true;
        strengh.enabled = true;
    }

    public void IsObjectiveTo(Attack_Behaviour attack_Behaviour)
    {
        attacker = attack_Behaviour;
        Debug.Log(this + " can be attacked by " + attack_Behaviour);
    }
}
