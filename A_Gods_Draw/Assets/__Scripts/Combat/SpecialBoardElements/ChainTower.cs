using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainTower : BoardTarget
{


    [SerializeField]
    private GameObject gfx, damageEffect;
    [SerializeField]
    private Vector3 effectOffset;
    [SerializeField]
    private CardPlayData specialGleipnirCard;

    private void Start()
    {

        currentHealth = maxHealth;
        Board = Component.FindObjectOfType<BoardStateController>();
        Board.AddBoardTarget(this);

    }

    public override void TakeDamage(int _amount)
    {

        currentHealth -= _amount;

        Destroy(GameObject.Instantiate(damageEffect, transform.position, Quaternion.identity), 0.4f);

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
