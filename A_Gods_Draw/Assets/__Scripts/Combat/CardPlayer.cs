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

    Card_Loader _selectedCard;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            if(_selectedCard is null)
            {
                _selectedCard = selectCard();
                Debug.Log(_selectedCard);
                return;
            }
            
            Transform selectedLane;
            if(playCard(out selectedLane))
            {
                placeCard(selectedLane, _selectedCard);
            }

            _selectedCard = null;
            Debug.Log("unselected");
        }
    }

    Card_Loader selectCard()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit, 1000000, cardLayer))
        {
            Card_Loader _Loader = hit.collider.GetComponentInChildren<Card_Loader>();
            Debug.Log(hit.collider.name);
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

    void placeCard(Transform lane, Card_Loader loader)
    {
        Card_Behaviour behaviour = loader.GetComponent<Card_Behaviour>();
        int index = _Hand.cardLoaders.IndexOf(loader);
        _Hand.RemoveCard(index);
        _Board.placeCardOnLane(lane, behaviour);
    }
}