using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayHealthInRestpkace : MonoBehaviour
{
    [SerializeField] private PlayerTracker pl;
    [SerializeField] private TMP_Text  text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "HP: " + pl.Health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
