using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardElementsInfo : MonoBehaviour
{

    [field:SerializeField] public Popup_ScriptableObject PopupInfo {private set; get;}
    
    // [SerializeField, Tooltip("If left unassigned, will use the tagged main camera")]
    // private Camera mainCamera;
    // [SerializeField]
    // private GameObject informationPopupWindow;
    // [SerializeField, Tooltip("Delay before information shows up in SECONDS")]
    // private float infoDelay;
    // private int delayCounter;
    // private RaycastHit previousInfoElement;

    // private void Start()
    // {

    //     if(!mainCamera)
    //         mainCamera = Camera.main;
    // }

    // private void FixedUpdate()
    // {

    //     if(Input.GetKey(KeyCode.Mouse1) && Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit _hit, 50))
    //     {

    //         if(_hit.transform != previousInfoElement.transform)
    //         {

    //             previousInfoElement = _hit;
    //             // Reset();
    //             return;

    //         }

    //         InfoElement _info;

    //         if(!_hit.transform.TryGetComponent<InfoElement>(out _info))
    //         {

    //             // Reset();
    //             return;

    //         }

    //         // delayCounter++;

    //         // if(delayCounter >= infoDelay * 60)
    //         // {

    //         //     informationText.text = _info.BoardElementInfo;

    //         // }
    //         // else
    //         // {

    //         //     informationPopupWindow.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
    //         //     informationText.text = "";
            
    //         // }

    //     }
    //     else
    //         Reset();

    // }

    // private void Reset()
    // {

    //     informationText.text = "";
    //     delayCounter = 0;

    // }

}