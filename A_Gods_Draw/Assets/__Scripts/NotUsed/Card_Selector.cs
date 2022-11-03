using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using FMODUnity;

public class Card_Selector : MonoBehaviour
{ 
    [SerializeField] EventReference cardflip;
    public ParamRef pp;
    
    
    public bool holdingOver;
    

     private void Start()
    {
        
        
      

    }

    

  public void OnMouseEnter()
    {
        holdingOver = true;
        
        SoundManager.Instance.Playsound(cardflip, gameObject);
       // Debug.Log("Called");
        
        
        
    }

    public void OnMouseExit()
    {
        holdingOver = false;
       // SoundManager.Instance.StopSound(cardflip,gameObject);
    }

    private void OnMouseDown()
    {
        gameObject.GetComponentInChildren<Card_Behaviour>(true).OnClick();
    }





}