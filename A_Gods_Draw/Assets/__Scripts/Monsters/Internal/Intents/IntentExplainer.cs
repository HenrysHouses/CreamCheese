// Written by Javier

using UnityEngine;

public class IntentExplainer : MonoBehaviour
{
    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);


    }

    // Update is called once per frame

    private void OnMouseOver()
    {
        panel.SetActive(true);
    }

    private void OnMouseExit()
    {
        panel.SetActive(false);
        
    }
}
