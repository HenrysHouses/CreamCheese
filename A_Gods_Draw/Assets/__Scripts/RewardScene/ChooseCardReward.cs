//modified by Charlie

using Map;
using System.Collections.Generic;
using UnityEngine;
using HH.MultiSceneTools;
public enum CardType
{
    Attack,
    Defence,
    Buff,
    Utility,
    God,
    None
}
public class ChooseCardReward : MonoBehaviour
{
    [SerializeField] CardReaderController CardInspector;
    
    List<Card_SO> searchResult = new();

    public Transform[] spots;
    Card_SO[] CardOptions;
    public GameObject prefab;

    [SerializeField]
    LayerMask laneLayer;

    

    private void Start()
    {
        CardOptions = new Card_SO[spots.Length];
        CameraMovement.instance.SetCameraView(CameraView.CardReward);
        
        GettingType(GameManager.instance.nextRewardType);
    }

    bool hasClicked = false;
    private void Update()
    {
        if(CardInspector.isInspecting)
        {
            if(Input.GetMouseButtonDown(1))
            {
                CardInspector.returnInspection();
            }

            if (Input.GetMouseButtonDown(0))
            {
                if(!CardInspector.isInspecting)
                {
                    CardInspector.returnInspection();
                    return;
                }

                int SelectIndex = SelectReward();

                if (SelectIndex > -1)
                {
                    DeckList_SO.playerObtainCard(CardOptions[SelectIndex]);
                    Map.Map_Manager.SavingMap();
                    MultiSceneLoader.loadCollection("Map", collectionLoadMode.Difference);
                }
            }

            if(Input.GetMouseButtonDown(0))
            {
                hasClicked = true;
            }

        }
        else
            hasClicked = false;
    }

    /// <summary>
    /// 
    /// </summary>
    public void GettingType(NodeType nodeType)
    {
        switch (nodeType)
        {
            case NodeType.Reward:
                searchResult = CardSearch.Search<Card_SO>();
                break;
            case NodeType.AttackReward:
                searchResult = CardSearch.Search<ActionCard_ScriptableObject>(new string[] { CardType.Attack.ToString() });
                break;
            case NodeType.DefenceReward:
                searchResult = CardSearch.Search<ActionCard_ScriptableObject>(new string[] { CardType.Defence.ToString() });
                break;
            case NodeType.BuffReward:
                searchResult = CardSearch.Search<ActionCard_ScriptableObject>(new string[] { CardType.Buff.ToString() });
                break;
            case NodeType.GodReward:
                searchResult = CardSearch.Search<ActionCard_ScriptableObject>(new string[] { "Tyr" });
                break;
                
        }
        InstantiateCards();
    }

    void InstantiateCards()
    {
        for (int i = 0; i < spots.Length; i++)
        {
            if (searchResult.Count <= 0)
            {
                break;
            }

            GameObject spawn = Instantiate(prefab, spots[i]);

            int randomIndex = Random.Range(0, searchResult.Count);
            Card_SO randomCard = searchResult[randomIndex];
            Card_Loader _Loader = spawn.GetComponentInChildren<Card_Loader>();
            _Loader.addComponentAutomatically = false;
            
            CardPlayData _randomData = new CardPlayData();
            _randomData.CardType = randomCard;
            _Loader.Set(_randomData);

            CardOptions[i] = searchResult[randomIndex];
            searchResult.RemoveAt(randomIndex);
        }
    }

    int SelectReward()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, laneLayer))
        {
            for (int i = 0; i < spots.Length; i++)
            {
                Transform target = hit.collider.transform.parent;
                if (target.Equals(spots[i]))
                {
                    return i;
                }
            }
        }
        return -1;
    }
}
