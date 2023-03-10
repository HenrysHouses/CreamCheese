using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageLightController : MonoBehaviour
{

    public bool UpDown;
    private Vector3 origin, velocity;
    [SerializeField]
    private float originPullForce, speedMult;

    private void Start()
    {

        origin = transform.position;

    }

    private void Update()
    {

        
        velocity += (((origin - transform.position).normalized * (Vector3.Distance(origin, transform.position) * originPullForce)) + (new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)))).normalized * (speedMult / (velocity.magnitude + 1f)) * Time.deltaTime;
        transform.position += velocity;

    }

}
