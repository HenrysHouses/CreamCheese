using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupHold : MonoBehaviour
{
    public static PopupHold instance;
    public GameObject PopupPrefab;
    public float HoverSpeedMultiplier = 1;
    [SerializeField] Canvas canvas;
    [SerializeField] Image Feedback;
    GameObject PopupSpawn;
    Coroutine routine;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 position = Camera.main.ScreenToViewportPoint(mousePos);
        position.x *=  Screen.width;
        position.x -=  Screen.width/2;
        position.y *= Screen.height;
        position.y -= Screen.height/2;
        position.z = 0;

        transform.localPosition = position;
        transform.rotation = canvas.transform.rotation;

        // Vector3 point = new Vector3();
        // Vector2 mousePos = new Vector2();

        // mousePos.x = Input.mousePosition.x;
        // mousePos.y = Camera.main.pixelHeight - Input.mousePosition.y;

        // point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, -mousePos.y+Screen.height, canvas.transform.position.z));
        // transform.position = point;
    }

    public void StartPopup(Popup_ScriptableObject Source)
    {
        if(routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(Popup(Source));
    }

    public void StopPopup()
    {
        StopCoroutine(routine);
        Feedback.fillAmount = 0;
        if(PopupSpawn)
            Destroy(PopupSpawn);
    }

    IEnumerator Popup(Popup_ScriptableObject Source)
    {
        while(Feedback.fillAmount < 1)
        {
            Feedback.fillAmount += Time.deltaTime * HoverSpeedMultiplier;
            yield return new WaitForEndOfFrame();
        }
        instantiatePopUp(Source);
    }

    void instantiatePopUp(Popup_ScriptableObject Panel)
    {
        Feedback.fillAmount = 0;
        PopupSpawn = Instantiate(PopupPrefab);
        PopupSpawn.transform.localScale = Vector3.one * Panel.Scale;
        PopupSpawn.transform.SetParent(Feedback.transform, false);

        RectTransform size = PopupSpawn.GetComponent<RectTransform>();

        Image _panel = PopupSpawn.GetComponent<Image>();
        _panel.sprite = Panel.Background;
        _panel.color = Panel.BackgroundColor;
        _panel.material = Panel.BackgroundMaterial;
        _panel.transform.rotation = canvas.transform.rotation;


        RectTransform shadow = PopupSpawn.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
        shadow.offsetMin = -Panel.ShadowOffset;
        PopupSpawn.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Panel.Background;

        TextMeshProUGUI _Text = PopupSpawn.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        _Text.text = Panel.Info;
        _Text.font = Panel.TextFont;
        _Text.color = Panel.TextColor;
        _Text.material = Panel.TextMaterial;
        _Text.transform.rotation = canvas.transform.rotation;

        PopupController controller = PopupSpawn.GetComponent<PopupController>();
        controller.SetAnchor(Panel.SpawnLocation);
    }
    

    IEnumerable<string> EnumerateLines(TMP_Text text)
    {
        // We use GetTextInfo because .textInfo is not always valid
        TMP_TextInfo textInfo = text.GetTextInfo(text.text);
    
        for (int i = 0; i < textInfo.lineCount; i++)
        {
            TMP_LineInfo line = textInfo.lineInfo[i];
            yield return text.text.Substring(line.firstCharacterIndex, line.characterCount);
        }
    }
}