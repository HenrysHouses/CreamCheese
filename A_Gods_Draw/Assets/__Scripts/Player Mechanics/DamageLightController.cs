using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageLightController : MonoBehaviour
{

    public bool UpDown;
    private Vector3 origin, velocity;
    [SerializeField]
    private float originPullForce, speedMult, maxDistance;

    private void Start()
    {

        origin = transform.position;

    }

    private void Update()
    {

        if(Vector3.Distance(transform.position, origin) < maxDistance)
            velocity += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * speedMult * Time.deltaTime;
        else
            velocity = (origin - transform.position).normalized * originPullForce;
        transform.position += velocity;

    }

}
