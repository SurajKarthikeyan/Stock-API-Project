using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveScript : MonoBehaviour
{
    public float deathTimer;
    void Start()
    {
        float lifeTimer = Time.time;
        deathTimer = lifeTimer + .55f;
    }
    void Update()
    {
        if(deathTimer <= Time.time)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collision.gameObject.layer == LayerMask.NameToLayer("BombTiles"))
        {
            Destroy(gameObject);
        }
    }
}
