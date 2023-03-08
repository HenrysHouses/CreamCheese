using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ChainTower : BoardTarget
{


    [SerializeField]
    private GameObject gfx, damageEffect, chainbreak;
    [SerializeField]
    private Vector3 effectOffset;
    [SerializeField]
    private CardPlayData specialGleipnirCard;
    [SerializeField]
    private EventReference chainsnap;

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

        if (currentHealth <= 0)
        {
            SoundPlayer.PlaySound(chainsnap, gameObject);
            Destroy(GameObject.Instantiate(damageEffect, transform.position, Quaternion.identity), 0.4f);


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
