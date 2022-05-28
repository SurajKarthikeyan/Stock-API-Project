using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float timeDeleteOffSet = 0.125f;
    public float timeDeleteActual = 0;
    void Start()
    {
        timeDeleteActual = timeDeleteOffSet + Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= timeDeleteActual)
        {
            Destroy(this.gameObject);
        }
    }
}
