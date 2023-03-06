using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainTower : BoardTarget
{


    [SerializeField]
    private GameObject gfx;
    [SerializeField]

    private void Start()
    {

        currentHealth = maxHealth;
        Board = Component.FindObjectOfType<BoardStateController>();
        Board.ExtraEnemyTargets.Add(this);

    }

    public override void TakeDamage(int _amount)
    {

        currentHealth -= _amount;

        if(currentHealth <= 0)
        {

            DeActivate();

        }

    }

    private void DeActivate()
    {

        gfx.SetActive(false);
        Board.ExtraEnemyTargets.Remove(this);

    }

    public void ReActivate()
    {



    }

}
