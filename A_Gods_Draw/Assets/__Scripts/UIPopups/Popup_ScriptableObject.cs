using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "UI/Popup")]
public class Popup_ScriptableObject : ScriptableObject
{
    public PopupLocation SpawnLocation;
    public float Scale = 10;
    public Vector2 ShadowOffset = new Vector2(1,1);
    public Sprite Background;
    public Color BackgroundColor = new Color(1,1,1,1);
    public Material BackgroundMaterial;
    [TextArea(5, 30)]
    public string Info = "Popup Info";
    public TMP_FontAsset TextFont;
    public Color TextColor = new Color(0,0,0,1);
    public Material TextMaterial;
}

[System.Serializable]
public enum PopupLocation
{
    RightTop,
    RightBottom,
    LeftTop,
    LeftBottom
}