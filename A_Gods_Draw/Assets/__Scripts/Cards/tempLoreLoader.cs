using UnityEngine;
using TMPro;
public class tempLoreLoader : MonoBehaviour
{
    [SerializeField] TextMeshPro LoreText; 
    [SerializeField] Card_Loader loader;
    // Start is called before the first frame update
    void Start()
    {
        LoreText.text = loader.GetCardSO.description;
        LoreText.ForceMeshUpdate();
    }
}
