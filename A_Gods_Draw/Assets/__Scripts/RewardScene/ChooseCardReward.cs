//modified by Charlie

using Map;
using System.Collections.Generic;
using UnityEngine;
using HH.MultiSceneTools;
// using static UnityEditor.Progress;
using System.Collections;
using HH.MultiSceneTools.Examples;
using static UnityEngine.ParticleSystem;

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

    //card confirmation
    [Header("Card Confirmation")]
    [SerializeField] CardRewardOption[] rewardOptions;
    public bool shouldConfirmSelection;
    bool confirmed;
    [SerializeField] Transform EndOfPath, EndPosition;
    [SerializeField] GameObject deckpileParticle;
    bool isClicked;
    List<GameObject> descObjects = new List<GameObject>();


    //disable board text when selected
    [SerializeField] GameObject[] boardText;

    private void Start()
    {
        CardOptions = new Card_SO[spots.Length];
        CameraMovement.instance.SetCameraView(CameraView.CardReward);
        
        GettingType(GameManager.instance.nextRewardType);


        StartCoroutine(LoadAfterCards());
    }

    IEnumerator LoadAfterCards()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < rewardOptions.Length; i++)
        {
            Card_Loader loader = rewardOptions[i].GetComponentInChildren<Card_Loader>();
            rewardOptions[i].GetComponentInChildren<Card_InspectingPopup>().SetDescriptions(loader.GetCardSO as ActionCard_ScriptableObject, rewardOptions[i].gameObject);
            descObjects.Add(rewardOptions[i].transform.GetChild(0).GetChild(0).gameObject);
            Debug.Log(descObjects[i]);
        }
    }

    // bool hasClicked = false;
    private void Update()
    {
        if(GameManager.instance.PauseMenuIsOpen)
        {
            return;
        }

        if (!confirmed)
            checkSelected();

        if (CardInspector.isInspecting)
        {
            boardText[0].SetActive(false);
            boardText[1].SetActive(false);
            if(isClicked)
            {
                foreach (Transform objects in deckpileParticle.transform)
                {
                    EnableParticleSystems(objects);
                    for (int i = 0; i < rewardOptions.Length; i++)
                    {
                        if(rewardOptions[i].transform == CardInspector.getTarget().transform)
                            descObjects[i].SetActive(true);
                    }
                }
                isClicked = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                CardInspector.returnInspection();
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!CardInspector.isInspecting)
                {
                    CardInspector.returnInspection();
                    return;
                }
            }
            // else
            //     hasClicked = false;
        }
        else
        {
            for (int i = 0; i < descObjects.Count; i++)
            {
                descObjects[i].SetActive(false);
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
                searchResult = CardSearch.Search<ActionCard_ScriptableObject>(new string[] { "Eir" });
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
            rewardOptions[i].AddToDeck = searchResult[randomIndex];
            searchResult.RemoveAt(randomIndex);
        }
    }

    void checkSelected()
    {
        boardText[0].SetActive(true);
        boardText[1].SetActive(true);
        //deckpileParticle.SetActive(false);
        if (!isClicked)
        {
            foreach (Transform objs in deckpileParticle.transform)
            {
                DisableParticleSystems(objs);
                isClicked = true;
            }
        }
        
        for (int i = 0; i < rewardOptions.Length; i++)
        {
            if (rewardOptions[i].isBeingInspected && shouldConfirmSelection)
            {
                confirmDeck(rewardOptions[i]);
                break;
            }
        }
        shouldConfirmSelection = false;
    }

    void confirmDeck(CardRewardOption Selected)
    {
        confirmed = true;
        DeckList_SO.playerObtainCard(Selected.AddToDeck);
        GameSaver.SaveData(GameManager.instance.PlayerTracker.CurrentDeck.deckData.GetDeckData());
        Map_Manager.SavingMap();
        StartCoroutine(animateDeck(Selected));
    }

    IEnumerator animateDeck(CardRewardOption selected)
    {
        EndOfPath.position = EndPosition.position;
        yield return new WaitUntil(() => !selected.isBeingInspected);
        yield return new WaitForSeconds(0.3f);
        MultiSceneLoader.loadCollection("Map", collectionLoadMode.Difference);
    }

    private void DisableParticleSystems(Transform target)
    {
        ParticleSystem[] particles = target.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Stop();
            particles[i].Clear();
        }
    }
    private void EnableParticleSystems(Transform target)
    {
        ParticleSystem[] particles = target.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
    }
}
