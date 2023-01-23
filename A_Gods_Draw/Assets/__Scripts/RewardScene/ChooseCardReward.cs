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
    Special,
    God,
    None
}
public class ChooseCardReward : MonoBehaviour
{
    [SerializeField] CardReaderController CardInspector;
    [SerializeField] DeckManager_SO deckManager;
    
    List<Card_SO> searchResult = new();

    public Transform[] spots;
    Card_SO[] CardOptions;
    public GameObject prefab;

    [SerializeField]
    LayerMask laneLayer;

    
    //TODO make item database and add here

    private void Start()
    {
        CardOptions = new Card_SO[spots.Length];
        
        GettingType(GameManager.instance.nextRewardType);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(CardInspector.isInspecting)
            {
                CardInspector.returnInspection();
                return;
            }

            int SelectIndex = SelectReward();

            if (SelectIndex > -1)
            {
                deckManager.addCardToDeck(CardOptions[SelectIndex]);
                MultiSceneLoader.loadCollection("Map", collectionLoadMode.Difference);
            }
        }
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
                searchResult = CardSearch.Search<NonGod_Card_SO>(new string[] { CardType.Attack.ToString() });
                break;
            case NodeType.DefenceReward:
                searchResult = CardSearch.Search<NonGod_Card_SO>(new string[] { CardType.Defence.ToString() });
                break;
            case NodeType.BuffReward:
                searchResult = CardSearch.Search<NonGod_Card_SO>(new string[] { CardType.Buff.ToString() });
                break;
            case NodeType.GodReward:
                searchResult = CardSearch.Search<NonGod_Card_SO>(new string[] { "Tyr" });
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
            _Loader.Set(randomCard);

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
