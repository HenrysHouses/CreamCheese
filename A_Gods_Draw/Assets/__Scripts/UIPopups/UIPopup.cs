using UnityEngine;

public class UIPopup : MonoBehaviour
{
    public Popup_ScriptableObject PopupInfo;
    

    void OnMouseEnter()
    {
        if(PopupInfo)
            PopupHold.instance.StartPopup(PopupInfo);
    }

    void OnMouseExit()
    {
        if(PopupInfo)
            PopupHold.instance.StopPopup();
    }
}
