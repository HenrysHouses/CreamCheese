using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HH.MultiSceneTools;
using HH.MultiSceneTools.Examples;

public class ConfirmDeck : MonoBehaviour
{
    [SerializeField] Animator ConfirmAnimation;
    public DeckList_SO targetDeck;
    [SerializeField] SceneTransition transition;

    bool hasConfirmed = false;

    void OnMouseDown()
    {
        if(targetDeck == null || hasConfirmed)
            return;

        hasConfirmed = true;
        GameSaver.SaveData(targetDeck.deckData.GetDeckData());
        GameManager.instance.PlayerTracker.setDeck(targetDeck.deckData);
        CameraMovement.instance.SetCameraView(CameraView.Reset);

        StartCoroutine(DoConfirmAnimation());
    }

    IEnumerator DoConfirmAnimation()
    {
        ConfirmAnimation.SetTrigger("Confirm");

        yield return new WaitUntil(() => ConfirmAnimation.GetCurrentAnimatorStateInfo(0).IsName("ConfirmDeck"));
        yield return new WaitForSeconds(ConfirmAnimation.GetCurrentAnimatorStateInfo(0).length);
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
