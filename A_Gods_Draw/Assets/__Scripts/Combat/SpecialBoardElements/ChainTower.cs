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
    [SerializeField]
    private Renderer[] renderers;
    [SerializeField]
    private float outlineSize;
    [SerializeField]
    private Color outlineColor;

    private void Start()
    {

        currentHealth = maxHealth;
        Board = Component.FindObjectOfType<BoardStateController>();
        Board.AddBoardTarget(this);
        DeactivateOutline();

    }

    public override void DealDamage(int _amount, UnityEngine.Object _source = null)
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

    public override void Targeted(GameObject _sourceGO)
    {

        foreach(Renderer _renderer in renderers)
        {

            if(_renderer.materials.Length < 2)
                continue;

            _renderer.materials[1].SetFloat("_Size", outlineSize);            
            _renderer.materials[1].SetColor("_Color", outlineColor);

        }

    }

    public void DeactivateOutline()
    {

        foreach(Renderer _renderer in renderers)
        {

            if(_renderer.materials.Length < 2)
                continue;
                
            _renderer.materials[1].SetFloat("_Size", 0);            
            _renderer.materials[1].SetColor("_Color", outlineColor);

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
