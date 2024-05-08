using UnityEngine;

public class Sculpt : MonoBehaviour
{
    private RaycastInfo raycastInfoScript;
    private GameObject lastHighlighted = null;
    private AudioManager am;
    public GameObject dustParticles;
    public GameObject sculptParticles;


    void Start()
    {
        raycastInfoScript = GetComponent<RaycastInfo>();
        am = FindObjectOfType<AudioManager>();

        if (raycastInfoScript == null)
        {
            Debug.LogError("RaycastInfo script not found on the GameObject.");
        }
    }

    void Update()
    {
        if (GetComponent<ModeSelection>().sculptEnabled == false) {
            return;
        }

        GameObject currentTarget = raycastInfoScript?.currentTarget;

        if (currentTarget != lastHighlighted)
        {
           // If there was a last highlighted object, remove its highlight
            if (lastHighlighted != null && lastHighlighted.CompareTag("Sculptable"))
            {
                RemoveHighlight(lastHighlighted);
            }

            // If the current target is sculptable, highlight it
            if (currentTarget != null && currentTarget.CompareTag("Sculptable"))
            {
                HighlightObject(currentTarget);
            }

            // Update the last highlighted object
            lastHighlighted = currentTarget;
        }

        if (Input.GetMouseButtonDown(0) && currentTarget != null && currentTarget.CompareTag("Sculptable"))
        {
            am.Play("Sculpt");
            Instantiate(dustParticles, currentTarget.transform.position, Quaternion.identity);
            Instantiate(sculptParticles, currentTarget.transform.position, Quaternion.identity);
            Destroy(currentTarget);
            lastHighlighted = null; // Reset last highlighted since we destroyed the object
        }
    }

    void HighlightObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.blue;
        }
    }

    void RemoveHighlight(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white; // Or revert to the object's original color
        }
    }
}
