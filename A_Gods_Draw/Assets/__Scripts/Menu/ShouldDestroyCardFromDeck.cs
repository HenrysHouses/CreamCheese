/*
 * Written by:
 * Henrik
 * 
 */

using UnityEngine;

public class ShouldDestroyCardFromDeck : MonoBehaviour
{
    [SerializeField] GameObject healButton;

    private void Start() {
        if(GameManager.instance.shouldDestroyCardInDeck)
            healButton.SetActive(false);
    }

    public void DestroyCardNextTimeInLibrary()
    {
        GameManager.instance.DestroyCardFromDeck();
    }
}
