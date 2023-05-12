using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoves : MonoBehaviour
{
    private Vector3 v1,v2;
    private float timer;
    public float speed, speed2, ymin,ymax;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime / speed;
        Vector3 pos1 = new Vector3(0,ymax,0);
        Vector3 pos2 = new Vector3(0,ymin,0);
        
        float whatever =  Mathf.SmoothStep(0,1,Mathf.PingPong(timer,1));
        transform.GetChild(0).localPosition = Vector3.Lerp(pos1,pos2, whatever);

        if(whatever <0.01 || whatever > 0.99f)
        {
            speed = Random.Range(2f,8f);
        }
    }
}
