using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastInfo : MonoBehaviour
{
    public GameObject currentTarget;
    public Camera mainCamera;
    public RaycastHit hitInfo;

    void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found in the scene.");
        }
    }

    void Update()
    {
        getCurrentTarget();
    }

    void getCurrentTarget()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        
        if (Physics.Raycast(ray, out hit)) 
        {
            GameObject hitObject = hit.collider.gameObject; 
            currentTarget = hit.collider.gameObject; 
            hitInfo = hit; 
        } else {
            currentTarget = null;
        }
    }
}
