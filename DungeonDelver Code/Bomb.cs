using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour

{
    public float timeToExplodeOffset = 2;
    public float timeToExplodeActual;
    public GameObject explosionObject;
    bool hasExploded = false;
    // Start is called before the first frame update
    void Start()
    {
        timeToExplodeActual = Time.time + timeToExplodeOffset;
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time >= timeToExplodeActual && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }


    void Explode()
    {
        Vector3 newPos = new Vector3(transform.position.x - 1.42f, transform.position.y + 1.59f, transform.position.z);
        Instantiate<GameObject>(explosionObject,newPos,Quaternion.identity);


        Destroy(this.gameObject);
    }

}
