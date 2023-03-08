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

        CameraMovement.instance.SetCameraView(CameraView.Library);
    }

    public void DestroyCardNextTimeInLibrary()
    {
        GameManager.instance.DestroyCardFromDeck();
    }
}
