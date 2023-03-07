using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderButton : MonoBehaviour
{
    private bool lastInteractableState;
    public bool interactable = true;
    public UnityEvent OnClick = new UnityEvent();
    public UnityEvent OnInteractableDisabled = new UnityEvent();
    public UnityEvent OnInteractableEnabled = new UnityEvent();

    void Awake()
    {
        lastInteractableState = interactable;
    }

    void OnMouseDown()
    {
        if(!interactable)
            return;
        
        OnClick?.Invoke();
    }

    void Update()
    {
        if(lastInteractableState == interactable)
            return;

        if(interactable == false)
            OnInteractableDisabled?.Invoke();
        else
            OnInteractableEnabled?.Invoke();
        
        lastInteractableState = interactable;
    }
}
