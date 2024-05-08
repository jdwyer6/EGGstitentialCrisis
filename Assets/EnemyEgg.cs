using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEgg : MonoBehaviour
{
    public EnemyHealth enemyHealth;

    // Start is called before the first frame update
    void Start()
    {

        if (enemyHealth == null)
        {
            Debug.LogWarning("EnemyHealth component not found on " + gameObject.name);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the tag 'Projectile'
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // Call TakeDamage on the EnemyHealth script
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1);
            }
        }
    }
}
