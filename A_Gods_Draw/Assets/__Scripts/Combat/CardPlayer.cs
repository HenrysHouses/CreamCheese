using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayer : MonoBehaviour
{
    // * Mechanic References
    [SerializeField] Player_Hand _Hand;
    [SerializeField] BoardStateController _Board;

    // * Variables
    Camera mainCam;
    [SerializeField] LayerMask cardLayer;
    [SerializeField] LayerMask laneLayer;

    Card_Behaviour _selectedCard;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            if(_selectedCard is null)
            {
                _selectedCard = selectCard();
                return;
            }
            
            Transform selectedLane;
            if(playCard(out selectedLane))
            {
            }

            _selectedCard = null;
        }
    }

    Card_Behaviour selectCard()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit, 1000000, cardLayer))
        {
            Card_Behaviour behaviour = hit.collider.GetComponentInChildren<Card_Behaviour>();
            Debug.Log(hit.collider.name);
            return behaviour;
        }
        return null;
    }

    bool playCard(out Transform lane)
    {
        lane = null;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit, 100, laneLayer))
        {
            lane = hit.collider.transform;
        }

        if(lane is not null)
            return true;
        return false;
    }

    void placeCard(Transform lane, Card_Behaviour behaviour)
    {
        // _Hand.behaviour.Add();
        // int index _Hand.behaviour.IndexOf()
        // _Hand.RemoveCard()
        _Board.placeCardOnLane(lane, behaviour);
    }
}