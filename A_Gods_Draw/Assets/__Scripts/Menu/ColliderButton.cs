using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderButton : MonoBehaviour
{
    private bool lastInteractableState;
    public bool isInteractable = true;
    public void interactable(bool state) => isInteractable = state; 
    public UnityEvent OnClick = new UnityEvent();
    public UnityEvent OnInteractableDisabled = new UnityEvent();
    public UnityEvent OnInteractableEnabled = new UnityEvent();

    void Awake()
    {
        lastInteractableState = isInteractable;
    }

    void OnMouseDown()
    {
        if(!isInteractable)
            return;
        
        OnClick?.Invoke();
    }

    void Update()
    {
        if(lastInteractableState == isInteractable)
            return;

        if(isInteractable == false)
            OnInteractableDisabled?.Invoke();
        else
            OnInteractableEnabled?.Invoke();
        
        lastInteractableState = isInteractable;
    }

}
