using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HH.MultiSceneTools;
using HH.MultiSceneTools.Examples;

public class ConfirmDeck : MonoBehaviour
{
    public DeckList_SO targetDeck;
    [SerializeField] SceneTransition transition;

    void OnMouseDown()
    {
        if(targetDeck == null)
            return;

        GameSaver.SaveData(targetDeck.deckData.GetDeckData());
        GameManager.instance.PlayerTracker.setDeck(targetDeck.deckData);
        transition.TransitionScene(false, "Map");
    }

    public void reset()
    {
        targetDeck = null;
    }

    public void setDeck(DeckList_SO deck)
    {
        targetDeck = deck;
    }
}
