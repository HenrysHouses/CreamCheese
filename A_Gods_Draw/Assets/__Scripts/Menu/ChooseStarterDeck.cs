using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HH.MultiSceneTools;

public class ChooseStarterDeck : MonoBehaviour
{
    [SerializeField] StarterDeckOption[] Options;
    [SerializeField] Transform EndOfPath, EndPosition;
    public bool shouldConfirmSelection;
    bool confirmed;

    // Update is called once per frame
    void Update()
    {
        if(!confirmed)
            checkSelected();
    }

    void checkSelected()
    {
        for (int i = 0; i < Options.Length; i++)
        {
            if(Options[i].isBeingInspected && shouldConfirmSelection)
            {
                confirmDeck(Options[i]);       
                break; 
            }
        }
        shouldConfirmSelection = false;
    }

    void confirmDeck(StarterDeckOption Selected)
    {
        confirmed = true;
        GameManager.instance.PlayerTracker.setDeck(Selected.StarterDeck.deckData);
        GameSaver.SaveData(Selected.StarterDeck.deckData.GetDeckData());
        StartCoroutine(animateDeck(Selected));
    }

    IEnumerator animateDeck(StarterDeckOption selected)
    {
        EndOfPath.position = EndPosition.position;
        yield return new WaitUntil(() => !selected.isBeingInspected);
        GameManager.instance.shouldGenerateNewMap = true;
        yield return new WaitForSeconds(0.3f);
        MultiSceneLoader.loadCollection("Map", collectionLoadMode.Difference);
    }
}
