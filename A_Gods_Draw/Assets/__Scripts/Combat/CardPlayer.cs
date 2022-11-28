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

    bool shouldCancelSelection;
    Card_Behaviour _selectedCard;
    Card_Selector _currSelectedCard;
    [SerializeField] PathController path;
    [SerializeField] float cardSelectSpeed;
    float SelectedCardT = 0;

    [HideInInspector]
    public bool playedSFX;
    private bool hasClickedLastFrame;

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
            hasClickedLastFrame = true;
        }
        if (hasClickedLastFrame)
        {
            hasClickedLastFrame = false;

            if (_selectedCard is null)
            {
                _selectedCard = selectCard();
                if (_selectedCard is null)
                    return;

                SelectedCardT = 0;
                _currSelectedCard = _selectedCard.GetComponentInParent<Card_Selector>();
                path.controlPoints[0].position = _selectedCard.ParentTransform.position;
                path.recalculatePath();
                _currSelectedCard.disableSelection();
                shouldCancelSelection = false;
                return;
            }
            else
            {
                if (_selectedCard.ShouldCancelSelection())
                {
                    _selectedCard.CancelSelection();
                    _selectedCard = null;
                    shouldCancelSelection = true;
                    path.controlPoints[0].position = _currSelectedCard.targetHandPos;
                    Debug.Log("unselected");
                }
            }
        }

        if(shouldCancelSelection)
        {
            OrientedPoint OP = path.GetEvenPathOP(SelectedCardT);
            
            if(!_selectedCard)
            {
                removeSelection();
                return;
            }

            _selectedCard.ParentTransform.position = OP.pos;
            SelectedCardT = Mathf.Clamp01(SelectedCardT - Time.deltaTime * cardSelectSpeed);

            if(SelectedCardT == 0)
            {
                Debug.Log(OP.pos + " - " + _selectedCard.ParentTransform.position);
                removeSelection();
            }
            return;
        }


        if(_selectedCard)
        {
            _currSelectedCard.holdingOver = true;
            OrientedPoint OP = path.GetEvenPathOP(SelectedCardT);
            _selectedCard.ParentTransform.position = OP.pos;
            SelectedCardT = Mathf.Clamp01(SelectedCardT + Time.deltaTime * cardSelectSpeed);
        }

    }

    void removeSelection()
    {
        shouldCancelSelection = false;
        _selectedCard = null;
        
        if(_currSelectedCard)
            StartCoroutine(_currSelectedCard.enableSelection(_Hand));
        _currSelectedCard = null;
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
                //SoundPlayer.PlaySound(monster.hoverOver_SFX, gameObject);
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
            if (_Loader)
                if (_Loader.CanBeSelected())
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