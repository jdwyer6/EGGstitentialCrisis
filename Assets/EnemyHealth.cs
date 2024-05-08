using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 2;
    public Material crackedEggMaterial;
    public GameObject crackedEggModel;  // Prefab to be instantiated when the egg is fully damaged
    public GameObject wholeEggModel;    // Currently active model that will be hidden once the egg is fully cracked

    private Renderer eggRenderer;       // Renderer of the egg object
    private bool isCracked = false;
    public GameObject egg;
    public GameObject legs;

    public float explosionForce = 1000f;
    public float explosionRadius = 5f;

    void Start()
    {
        if (egg != null) {
            eggRenderer = egg.GetComponent<Renderer>();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health == 1 && !isCracked)
        {
            // Change the material to cracked egg
            if (eggRenderer != null) {
                eggRenderer.material = crackedEggMaterial;
            }
            isCracked = true;
        }
        else if (health <= 0)
        {
            GameObject crackedEggInstance = Instantiate(crackedEggModel, wholeEggModel.transform.position, transform.rotation);
    
            // Get the Rigidbody components from the cracked egg instance
            Rigidbody[] rigidbodies = crackedEggInstance.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rb in rigidbodies)
            {
                Vector3 explosionDirection = (rb.position - crackedEggInstance.transform.position).normalized;

                // Apply a force in the calculated direction to simulate an outward explosion
                rb.AddForce(explosionDirection * explosionForce, ForceMode.Impulse);
            }

            if (wholeEggModel != null) {
                wholeEggModel.SetActive(false);
            }
            if (legs != null) {
                legs.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(1);
        }
    }
}
