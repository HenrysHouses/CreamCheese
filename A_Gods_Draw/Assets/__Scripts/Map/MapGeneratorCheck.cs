using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
public class MapGeneratorCheck : MonoBehaviour
{
    public GameManager GM;
    public Map_Manager MM;
    
    // Start is called before the first frame update
    void Start()
    {
        MM = GameObject.Find("Map_Manager").GetComponent<Map_Manager>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GM.startedNewGame)
        {
            Debug.Log("Generates a new map");
            MM.GenerateNewMap();
           GM.startedNewGame = false;
        }
        
    }
}
