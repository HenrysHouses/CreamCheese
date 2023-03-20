using UnityEngine;

public class UIPopup : MonoBehaviour
{
    public Popup_ScriptableObject PopupInfo;
    public bool CreateInstance;
    
    void Start()
    {
        BoardElementsInfo info = GetComponent<BoardElementsInfo>();
        
        if(info)
        {
            if(CreateInstance)
            {
                info.PopupInfo.Clone(ref PopupInfo);
            }
            else
                PopupInfo = info.PopupInfo;
        }
        else if(PopupInfo != null && CreateInstance)
        {
            Popup_ScriptableObject temp = ScriptableObject.CreateInstance<Popup_ScriptableObject>();
            PopupInfo.Clone(ref temp);
            PopupInfo = temp;
            Debug.Log("instance created");
        }
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
