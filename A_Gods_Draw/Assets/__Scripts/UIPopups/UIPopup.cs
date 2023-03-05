using UnityEngine;

public class UIPopup : MonoBehaviour
{
    public Popup_ScriptableObject PopupInfo;
    
    void Start()
    {
        BoardElementsInfo info = GetComponent<BoardElementsInfo>();
        
        if(info)
            PopupInfo = info.PopupInfo;
    }

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

    public void setDescription(string description)
    {
        Popup_ScriptableObject tempInfo = PopupInfo;
        PopupInfo = ScriptableObject.CreateInstance<Popup_ScriptableObject>();
        tempInfo.Clone(ref PopupInfo);
        PopupInfo.Info = description;
    }
}
