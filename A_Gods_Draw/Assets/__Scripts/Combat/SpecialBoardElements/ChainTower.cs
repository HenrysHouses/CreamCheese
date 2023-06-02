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
    private bool targeted;
    public GameObject gleipnirAnimObj;

    private void Start()
    {

        currentHealth = maxHealth;
        Board = Component.FindObjectOfType<BoardStateController>();
        Board.AddBoardTarget(this);
        DeactivateOutline();
       
        

    }

    private void Update()
    {

        if(!targeted)
            return;
        
        foreach(Renderer _renderer in renderers)
        {

            if(_renderer.materials.Length < 2)
                continue;
            
            _renderer.materials[1].SetFloat("_Size", outlineSize * Mathf.PingPong(Time.time, 1f));

        }

    }

    public override void DealDamage(int _amount, UnityEngine.Object _source = null)
    {

        currentHealth -= _amount;

        DeactivateOutline();
        Destroy(GameObject.Instantiate(damageEffect, transform.position, Quaternion.identity), 0.4f);

        if (currentHealth <= 0)
        {

            SoundPlayer.PlaySound(chainsnap, gameObject);
            Destroy(GameObject.Instantiate(damageEffect, transform.position, Quaternion.identity), 0.4f);

            DeActivate();

        }

    }

    public override void Targeted(GameObject _sourceGO = null)
    {

        foreach(Renderer _renderer in renderers)
        {

            if(_renderer.materials.Length < 2)
                continue;

            targeted = true;
            _renderer.materials[1].SetColor("_Color", outlineColor);

        }

    }

    public override void UnTargeted(GameObject _sourceGO = null)
    {

        DeactivateOutline();
        
    }

    public void DeactivateOutline()
    {

        foreach(Renderer _renderer in renderers)
        {

            if(_renderer.materials.Length < 2)
                continue;
                
            targeted = false;
            _renderer.materials[1].SetFloat("_Size", 0);
            _renderer.materials[1].SetColor("_Color", Color.white);

        }

    }

    protected override void DeActivate()
    {

        gfx.SetActive(false);
        IsActive = false;
        Board.ActiveExtraEnemyTargets.Remove(this);
        
        GameObject.FindObjectOfType<DeckController>().AddCardToLib(specialGleipnirCard);
        Instantiate(gleipnirAnimObj,Vector3.zero,Quaternion.identity);  
       
        

    }

    public override void ReActivate()
    {

        currentHealth = maxHealth;
        gfx.SetActive(true);
        IsActive = true;
        Board.ActiveExtraEnemyTargets.Add(this);

    }

}
