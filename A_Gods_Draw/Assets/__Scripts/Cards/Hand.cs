using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hand: MonoBehaviour
{   
    public class CardInHand
    {
        public float time;
        public Card_SO cards_SO;
        public GameObject gameObject;
        public UnityEvent hoverTrigger;

        public CardInHand()
        {
            hoverTrigger = new UnityEvent( );
        }
    }
    
    private Card_Selector booling;
    private List<CardInHand> listOfCards = new List<CardInHand>();
    public float tiltAngle;
    public float smoothTime;
    

    public void AddCard(CardInHand instance)
    {
        GameObject newPivotPoint = new GameObject("CardPivot");
        newPivotPoint.transform.SetParent(transform, false);
        newPivotPoint.transform.position = new Vector3(2,0f,0);
        instance.gameObject.transform.SetParent(newPivotPoint.transform);
        listOfCards.Add(instance);
    }


    public void RemoveCard(GameObject target, Card_SO card)
    {
        target.transform.parent = null;
        for (int i = 0; i < listOfCards.Count; i++)
              
        {
            if(listOfCards[i].cards_SO == card)
            {
                listOfCards.RemoveAt(i);
            }
            
        }

    }
    // Update is called once per frame
    void Update()
    {
        

        
         bool cardRotationCheck = false;
         if(listOfCards.Count > 0)
         {
            while(!cardRotationCheck)
            {
                for (int i = 0; i < listOfCards.Count; i++)
                {
                    // Quaternion cardTilt = Quaternion.Euler(0,0,tiltAngle * i);
                    Vector3 euler = listOfCards[i].gameObject.transform.parent.eulerAngles;
                    euler.z += tiltAngle * i; 
                    Quaternion Q = Quaternion.Euler(0, 0, euler.z);
                    

                    float t = Mathf.Clamp( listOfCards[i].time * smoothTime * 10, 0, 1);
                    Quaternion newRot = Quaternion.Slerp(transform.rotation, Q,t);
                    Debug.Log(listOfCards[i].gameObject.transform.parent.rotation.eulerAngles);
                    
                    if(listOfCards[i].gameObject.transform.parent.rotation.eulerAngles.z <= tiltAngle * i)
                        listOfCards[i].gameObject.transform.parent.rotation = newRot;
                    listOfCards[i].time += Time.deltaTime;
                    Debug.Log(listOfCards[i].gameObject.transform.parent.rotation.eulerAngles);
                    listOfCards[i].gameObject.transform.parent.position = new Vector3(2,0,0);

                    listOfCards[i].gameObject.transform.position = Vector3.MoveTowards(listOfCards[i].gameObject.transform.position, transform.position, 0.003f);

                    // Debug.Log(t);
                }
                for (int i = 0; i < listOfCards.Count; i++)
                {
                    if(listOfCards[i].gameObject.transform.parent.rotation.eulerAngles.z >= tiltAngle * i)
                    {
                        cardRotationCheck = true;
                    }
                }
                 Debug.Log(cardRotationCheck);
            }
         }
    }
}
