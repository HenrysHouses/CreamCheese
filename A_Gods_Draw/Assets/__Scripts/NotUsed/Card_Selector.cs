using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using FMODUnity;

public class Card_Selector : MonoBehaviour
{ 
    [SerializeField] StudioEventEmitter cardflip;
    
    
    public bool holdingOver;
    public string cardsound;
    

     private void Start()
    {
        

     //  cardflip = GetComponent<FMODUnity.StudioEventEmitter>();

    }

    

  public void OnMouseOver()
    {
        holdingOver = true;
        //cardflip.SetParameter("Card Effects" , 0);
//        RuntimeManager.PlayOneShot(cardsound,transform.position);
        
        
        
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