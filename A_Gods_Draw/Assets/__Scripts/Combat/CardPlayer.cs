/* 
 * Written by 
 * Henrik
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayer : MonoBehaviour
{
    // * Mechanic References
    [SerializeField] Player_Hand _Hand;
    [SerializeField] BoardStateController _Board;
    [SerializeField] GameObject ProceduralMeshPrefab;

    // * Variables
    Camera mainCam;
    [SerializeField] LayerMask cardLayer;
    [SerializeField] LayerMask laneLayer;

    bool shouldCancelSelection;
    List<GameObject> MeshSelections = new List<GameObject>();
    Card_Behaviour _selectedCard;
    Card_Selector _currSelectedCard;
    [SerializeField] PathController path;
    [SerializeField] Transform CardHoverPos, BuffCardHoverPos;
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

                if(_selectedCard is null) // checks if no card was selected
                    return;

                _selectedCard.OnBeingClicked();

                // get ready to move selected card to hover position
                SelectedCardT = 0;
                _currSelectedCard = _selectedCard.GetComponentInParent<Card_Selector>();

                NonGod_Behaviour _card = _selectedCard as NonGod_Behaviour;
                if(_card is null)
                    return;

                if(_card.CardSO.type.Equals(CardType.Buff))
                    path.controlPoints[1].position = BuffCardHoverPos.position;
                else
                    path.controlPoints[1].position = CardHoverPos.position;

                path.controlPoints[0].position = _selectedCard.ParentTransform.position;
                path.recalculatePath();
                _currSelectedCard.disableHover();
                shouldCancelSelection = false;

                StartCoroutine(spawnSelectionMesh(_card)); // TODO multiple targets
                return;
            }
            else
            {
                _selectedCard.OnClickOnSelected();

                if (_selectedCard.ShouldCancelSelection())
                {
                    _selectedCard.CancelSelection();
                    // _selectedCard = null;
                    shouldCancelSelection = true;
                    path.controlPoints[0].position = _currSelectedCard.targetHandPos;
                    Debug.Log("unselected");
                    clearTargetMeshes();
                }
            }
        }

        if(shouldCancelSelection)
        {
            if(!_selectedCard || SelectedCardT == 0)
            {
                removeSelection();
                return;
            }
            
            // Moves deselected card back to hand
            OrientedPoint OP = path.GetEvenPathOP(SelectedCardT);
            _selectedCard.ParentTransform.position = OP.pos;
            SelectedCardT = Mathf.Clamp01(SelectedCardT - Time.deltaTime * cardSelectSpeed);

            if(_selectedCard is not NonGod_Behaviour _card || !_card.GetCardType.Equals(CardType.Buff))
                    return;

            Vector3 rot = _selectedCard.transform.eulerAngles;
            rot.x -=  cardSelectSpeed * Time.deltaTime * 200;
            rot.x = Mathf.Clamp(rot.x, 0, 89);

            _selectedCard.transform.eulerAngles = rot;
            return;
        }

        // Moves selected card to hover position
        if(_selectedCard)
        {
            _currSelectedCard.holdingOver = true;
         
            updateMeshTargeting();

            if(SelectedCardT != 1)
            {
                OrientedPoint OP = path.GetEvenPathOP(SelectedCardT);
                _selectedCard.ParentTransform.position = OP.pos;
                SelectedCardT = Mathf.Clamp01(SelectedCardT + Time.deltaTime * cardSelectSpeed);

                Transform targetOrigin = getCurrSelectionMesh().GetChild(1);
                targetOrigin.position = _selectedCard.transform.position;    
                Vector3 localPos = targetOrigin.transform.localPosition;
                localPos.z += 0.1f;
                targetOrigin.transform.localPosition = localPos;

                if(_selectedCard is not NonGod_Behaviour _card || !_card.GetCardType.Equals(CardType.Buff))
                        return;

                Vector3 rot = _selectedCard.transform.eulerAngles;
                rot.x +=  cardSelectSpeed * Time.deltaTime * 100;
                rot.x = Mathf.Clamp(rot.x, 0, 89);

                _selectedCard.transform.eulerAngles = rot;
            }
        }
    }

    void updateMeshTargeting()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mainCam.nearClipPlane + 1;
        getCurrSelectionMesh().GetChild(0).position = mainCam.ScreenToWorldPoint(mousePos);
    }

    void clearTargetMeshes()
    {
        foreach (var mesh in MeshSelections)
        {
            Destroy(mesh);
        }
        MeshSelections.Clear();
    }

    Transform getCurrSelectionMesh()
    {
        if(MeshSelections.Count == 0)
            return null;

        return MeshSelections[MeshSelections.Count-1].transform;
    }

    IEnumerator spawnSelectionMesh(NonGod_Behaviour Card) // TODO multiple targets
    {
        instantiateTargetingMesh(Card.transform);

        for (int i = 0; i < Card.actions.Count; i++)
        {
            int lastTargetIndex = 0;
            int currLastTargetNum = Card.actions[i].actions[0].targets.Count;
            int totalLastTargetNum = Card.actions[i].nTargets;
            
            while(currLastTargetNum < totalLastTargetNum)
            {
                currLastTargetNum = Card.actions[i].actions[0].targets.Count;

                if(currLastTargetNum != lastTargetIndex)
                {
                    Debug.Log("Update");

                    getCurrSelectionMesh().GetChild(0).position = Card.actions[i].actions[0].targets[lastTargetIndex].transform.position;
                    instantiateTargetingMesh(Card.transform);

                    lastTargetIndex = currLastTargetNum;
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }

    void instantiateTargetingMesh(Transform Card)
    {
        GameObject spawn = Instantiate(ProceduralMeshPrefab);
        spawn.transform.GetChild(1).position = Card.parent.position;
        
        Vector3 localPos = spawn.transform.localPosition;
        localPos.z += 0.1f;
        spawn.transform.localPosition = localPos;
        
        spawn.transform.GetChild(1).localScale = new Vector3(0,0, 0.01f);
        MeshSelections.Add(spawn);
        spawn.transform.position = Vector3.zero;

        spawn.GetComponent<Renderer>().material.SetColor("_MainColor", Color.white);
    }

    void removeSelection()
    {
        shouldCancelSelection = false;
        _selectedCard = null;
        
        if(_currSelectedCard)
            StartCoroutine(_currSelectedCard.enableHover(_Hand));
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
        {
            return true;
        }
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
        clearTargetMeshes();
    }
}