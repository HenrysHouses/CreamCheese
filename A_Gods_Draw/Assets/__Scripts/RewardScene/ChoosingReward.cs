//modified by Charlie

using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum CardType
{
    Attack,
    Defence,
    Buff,
    Special,
    God,
    None
}
public class ChoosingReward : MonoBehaviour
{
    [SerializeField] DeckManager_SO deckManager;
    
    List<Card_SO> searchResult;

    public Transform[] spots;
    Card_SO[] CardOptions;
    public GameObject prefab;

    [SerializeField]
    LayerMask laneLayer;

    private void Start()
    {
        CardOptions = new Card_SO[spots.Length];
        
        GettingType(GameManager.instance.nextRewardType);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int SelectIndex = SelectReward();
            deckManager.addCardToDeck(CardOptions[SelectIndex]);
            if (SelectIndex > -1)
            {
                MultiSceneLoader.loadCollection("Map", collectionLoadMode.difference);
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
            case NodeType.DefenceReward:
            case NodeType.BuffReward:
                searchResult = CardSearch.Search<NonGod_Card_SO>(new string[] { GameManager.instance.nextRewardType.ToString() });
                break;
        }
        InstantiateCards();
    }

    void InstantiateCards()
    {
        for (int i = 0; i < spots.Length; i++)
        {
            GameObject spawn = Instantiate(prefab, spots[i]);

            Debug.Log(searchResult);

            int randomIndex = Random.Range(0, searchResult.Count);
            Card_SO randomCard = searchResult[randomIndex];
            Card_Loader _Loader = spawn.GetComponentInChildren<Card_Loader>();
            _Loader.shouldAddComponent = false;
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
