using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Hand : MonoBehaviour
{   public GameObject cardPrefab;
    public Card_SO cardSo;
    private float xVelocity,yVelocity,zVelocity;
    public float smoothTime;
    private Vector3 startPos;
    private GameObject camera;
   // private Vector3 Target;
    //public float time;
    // public float angle;
     private bool holdingOver;

    
    public class CardInHand 
    {
        public Vector3 position;
        public Card_Selector CS;
        public Vector3 hoverPos;

        public CardInHand()
        {
            hoverPos = new Vector3(position.x, position.y + 0.5f, position.z -0.5f);
        }
    
    }


    private List<CardInHand> CardsInHand = new List<CardInHand>(); 
   
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.gameObject;

        //startPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + cardPosRelativeTohand, transform.position.z);
        //Target = new Vector3(startPos.x, startPos.y + 20f, startPos.z - 20f);
       // Target = new Vector3(startPosition.x, startPosition.y + 0.03f, startPosition.z - 0.15f);

        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.A))
        {
            GameObject newCards = Instantiate(cardPrefab);
            newCards.GetComponent<Card_Selector>();
            AddCard(newCards.GetComponent<Card_Selector>(), cardSo);
            

        }
        for (int i = 0; i < CardsInHand.Count ; i++)
        {
            
            if(CardsInHand[i].CS.holdingOver)
            {   
                Debug.Log(CardsInHand[i].CS.transform.position);
                float newX = Mathf.SmoothDamp(transform.position.x, CardsInHand[i].hoverPos.x, ref xVelocity, smoothTime);
                float newY = Mathf.SmoothDamp(transform.position.y, CardsInHand[i].hoverPos.y, ref yVelocity, smoothTime);
                float newZ = Mathf.SmoothDamp(transform.position.z, CardsInHand[i].hoverPos.z, ref zVelocity, smoothTime);
            }
            else 
            {
                 float oldPos = Mathf.SmoothDamp(transform.position.y, CardsInHand[i].position.y, ref yVelocity, smoothTime);
                 CardsInHand[i].CS.transform.position = new Vector3(transform.position.x, oldPos, transform.position.y);
            }
        }
        
    }


    public void AddCard(Card_Selector target, Card_SO card_So)
    {
        GameObject newPivotPoint = new GameObject("CardPivot");
        newPivotPoint.transform.SetParent(transform, false);
        newPivotPoint.transform.position = new Vector3(0,0f,0);
        target.transform.SetParent(newPivotPoint.transform);
        // listOfCards.Add(instance);

        CardInHand card = new CardInHand();
        card.CS = target;
        card.position = newPivotPoint.transform.position + new Vector3 (0,0,0);
        CardsInHand.Add(card);



        //target = newPivotPoint.transform.position + new Vector3(2,0f,0);

    }

      public void RemoveCard(GameObject target, Card_SO card)
    {
        target.transform.parent = null;



    }


}
