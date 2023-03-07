using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainTower : BoardTarget
{


    [SerializeField]
    private GameObject gfx;
    [SerializeField]
    private CardPlayData specialGleipnirCard;

    private void Start()
    {

        currentHealth = maxHealth;
        Board = Component.FindObjectOfType<BoardStateController>();
        Board.ActiveExtraEnemyTargets.Add(this);

    }

    public override void TakeDamage(int _amount)
    {

        currentHealth -= _amount;

        if(currentHealth <= 0)
        {

            DeActivate();

        }

    }

    protected override void DeActivate()
    {

        gfx.SetActive(false);
        IsActive = false;
        Board.ActiveExtraEnemyTargets.Remove(this);
        GameObject.FindObjectOfType<DeckController>().AddCardToLib(specialGleipnirCard);

    }

    public override void ReActivate()
    {

        currentHealth = maxHealth;
        gfx.SetActive(true);
        IsActive = true;
        Board.ActiveExtraEnemyTargets.Add(this);

    }

}
