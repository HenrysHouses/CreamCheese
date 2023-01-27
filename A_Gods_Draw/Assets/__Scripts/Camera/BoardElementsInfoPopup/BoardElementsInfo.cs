using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardElementsInfo : MonoBehaviour
{

    [SerializeField, Tooltip("If left unassigned, will use the tagged main camera")]
    private Camera mainCamera;
    [SerializeField]
    private GameObject informationPopupWindow;
    private TMP_Text informationText;
    private Canvas canvas;
    [SerializeField, Tooltip("Delay before information shows up in SECONDS")]
    private float infoDelay;
    private int delayCounter;
    private RaycastHit previousInfoElement;

    private void Start()
    {

        if(!mainCamera)
            mainCamera = Camera.main;
        
        informationText = informationPopupWindow.GetComponent<TMP_Text>();

    }

    private void FixedUpdate()
    {

        Debug.Log(Input.mousePosition.x + " | " + Input.mousePosition.y);
        informationPopupWindow.GetComponent<RectTransform>().position = Input.mousePosition;//new Vector3(Mathf.Clamp(Input.mousePosition.x, 0, canvas.renderingDisplaySize.x), Mathf.Clamp(Input.mousePosition.y, 0, canvas.renderingDisplaySize.y), 0);


        if(Input.GetKey(KeyCode.P) && Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit _hit, 50))
        {

            InfoElement _info;

            if(!_hit.transform.TryGetComponent<InfoElement>(out _info))
            {

                delayCounter = 0;
                return;

            }

            if(delayCounter >= infoDelay * 60)
            {

                informationText.text = _info.BoardElementInfo;

            }
            else
                delayCounter++;

        }
        else
            delayCounter = 0;

    }

}