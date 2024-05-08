using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    private GameObject[] food;
    public float sightAngle = 45f;
    public float sightRange = 10f;


    // Start is called before the first frame update
    void Start()
    {
        food = GameObject.FindGameObjectsWithTag("Food");
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(GetObjectsInSight());
    }

    public GameObject[] GetObjectsInSight()
    {
        List<GameObject> visibleObjects = new List<GameObject>();

        // Get all objects within range of the sight
        Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(transform.position, sightRange);

        // Loop over all objects in range and check if they are within the sight angle
        foreach (Collider2D collider in collidersInRange)
        {
            Vector2 directionToCollider = collider.transform.position - transform.position;
            float angleToCollider = Vector2.Angle(transform.right, directionToCollider);

            if (angleToCollider <= sightAngle / 2f)
            {
                // Object is within the sight angle, add it to the list
                visibleObjects.Add(collider.gameObject);
            }
        }

        return visibleObjects.ToArray();
    }
}
