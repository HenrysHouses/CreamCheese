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
        if (_selectedCard && _selectedCard.CardIsReady())
        {
            _selectedCard.Placed();
            placeCard(_selectedCard);
            _selectedCard = null;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if(_selectedCard is null)
            {
                _selectedCard = selectCard();
                return;
            }
            //if (_selectCard.MissedClick())
            if (_selectedCard.CancelSelection())
            {
                _selectedCard = null;
                Debug.Log("unselected");
            }
        }
    }

    Card_Behaviour selectCard()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit, 1000000, cardLayer))
        {
            Card_Behaviour _Loader = hit.collider.GetComponentInChildren<Card_Behaviour>();
            if (_Loader.IsOnHand())
                return _Loader;
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

    void placeCard(Card_Behaviour behaviour)
    {
        Card_Loader loader = behaviour.GetComponent<Card_Loader>();
        God_Behaviour _God = behaviour as God_Behaviour;

        if(_God is null)
        {
            if(_Board.isGodPlayed)
            {
                _Board.playedGodCard.CardSO.StartDialogue(GodDialogueTrigger.Played , loader.GetCardSO);
            }
        }
        else
        {
            _God.CardSO.StartDialogue(GodDialogueTrigger.SeeEnemy, _Board.Enemies[0]);
            _God.CardSO.StartDialogue(GodDialogueTrigger.Played, loader.GetCardSO);
        }


        _Hand.RemoveCard(loader);
        _Board.placeCardOnLane(behaviour);
        behaviour.OnPlacedInLane();
    }
}