using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastMenuHighlight : MonoBehaviour
{
    [SerializeField] LayerMask layer;
    public bool invertHoldHighlights;
    public bool HighlightWhenInspecting;
    [SerializeField] CardReaderController cardInspector;
    Camera MainCam;

    void Start()
    {
        cardInspector = GameObject.FindObjectOfType<CardReaderController>();
        MainCam = Camera.main;
    }

    void Update()
    {
        findHighlight();
    }

    void findHighlight()
    {

        if(!HighlightWhenInspecting)
        {
            if(cardInspector != null)
                if(cardInspector.isInspecting)
                    return;
        }

        Debug.Log("sdas");

        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);

        if(!Physics.Raycast(ray, out RaycastHit hit, 100, layer))
            return;
        
        DisableHighlight found = hit.collider.GetComponent<DisableHighlight>();

        if(found == null)
            return;

        if(invertHoldHighlights)
        {
            found.highlight.SetActive(false);
            return;
        }

        found.highlight.SetActive(true);
        found.StayEnabled();
    }
}
