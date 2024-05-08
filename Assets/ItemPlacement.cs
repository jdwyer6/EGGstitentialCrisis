using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacement : MonoBehaviour
{
    // public GameObject block;
    // public GameObject blockSmall;
    private RaycastInfo raycastInfoScript;
    private AudioManager am;
    private GameObject gm;
    float[] gridSizes = new float[] { 1.5f, 0.5f };
    public GameObject[] uiPlacementHighlighters;
    public int currentGridSizeIdx = 0;

    public GameObject[] blocks;
    public int currentBlockIdx = 0;

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

        if (GetComponent<ModeSelection>().buildEnabled == false)
        {
            foreach (var highlighter in uiPlacementHighlighters)
            {
                highlighter.SetActive(false);
            }
            return;
        } else {
            foreach (var highlighter in uiPlacementHighlighters)
            {
                highlighter.SetActive(false);
            }
            uiPlacementHighlighters[currentBlockIdx].SetActive(true);
            
            Vector3 placementPosition = GetPlacementPosition();
            if (placementPosition != Vector3.zero) 
            {
                uiPlacementHighlighters[currentBlockIdx].transform.position = placementPosition;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (currentBlockIdx >= blocks.Length - 1)
            {
                currentBlockIdx = 0;
            }
            else
            {
                currentBlockIdx++;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentBlockIdx <= 0)
            {
                currentBlockIdx = blocks.Length -1;
            }
            else
            {
                currentBlockIdx--;
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            PlaceItem();
        }

        if (currentBlockIdx >= 0) { // currently all blocks use same grid size. Useful if needing to snap to full size grid
            currentGridSizeIdx = 1;
        } else {
            currentGridSizeIdx = 0;
        }
    }

    private void PlaceItem()
    {
        Debug.Log("Current Grid Size Index: " + gridSizes[currentGridSizeIdx]);

        Vector3 placementPosition = GetPlacementPosition();
        if (placementPosition != Vector3.zero)
        {
            am.Play("Build");
            var newBlock = Instantiate(blocks[currentBlockIdx], placementPosition, Quaternion.identity);
            foreach (Transform child in newBlock.transform)
            {
                child.GetComponent<Renderer>().material = gm.GetComponent<Data>().materials[GetComponent<MaterialSelector>().currentMaterialIdx].material;
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
        float adjustedY = Mathf.Floor(hitPoint.y / gridSizes[currentGridSizeIdx]) * gridSizes[currentGridSizeIdx];

        if (adjustedY < 0)
        {
            adjustedY = 0;
        }
        
        // Initial grid-aligned position calculation
        Vector3 gridAlignedPosition = new Vector3(
            Mathf.Floor(hitPoint.x / gridSizes[currentGridSizeIdx]) * gridSizes[currentGridSizeIdx] + gridSizes[currentGridSizeIdx] / 2,
            adjustedY,
            Mathf.Floor(hitPoint.z / gridSizes[currentGridSizeIdx]) * gridSizes[currentGridSizeIdx] + gridSizes[currentGridSizeIdx] / 2
        );

        // Detecting a vertical surface using the y component of the normal
        if (Mathf.Abs(hitNormal.y) < 0.1f)
        {
            // Calculate the direction from the hit point towards the player, ignoring the y component
            Vector3 directionTowardsPlayer = (transform.position - hitPoint).normalized;
            Vector3 horizontalDirectionTowardsPlayer = new Vector3(directionTowardsPlayer.x, 0, directionTowardsPlayer.z).normalized;

            gridAlignedPosition = hitPoint + horizontalDirectionTowardsPlayer * gridSizes[currentGridSizeIdx] / 2; // Adjust the addition to half the grid size

            // Snap the new position to the grid
            gridAlignedPosition = new Vector3(
                Mathf.Floor(gridAlignedPosition.x / gridSizes[currentGridSizeIdx]) * gridSizes[currentGridSizeIdx] + gridSizes[currentGridSizeIdx] / 2,
                adjustedY, // Y remains as calculated before
                Mathf.Floor(gridAlignedPosition.z / gridSizes[currentGridSizeIdx]) * gridSizes[currentGridSizeIdx] + gridSizes[currentGridSizeIdx] / 2
            );
        }

        return gridAlignedPosition;
    }


}
