using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastMenuHighlight : MonoBehaviour
{
    [SerializeField] LayerMask layer;

    Camera MainCam;

    void Start()
    {
        MainCam = Camera.main;
    }

    void Update()
    {
        findHighlight();
    }

    void findHighlight()
    {
        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);

        if(!Physics.Raycast(ray, out RaycastHit hit, 100, layer))
            return;
        
        DisableHighlight found = hit.collider.GetComponent<DisableHighlight>();

        if(found == null)
            return;

        found.highlight.SetActive(true);
        found.StayEnabled();
    }
}
