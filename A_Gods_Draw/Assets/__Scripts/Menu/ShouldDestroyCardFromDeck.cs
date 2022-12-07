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
        cameraMovement = GameObject.FindObjectOfType<CameraMovement>();
        cameraMovement.LookUp();
    }

    public void DestroyCardNextTimeInLibrary()
    {
        GameManager.instance.DestroyCardFromDeck();
    }
}
