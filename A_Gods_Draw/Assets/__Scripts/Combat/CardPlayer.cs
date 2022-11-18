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

    [HideInInspector]
    public bool playedSFX;

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
        if (_selectedCard is NonGod_Behaviour card)
        {
            switch (card.GetCardType)
            {
                case CardType.Attack:
                case CardType.Defence:
                    setEnemyHighlight();
                    break;
                case CardType.Buff:
                    setCardHighlight();
                    break;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (_selectedCard is null)
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

    private void setEnemyHighlight()
    {
        int layer = 1 << 9;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        IMonster monster = null;


        if (Physics.Raycast(ray, out RaycastHit hit, 10000, layer))
        {
            monster = hit.collider.GetComponent<IMonster>();

            if (!playedSFX)
            {
                SoundPlayer.PlaySound(monster.hoverOver_SFX, gameObject);
                playedSFX = true;

            }
            monster.setOutline(monster.outlineSize);
        }
        if(monster == null)
        {
            playedSFX = false;
        }
    }

    private void setCardHighlight()
    {
        int layer = 1 << 6;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        CardHighlight highlight = null;


        if (Physics.Raycast(ray, out RaycastHit hit, 10000, layer))
        {
            GameObject card = hit.collider.gameObject;
            highlight = card.GetComponent<CardHighlight>();
            
            if (highlight)
            {
                highlight.EnableHighlight();

                if (!playedSFX)
                {
                    SoundPlayer.PlaySound(highlight.hoveringover_SFX, gameObject);
                    playedSFX = true;

                }
            }
            if(highlight == null)
            {
                playedSFX = false;
            }
        }

    }

    Card_Behaviour selectCard()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000000, cardLayer))
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

        if (Physics.Raycast(ray, out RaycastHit hit, 100, laneLayer))
        {
            lane = hit.collider.transform;
        }

        if (lane is not null)
            return true;
        return false;
    }

    void placeCard(Card_Behaviour behaviour)
    {
        Card_Loader loader = behaviour.GetComponent<Card_Loader>();
        God_Behaviour _God = behaviour as God_Behaviour;

        if (_God is null)
        {
            if (_Board.isGodPlayed)
            {
                _Board.playedGodCard.CardSO.StartDialogue(GodDialogueTrigger.Played, loader.GetCardSO);
            }
            CardHighlight highlight = behaviour.GetComponentInChildren<CardHighlight>();
            highlight.enabled = true;
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