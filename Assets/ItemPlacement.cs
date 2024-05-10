using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacement : MonoBehaviour
{
    private RaycastInfo raycastInfoScript;
    private AudioManager am;
    private GameObject gm;
    public GameObject[] uiPlacementHighlighters;
    public float gridSize = .5f;

    public GameObject[] blocks;
    public int currentBlockIdx = 0;

    private Quaternion currentRotation = Quaternion.identity; // Added to keep track of the current rotation

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("gm");
        raycastInfoScript = GetComponent<RaycastInfo>();
        am = FindObjectOfType<AudioManager>();

        if (raycastInfoScript == null)
        {
            Debug.LogError("RaycastInfo script not found on the GameObject.");
        }
    }

        void Update()
    {
        if(MenuManager.gamePaused)
        {
            return;
        }

        if (!GetComponent<ModeSelection>().buildEnabled)
        {
            foreach (var highlighter in uiPlacementHighlighters)
            {
                highlighter.SetActive(false);
            }
            return;
        }

        // Deactivate all highlighters first
        foreach (var highlighter in uiPlacementHighlighters)
        {
            highlighter.SetActive(false);
        }
        
        // Activate only the current highlighter
        uiPlacementHighlighters[currentBlockIdx].SetActive(true);
        
        Vector3 placementPosition = GetPlacementPosition();
        if (placementPosition != Vector3.zero) 
        {
            uiPlacementHighlighters[currentBlockIdx].transform.position = placementPosition;
            // Update rotation for each child of the UI indicator
            foreach (Transform child in uiPlacementHighlighters[currentBlockIdx].transform)
            {
                child.localRotation = currentRotation; // Apply current rotation to UI indicator's child only
            }
        }

        HandleRotation();

        if (Input.GetKeyDown(KeyCode.C))
        {
            currentBlockIdx = (currentBlockIdx + 1) % blocks.Length;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            currentBlockIdx = (currentBlockIdx - 1 + blocks.Length) % blocks.Length;
        }

        if (Input.GetMouseButtonDown(0))
        {
            PlaceItem();
        }
    }

    private void HandleRotation()
    {
        float rotationStep = 90.0f;
        if (Input.mouseScrollDelta.y != 0)
        {
            if (Input.GetMouseButton(1)) // Right mouse button held down
            {
                // Rotate around Z axis
                currentRotation *= Quaternion.Euler(0, 0, Input.mouseScrollDelta.y * rotationStep);
            }
            else
            {
                // Rotate around Y axis
                currentRotation *= Quaternion.Euler(0, Input.mouseScrollDelta.y * rotationStep, 0);
            }
        }
    }

    private void PlaceItem()
    {
        Debug.Log("Current Grid Size Index: " + gridSize);

        Vector3 placementPosition = GetPlacementPosition();
        if (placementPosition != Vector3.zero)
        {
            am.Play("Build");
            var newBlock = Instantiate(blocks[currentBlockIdx], placementPosition, Quaternion.identity); // Keep parent's rotation unchanged
            foreach (Transform child in newBlock.transform)
            {
                child.GetComponent<Renderer>().material = gm.GetComponent<Data>().materials[GetComponent<MaterialSelector>().currentMaterialIdx].material;
                child.localRotation = currentRotation; // Only rotate the child
            }
        }
    }


    private Vector3 GetPlacementPosition()
    {
        if (raycastInfoScript.hitInfo.point == Vector3.zero || raycastInfoScript.hitInfo.normal == Vector3.zero)
        {
            return Vector3.zero; // Return a zero vector if no valid hit point or normal
        }

        Vector3 hitPoint = raycastInfoScript.hitInfo.point;
        Vector3 hitNormal = raycastInfoScript.hitInfo.normal.normalized;

        // Offset the hit point by the normal scaled by the desired distance
        Vector3 offsetPoint = hitPoint + hitNormal * 0.25f; // Adjust by 0.25 units in the direction of the normal

        // Snap the offset point to the nearest grid point considering the gridSize
        Vector3 gridAlignedPosition = new Vector3(
            Mathf.Floor((offsetPoint.x + (gridSize / 2)) / gridSize) * gridSize,
            Mathf.Floor((offsetPoint.y + (gridSize / 2)) / gridSize) * gridSize,
            Mathf.Floor((offsetPoint.z + (gridSize / 2)) / gridSize) * gridSize
        );

        return gridAlignedPosition;
    }

}
