/*
 * Written by:
 * Henrik
 * 
 */

using UnityEngine;

public class ShouldDestroyCardFromDeck : MonoBehaviour
{
    [SerializeField] CameraMovement cameraMovement;

    private void Start() {
        CameraMovement.instance.SetCameraView(CameraView.Library);
    }

    public void DestroyCardNextTimeInLibrary()
    {
        GameManager.instance.DestroyCardFromDeck();
    }
}
