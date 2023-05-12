using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDeadEnemies : MonoBehaviour
{
    [SerializeField]
    private GameObject deadEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.K))
        {
            SpawndeadEnemy();
        }
       
    }

    private void SpawndeadEnemy()
    {
        GameObject.Instantiate(deadEnemy,transform.position,Quaternion.identity);
    }
}
