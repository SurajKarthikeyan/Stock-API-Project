using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth ;
    private int currentHealth;
    public Transform location;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        location = gameObject.transform.parent;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //hurt particle effect?
        GameManager.bossHealth -= damage;
        Die();
    }
    void Die()
    {
        //death particle effect
        BossAttack.UniversalAttackSpawnLoc.Add(location);
        Destroy(gameObject);
    }
}
