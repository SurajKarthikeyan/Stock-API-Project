using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///This enum is used to switch between the states. Found it easier than using a bool
public enum eWeapon { sword, hammer, specialHammer };
public class SwordController : MonoBehaviour
{
    ///Used two gameObjects because in Unity I simply duplicated the sword object
    ///The 'S' and 'H' stand for 'sword' and 'hammer'
    private GameObject weaponS;
    private GameObject weaponH;
    public GameObject shockWave;
    public bool hasInstantiatedThree = false;
    private Dray dray;
    ///Instantiated an instance of the enum here
    public static eWeapon eWep = eWeapon.sword;

    void Start()
    {
        // Find the Hammer child of SwordController
        Transform hammerT = transform.Find("Hammer");
        if (hammerT == null)
        {
            Debug.LogError("Could not find Hammer child of SwordController."); 
            return;
        }
        weaponH = hammerT.gameObject;

        //Deactivate the hammer
        weaponH.SetActive(false);



        // Find the Sword child of SwordController
        Transform swordT = transform.Find("Sword");
        if (swordT == null)
        {
            Debug.LogError("Could not find Sword child of SwordController.");
            return;
        }
        weaponS = swordT.gameObject;

        // Find the Dray component on the parent of SwordController
        dray = GetComponentInParent<Dray>();
        if (dray == null)
        {
            Debug.LogError("Could not find parent component Dray.");
            return;
        }

        // Deactivate the weapon
        weaponS.SetActive(false);
    }

    void Update()
    {
        ///Check at the start if the weapon is in "specialAttack" mode
        ///If so, and its in hammer base mode, switch it back to hammer base mode
        if (eWep == eWeapon.specialHammer && Time.time > Dray.timeSpecAtkDone)
        {
            Dray.timeSpecAtkDone = Time.time + 2 * Dray.attackDuration;
            Dray.timeSpecAtkNext = Time.time + 2 * Dray.attackDelay;
            eWep = eWeapon.hammer;
            hasInstantiatedThree = false;
            
        }

        ///Switch Weapons
        if (Input.GetKeyDown(KeyCode.R))
        {

            if (eWep != eWeapon.sword)
            {
                eWep = eWeapon.sword;
            }
            else if(eWep != eWeapon.hammer)
            {
                eWep = eWeapon.hammer;
            }
        }

        //If in hammer, and pressed Q, set to special hammer
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(eWep == eWeapon.hammer) eWep = eWeapon.specialHammer;
        }

       
        
        if (eWep == eWeapon.sword)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90 * dray.facing); // set rotation of object
            weaponS.SetActive(dray.mode == Dray.eMode.attack);
        }
        else if(eWep == eWeapon.hammer)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90 * dray.facing);
            weaponH.SetActive(dray.mode == Dray.eMode.attack);
        }
        else if(eWep == eWeapon.specialHammer )
        {
            transform.rotation = Quaternion.Euler(0, 0, 90 * dray.facing);
            weaponH.SetActive(dray.mode == Dray.eMode.specialAttack);

            ///This is bad and jank but it works. It spawns the shockwaves in the
            ///right direction and in good spacing. If it works it works. And it works
            switch (dray.facing)
            {
                case 0:
                    InstantiateShockWave(1,0);
                    break;
                case 1:
                    InstantiateShockWave(0,1);
                    break;
                case 2:
                    InstantiateShockWave(-1, 0);
                    break;
                case 3:
                    InstantiateShockWave(0, -1);
                    break;

            }

        }



    }

    /// <summary>
    /// Having problems with offset instantiations of the sprite prefab
    /// Used this function to instantiate it correctly along with the proper distance from the player
    /// Consieders how many it has intantiated
    /// </summary>
    /// <param name="xOf"></param>
    /// <param name="yOf"></param>
    public void InstantiateShockWave(int xOf, int yOf)
    {
        shockWave.transform.position = new Vector3(dray.transform.position.x + xOf, dray.transform.position.y + yOf, 0);
        bool zeroStatus = CheckNoneZero(xOf, yOf);
        bool posStatus = CheckPosOrNeg(xOf, yOf, zeroStatus);
        if (!hasInstantiatedThree)
        {
            if (zeroStatus && posStatus) //This means y != 0 and y>1
            {
                for (int i = 0; i < 3; i++)
                {
                    shockWave.transform.rotation = Quaternion.Euler(0, 0, 90 * dray.facing);
                    shockWave.transform.position = new Vector3(dray.transform.position.x + xOf -0.4f, dray.transform.position.y + yOf, 0);
                    Instantiate<GameObject>(shockWave);
                    yOf++;
                }
            }
            else if (zeroStatus && !posStatus) // y!= 0 and y<1
            {
                for (int i = 0; i < 3; i++)
                {
                    shockWave.transform.rotation = Quaternion.Euler(0, 0, 90* dray.facing);
                    shockWave.transform.position = new Vector3(dray.transform.position.x + xOf + 0.4f, dray.transform.position.y + yOf, 0);
                    Instantiate<GameObject>(shockWave);
                    yOf--;
                }
            }
            else if (!zeroStatus && posStatus) // x != 0 and x>1
            {
                for (int i = 0; i < 3; i++)
                {
                    shockWave.transform.rotation = Quaternion.Euler(0, 0, 90* dray.facing);
                    shockWave.transform.position = new Vector3(dray.transform.position.x + xOf, dray.transform.position.y + yOf + 0.4f, 0);
                    Instantiate<GameObject>(shockWave);
                    xOf++;
                }
            }
            else // x != and x<1
            {
                for (int i = 0; i < 3; i++)
                {
                    shockWave.transform.rotation = Quaternion.Euler(0, 0, 90 * dray.facing);
                    shockWave.transform.position = new Vector3(dray.transform.position.x + xOf, dray.transform.position.y + yOf - 0.4f, 0);
                    Instantiate<GameObject>(shockWave);
                    xOf--;
                }
            }
            hasInstantiatedThree = true;
        }

    }

    /// <summary>
    /// Checks if the x offset is zero. If it is, then Y must be the one changing the location, else it must be X
    /// </summary>
    /// <param name="xOf"></param>
    /// <param name="yOf"></param>
    /// <returns></returns>
    public bool CheckNoneZero(int xOf, int yOf)
    {
        return (xOf == 0 ? true : false);
    }

    /// <summary>
    /// Given the previous function, we know what is non zero. Now we must check if its pos or neg as it tells us where to place it
    /// </summary>
    /// <param name="xOf"></param>
    /// <param name="yOf"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public bool CheckPosOrNeg(int xOf, int yOf, bool status)
    {
        if (status)
        {
            return (yOf > 0 ? true : false);
        }
        else
        {
            return (xOf > 0 ? true : false);
        }
    }
}
