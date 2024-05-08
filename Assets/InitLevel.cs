using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GetComponent<SaveGame>().LoadPositions(0);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Cursor.visible || Cursor.lockState != CursorLockMode.Locked)
        // {
        //     Cursor.visible = false;
        //     Cursor.lockState = CursorLockMode.Locked;
        // }
    }
}
