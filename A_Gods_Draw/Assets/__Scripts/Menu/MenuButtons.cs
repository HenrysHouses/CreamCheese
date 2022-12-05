using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public GameObject HTPPanel;
    // Start is called before the first frame update
    void Start()
    {
        HTPPanel.SetActive(false);
    }

    public void ShowHowToPlay()
    {
        HTPPanel.SetActive(true);
    }

    public void HideHowToPlay()
    {
        HTPPanel.SetActive(false);
    }
}
