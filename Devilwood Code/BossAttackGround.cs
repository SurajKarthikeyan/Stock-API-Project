using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossAttackGround : MonoBehaviour
{

    public static List<Transform> UniversalAttackSpawnLoc;
    public List<Transform> BossAttackSpawnLoc;
    public GameObject bossAttackObjectPhase1;
    public GameObject bossAttackObjectPhase2;
    public GameObject bossAttackObjectPhase3;
    public float timeBetweenAttack;
    public float timeBetweenAttackAlpha;
    public float timeBetweenAttackBeta;
    public float timeBetweenAttackCharlie;
    public int phase2;
    public int phase3;
    public bool isSpawning;

    private GameObject bossAttackObject;



    private void Start()
    {
        /*
         Sets the initial attack frequency
         */
        timeBetweenAttack = timeBetweenAttackAlpha;
        bossAttackObject = bossAttackObjectPhase1;

        UniversalAttackSpawnLoc = BossAttackSpawnLoc;
    }

    void Update()
    {
        UniversalAttackSpawnLoc = BossAttackSpawnLoc;

        /*
         Checks if it is spawning or not, if not, start the Spawn() func and change the bool var
         */
        if (!isSpawning && timeBetweenAttack != Mathf.Infinity)
        {
            isSpawning = true;
            Spawn();
        }
        /*
         Changes the attack frequency based on the health of the boss from the GameManager Script
         */

        if (GameManager.bossHealth <= phase3)
        {
            //Trigger in between stage animation
            timeBetweenAttack = timeBetweenAttackCharlie;
            bossAttackObject = bossAttackObjectPhase3;
        }

        else if (GameManager.bossHealth <= phase2)
        {
            //Trigger in between stage animation
            timeBetweenAttack = timeBetweenAttackBeta;
            bossAttackObject = bossAttackObjectPhase2;
        }
    }


    /*
    This function spawns the boss's attacks and their associated sprites. It does this with a given time delay as per the timeBetweenAttack var.
    It uses a corotine to disperse the attacks. There is also a list of locations that can be changed such that the location of where to spawn the attack
    is a wide area and the index is a random value from these indicies.
    */
    public void Spawn()
    {
        int length = BossAttackSpawnLoc.Count;
        if (length > 0)
        {
            int index = Random.Range(0, length);
            StartCoroutine(SpawnObject());
            IEnumerator SpawnObject()
            {
                yield return new WaitForSeconds(timeBetweenAttack);
                Instantiate(bossAttackObject, BossAttackSpawnLoc[index]);
                isSpawning = false;
                BossAttackSpawnLoc.Remove(BossAttackSpawnLoc[index]);
            }

        }
        else
        {
            isSpawning = false;
        }

    }
}
