using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    public GameObject[] lava;
    public GameObject lavaplain, lokiFire;
    public Transform lavaplainpos1, lavaplainpos2;
    public float lavaplainSpeed;
    public bool turnOnLava = false, testing, fireOn;
    public HealthCounterController playerHealth;
    public int healtLavaShouldSpawn;
    // Start is called before the first frame update
    void Start()
    {
        lokiFire.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("player current health is" + playerHealth.currHealth);
        if (!testing)
        {

            if (playerHealth.currHealth < healtLavaShouldSpawn)
                turnOnLava = true;

            if (playerHealth.currHealth > healtLavaShouldSpawn)
                turnOnLava = false;

        }


        if (turnOnLava)
        {
            LavaOn();
            if(fireOn)
            {
                lokiFire.SetActive(true);

            }
            lavaplain.SetActive(true);
            float step = lavaplainSpeed * Time.deltaTime; 
            lavaplain.transform.position = Vector3.MoveTowards(lavaplain.transform.position, lavaplainpos2.position, step);

        }
        else
        {
            foreach (GameObject lavas in lava)
            {
                lavas.SetActive(false);
                lokiFire.SetActive(false);
            }
            float step = lavaplainSpeed * Time.deltaTime;
            lavaplain.transform.position = Vector3.MoveTowards(lavaplain.transform.position, lavaplainpos1.position, step * 3);

        }
    }

    public void LavaOn()
    {

        foreach (GameObject lavas in lava)
        {
            lavas.SetActive(true);
        }
    }
}
