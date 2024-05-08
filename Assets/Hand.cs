using UnityEngine;

public class Hand : MonoBehaviour
{
    private GameObject currentItem = null;
    public Transform holdLocation; // Assign this in the inspector to the transform where picked items should be held
    private GameObject currentItemInHand;
    public float throwItemForce = 10;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // If currently holding an item, drop it
            if (currentItem != null)
            {
                DropItem();
            }
            else
            {
                // Attempt to pick up a new item
                TryPickUpItem();
            }
        }
    }

    void TryPickUpItem()
    {
        RaycastInfo raycastInfo = GetComponent<RaycastInfo>();
        if (raycastInfo != null && raycastInfo.hitInfo.collider != null)
        {
            ItemData itemData = raycastInfo.hitInfo.collider.GetComponent<ItemData>();
            // Check if the target has ItemData and can be picked up
            if (itemData != null && itemData.canBePickedUp)
            {
                PickupItem(raycastInfo.hitInfo.collider.gameObject);
            }
        }
    }

    void PickupItem(GameObject item)
    {
        currentItem = item;
        item.GetComponent<ItemData>().isBeingHeld = true;

        if (item.GetComponent<Rigidbody>() != null)
        {
            item.GetComponent<Rigidbody>().isKinematic = true;
            item.GetComponent<Collider>().enabled = false;
        }
        item.transform.SetParent(holdLocation);
        item.transform.localPosition = Vector3.zero; 
    }

    void DropItem()
    {
        currentItem.GetComponent<ItemData>().isBeingHeld = false;
        currentItem.GetComponent<Collider>().enabled = true;
        if (currentItem.GetComponent<Rigidbody>() != null)
        {
            currentItem.GetComponent<Rigidbody>().isKinematic = false;
            currentItem.GetComponent<Rigidbody>().AddForce(transform.forward * throwItemForce, ForceMode.Impulse);
        }
        currentItem.transform.SetParent(null); 
        currentItem = null;
    }
}
