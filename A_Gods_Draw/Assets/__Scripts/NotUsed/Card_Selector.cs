using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Card_Selector : MonoBehaviour
{ 
    private Card_Behaviour CB;
    private Animator anim;
    [SerializeField] EventReference cardflip;
    public ParamRef pp;
    
    
    public bool holdingOver;
    

     private void Start()
    {
        CB = gameObject.GetComponentInChildren<Card_Behaviour>();
        anim = GetComponent<Animator>();
        
      

    }

     private void Update() 
     {
        // if(CB.IsThisSelected)
        // {
        //     anim.SetBool("SelectedCard", true);
            
        // }
        // else
        //  {
        //     anim.SetBool("SelectedCard", false);
        //  }

    }

    

    public void OnMouseEnter()
    {
        holdingOver = true;
        
        SoundPlayer.PlaySound(cardflip, gameObject);
        anim.SetBool("SelectedCard", true);
       // Debug.Log("Called");
    }

    public void OnMouseExit()
    {
        holdingOver = false;
        //SoundManager.Instance.StopSound(cardflip,gameObject);
        //anim.SetBool("SelectedCard", false);
        
    }

    private void OnMouseUp()
    {
        gameObject.GetComponentInChildren<Card_Behaviour>().OnBeingClicked();
    }





}