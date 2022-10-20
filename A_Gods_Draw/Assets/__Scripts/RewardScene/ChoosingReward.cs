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
    CardType currtype;

    DeckManager_SO deckManager;
    Card_Behaviour behaviour;
    List<Card_SO> searchResult;

    public Transform[] spots;
    public GameObject prefab;

    [SerializeField]
    LayerMask laneLayer;

    private void Start()
    {
        behaviour = GetComponentInChildren<Card_Behaviour>();
        searchResult = CardSearch.Search<NonGod_Card>(new string[] {currtype.ToString()});
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectReward();
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
            case NodeType.DefenceReward:
            case NodeType.BuffReward:
                InstantiateCards();
                break;
        }
    }

    void InstantiateCards()
    {
        GameObject spawn1 = Instantiate(prefab, spots[0]);

        Card_SO randomCard = searchResult[Random.Range(0, searchResult.Count)];
        spawn1.GetComponentInChildren<Card_Loader>().Set(randomCard, null);
    }

    int SelectReward()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, laneLayer))
        {
            for (int i = 0; i < spots.Length; i++)
            {
                if (hit.collider.transform.Equals(spots[i]))
                {
                    return i;
                }
            }
        }

        return -1;
    }
}
