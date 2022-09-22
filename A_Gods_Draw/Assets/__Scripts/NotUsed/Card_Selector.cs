using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Card_Selector : MonoBehaviour
{ 
    
    
    public bool holdingOver;
    
    
    
    /*
    
    [SerializeField] Card_SO card_SO;
    private Vector3 HoverCardpos;
    
    private bool holdsOverForLonger;
    public float smoothTime = 0.1f; // How much time it takes for the card to op up
    public float BackInHandTime = 0.003f; //Amounts of time it takes to get back in hand, 
    float xVelocity = 0f;
    float yVelocity = 0f;
    private float zVelocity = 0f;
    private bool isDragging;
    private Vector3 startPosition; // Change to handPos
    private Vector3 mousePos;
    public Vector3 Target;
    private GameObject camera;
    public Hand _hand;
    public float tiltAngle;

    // Start is called before the first frame update
    void Start()
    {
        
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        startPosition = gameObject.transform.position;
        Target = new Vector3(startPosition.x, startPosition.y + 0.03f, startPosition.z - 0.15f);

        Hand.CardInHand instance = new Hand.CardInHand();
        instance.cards_SO = card_SO;
        instance.gameObject = gameObject;
        instance.hoverTrigger.AddListener(OnMouseDown);
        _hand.AddCard(this);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
       // gameObject.transform.LookAt(camera.transform,Vector3.up);
        if (holdingOver)
        {   Debug.Log("Holding Over");
            float newX = Mathf.SmoothDamp(transform.position.x, Target.x, ref xVelocity, smoothTime);
            float newY = Mathf.SmoothDamp(transform.position.y, Target.y, ref yVelocity, smoothTime);
            float newZ = Mathf.SmoothDamp(transform.position.z, Target.z, ref zVelocity, smoothTime);

            transform.position = new Vector3(newX, newY, newZ);


            // if (holdingTimer > 0.2f)
            // {
            //     holdingOver = false;
            //     holdsOverForLonger = true;
            //     transform.position = HoverCardpos;
            //
            // }
        }
        else
        {
            
          
        }
        
    }

  

    void OnMouseExit()
    {
        holdingOver = false;
         // float oldPos = Mathf.SmoothDamp(transform.position.y, startPosition.y, ref yVelocity, smoothTime);
         // transform.position = new Vector3(transform.position.x, oldPos, transform.position.y);
    }

     void OnMouseDown()
    {
     if(holdingOver)
     {
        //refrence to deckmanager
     }

    }*/

    

  public void OnMouseOver()
    {
        holdingOver = true;
        
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