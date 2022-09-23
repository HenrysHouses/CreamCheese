using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using FMODUnity;

public class Card_Selector : MonoBehaviour
{ 
   // [SerializeField] StudioEventEmitter cardSounds;
    
    
    public bool holdingOver;
    

     private void Start()
    {
        

     //  cardSounds = GetComponent<StudioEventEmitter>();

    }

    

  public void OnMouseOver()
    {
        holdingOver = true;
       //cardSounds.Play();
        //cardSounds.SetParameter("Card Effects" , 0);
        
    }

    public void OnMouseExit()
    {
        holdingOver = false;
    }

    private void OnMouseDown()
    {
        gameObject.GetComponentInChildren<Card_Behaviour>(true).OnClick();
    }





}