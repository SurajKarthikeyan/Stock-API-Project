using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjectGroundScript : MonoBehaviour
{
    public Vector3 lerpVector;
    public Vector3 endLerpVector;
    public ParticleSystem bossAttackRumbleEffect;
    public float distanceToMoveLerp;
    public Transform location;
    public float lerpDuration;
    public float time;
    public float waitForDamage;
    public float particleWaitTime;
    public float particleOffSet;
    public float particleRotation;
    public int damageToPlayer;


    /*
    This is all handled in Start(). It gets the location of the object and uses that for the Lerp
    Then it runs the Destory Coroutine and then destorys itself.
     */

    void Start()
    {

        location = gameObject.transform.parent;

        lerpVector = location.position;


        if (location.rotation.eulerAngles.z == 0) //pointing right
        {
            endLerpVector = new Vector3(lerpVector.x + distanceToMoveLerp, lerpVector.y, lerpVector.z);
            particleOffSet = 3.5f;
            particleRotation = 0;
        }
        else if (location.rotation.eulerAngles.z == 180) //pointing left
        {
            endLerpVector = new Vector3(lerpVector.x - distanceToMoveLerp, lerpVector.y, lerpVector.z);
            particleOffSet = -4f;
            particleRotation = 180;
        }
        else //pointing up
        {
            endLerpVector = new Vector3(lerpVector.x, lerpVector.y + distanceToMoveLerp, lerpVector.z);
            particleOffSet = 3.2f;
        }



        //Moves the attack through a set distance via Lerp.
        StartCoroutine(MoveRoot());

        //Moves the root back and forth and sets a pause time at the end to allow the player to attack it :)
        IEnumerator MoveRoot()
        {

            // Trigger non looping particle effect here:
            // instantiate particle effect
            Instantiate(bossAttackRumbleEffect, new Vector3(transform.position.x+0.2f, transform.position.y + particleOffSet, transform.position.z), new Quaternion(0f, 0f, 45f, 0f));
            yield return new WaitForSeconds(particleWaitTime);
            time = 0;
            while (time < lerpDuration)
            {
                transform.position = Vector3.Lerp(lerpVector, endLerpVector, time / lerpDuration);
                time += Time.deltaTime;
                yield return null;

            }
            transform.position = endLerpVector;
            yield return new WaitForSeconds(waitForDamage);
            time = 0;
            while (time < lerpDuration)
            {
                transform.position = Vector3.Lerp(endLerpVector, location.position, time / lerpDuration);
                time += Time.deltaTime;
                yield return null;

            }
            transform.position = location.position;

            BossAttackGround.UniversalAttackSpawnLoc.Add(location);
            Destroy(gameObject);
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.playerHealth -= damageToPlayer;
        Debug.Log("Current Health is : " + GameManager.playerHealth);
    }
}
