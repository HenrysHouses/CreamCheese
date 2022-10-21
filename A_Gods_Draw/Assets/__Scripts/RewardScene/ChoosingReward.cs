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
    God
}
public class ChoosingReward : MonoBehaviour
{
    [SerializeField] DeckManager_SO deckManager;
    CardType currtype;

    Card_Behaviour behaviour;
    List<Card_SO> searchResult;

    public Transform[] spots;
    Card_SO[] CardOptions;
    public GameObject prefab;

    [SerializeField]
    LayerMask laneLayer;

    private void Start()
    {
        CardOptions = new Card_SO[spots.Length];
        behaviour = GetComponentInChildren<Card_Behaviour>();
        
        //GettingType(mapNode);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int SelectIndex = SelectReward();
            // if(SelectIndex)
            Debug.Log(deckManager.getDeck.Deck.Count);
            deckManager.addCardToDeck(CardOptions[SelectIndex]);
            Debug.Log(deckManager.getDeck.Deck.Count);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void GettingType(Map_Nodes mapNode)
    {
        switch (mapNode.Node.nodeType)
        {
            case NodeType.AttackReward:
                searchResult = CardSearch.Search<Attack_Card>(new string[] {currtype.ToString()});
                break;
            case NodeType.DefenceReward:
                searchResult = CardSearch.Search<Defense_Card>(new string[] {currtype.ToString()});
                break;
            case NodeType.BuffReward:
                searchResult = CardSearch.Search<Buff_Card>(new string[] {currtype.ToString()});
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
