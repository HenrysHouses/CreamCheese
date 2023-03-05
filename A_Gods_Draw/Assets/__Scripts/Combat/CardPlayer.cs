/* 
 * Written by 
 * Henrik
 * 
 * Modified by Javier
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonoBehaviour that handles when the player want to play cards and feedback
/// </summary>
public class CardPlayer : MonoBehaviour
{
    // * Mechanic References
    [SerializeField] DeckController deckController;
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
        // Place the card on the board
        if (_selectedCard && _selectedCard.CardIsReady())
        {
            placeCard(_selectedCard);
            _selectedCard.Placed();
            _selectedCard = null;
        }
        // highlight feedback
        if (_selectedCard is ActionCard_Behaviour card)
        {
            switch (card.GetCardType)
            {
                case CardType.Attack:
                case CardType.Defence:
                case CardType.Utility:
                    setEnemyHighlight();
                    break;
                case CardType.Buff:
                    setCardHighlight();
                    break;
            }
        }

        // ? not sure
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            hasClickedLastFrame = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && _selectedCard != null)
        {
            hasClickedLastFrame = true;
            ActionCard_Behaviour actionCard_ = _selectedCard as ActionCard_Behaviour;
            actionCard_.MissClick(); 
        }

        // Selects a card
        if (hasClickedLastFrame)
        {
            hasClickedLastFrame = false;

            if (_selectedCard is null) 
            {
                _selectedCard = selectCard();
                // Debug.Log(_selectedCard);

                if(_selectedCard is null) // checks if no card was selected
                    return;

                _selectedCard.OnBeingClicked();

                // get ready to move selected card to hover position
                SelectedCardT = 0;
                _currSelectedCard = _selectedCard.GetComponentInParent<Card_Selector>();

                ActionCard_Behaviour _card = _selectedCard as ActionCard_Behaviour;
                if(_card is null)
                    return;

                if(_card.CardSO.type.Equals(CardType.Buff))
                    path.controlPoints[1].position = BuffCardHoverPos.position;
                else
                    path.controlPoints[1].position = CardHoverPos.position;

                path.controlPoints[0].position = _selectedCard.transform.position;
                path.recalculatePath();
                _currSelectedCard.disableHover();
                shouldCancelSelection = false;

                StartCoroutine(SpawnSelectionsForTargeting(_card));
                return;
            }
            else
            {
                // Check if selection should be cancelled
                _selectedCard.OnClickOnSelected();

                if (_selectedCard.ShouldCancelSelection())
                {
                    _selectedCard.CancelSelection();
                    shouldCancelSelection = true;
                    path.controlPoints[0].position = _currSelectedCard.targetHandPos;
                    clearTargetMeshes();
                }
            }
        }

        // waits to remove the selection
        if(shouldCancelSelection)
        {
            if(!_selectedCard || SelectedCardT == 0)
            {
                removeSelection();
                return;
            }
            
            // Moves deselected card back to hand
            OrientedPoint OP = path.GetEvenPathOP(SelectedCardT);
            _selectedCard.transform.position = OP.pos;
            SelectedCardT = Mathf.Clamp01(SelectedCardT - Time.deltaTime * cardSelectSpeed);

            if(_selectedCard is not ActionCard_Behaviour _card || !_card.GetCardType.Equals(CardType.Buff))
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
                _selectedCard.transform.position = OP.pos;
                SelectedCardT = Mathf.Clamp01(SelectedCardT + Time.deltaTime * cardSelectSpeed);

                Transform targetOrigin = getCurrSelectionMesh().GetChild(1);
                targetOrigin.position = _selectedCard.transform.position;    
                Vector3 localPos = targetOrigin.transform.localPosition;
                localPos.z += 0.1f;
                targetOrigin.transform.localPosition = localPos;

                if(_selectedCard is not ActionCard_Behaviour _card || !_card.GetCardType.Equals(CardType.Buff))
                        return;

                Vector3 rot = _selectedCard.transform.eulerAngles;
                rot.x +=  cardSelectSpeed * Time.deltaTime * 100;
                rot.x = Mathf.Clamp(rot.x, 0, 89);

                _selectedCard.transform.eulerAngles = rot;
            }
        }
    }

    /// <summary> Moves the procedural targeting mesh's end point to the cursor</summary>
    void updateMeshTargeting()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mainCam.nearClipPlane + 1;
        getCurrSelectionMesh().GetChild(0).position = mainCam.ScreenToWorldPoint(mousePos);
    }

    /// <summary>Destroys all procedural targeting meshes</summary>
    void clearTargetMeshes()
    {
        foreach (var mesh in MeshSelections)
        {
            Destroy(mesh);
        }
        MeshSelections.Clear();
    }

    /// <summary>Gets the last procedural targeting mesh that was instantiated</summary>
    Transform getCurrSelectionMesh()
    {
        if(MeshSelections.Count == 0)
            return null;

        return MeshSelections[MeshSelections.Count-1].transform;
    }

    // Waits for the player to select targets and instantiates new targeting meshes
    IEnumerator SpawnSelectionsForTargeting(ActionCard_Behaviour Card)
    {
        instantiateTargetingMesh(Card.transform);

        for (int i = 0; i < Card.stats.numberOfTargets; i++)
        {
            int lastTargetIndex = 0;
            
            while(Card.AllTargets.Length < Card.stats.numberOfTargets)
            {
                if(lastTargetIndex < Card.AllTargets.Length)
                {
                    getCurrSelectionMesh().GetChild(0).position = Card.AllTargets[lastTargetIndex].transform.position;
                    instantiateTargetingMesh(Card.transform);

                    lastTargetIndex++;
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }

    /// <summary>Instantiates a procedural targeting mesh at target transform of a card</summary>
    /// <param name="Card">Target transform which the start points will be set to</param>
    void instantiateTargetingMesh(Transform Card)
    {
        GameObject spawn = Instantiate(ProceduralMeshPrefab);
        spawn.transform.GetChild(1).position = Card.position;
        
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

    /// <summary>RayCasts enemies and enables a outline highlight</summary>
    private void setEnemyHighlight()
    {
        int layer = 1 << 9;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Monster monster = null;


        if (Physics.Raycast(ray, out RaycastHit hit, 10000, layer))
        {
            monster = hit.collider.GetComponent<Monster>();

            if (!playedSFX)
            {
               // SoundPlayer.PlaySound(monster.HoverOver_SFX, gameObject);
                playedSFX = true;

            }
            monster.setOutline(monster.outlineSize, Color.white);
        }
        if(monster == null)
        {
            playedSFX = false;
        }
    }

    /// <summary>RayCasts cards and enables a outline highlight</summary>
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

        RaycastHit[] hits = Physics.RaycastAll(ray, 1000000, cardLayer);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                Card_Behaviour _Loader = hits[i].collider.GetComponent<Card_Behaviour>();
                if (_Loader)
                    if (_Loader.CanBeSelected())
                        return _Loader;
            }

        }
        return null;
    }

    void placeCard(Card_Behaviour behaviour)
    {
        Card_Loader loader = behaviour.GetComponent<Card_Loader>();
        GodCard_Behaviour _God = behaviour as GodCard_Behaviour;
        deckController.MoveCardToBoard(behaviour.getCardPlayData());
        
        if(_God)
            _God.animator = _God.GetComponentInChildren<Animator>();

        if (_God is null)
        {
            if (_Board.isGodPlayed)
            {
                _Board.playedGodCard.CardSO.StartDialogue(GodDialogueTrigger.Played, loader.GetCardSO);
                GodCard_Behaviour godCard = _Board.getGodLane().GetComponentInChildren<GodCard_Behaviour>();
            }
            CardHighlight highlight = behaviour.GetComponentInChildren<CardHighlight>();
            highlight.enabled = true;
            // Debug.Log("Card_Behaviour Highlight not implemented");
        }
        else
        {
            for (int i = 0; i < _Board.Enemies.Length; i++)
            {
                _God.CardSO.StartDialogue(GodDialogueTrigger.SeeEnemy, _Board.Enemies[i]);
            }
            _God.CardSO.StartDialogue(GodDialogueTrigger.Played, loader.GetCardSO);
            ActionCard_Behaviour actionCard = behaviour as ActionCard_Behaviour;
        }
        _Hand.RemoveCard(loader);
        _Board.placeCardOnLane(behaviour);
        clearTargetMeshes();
    }
}