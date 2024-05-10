using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    void Start()
    {
        // Hide and lock cursor at the start
        HideAndLockCursor();
    }

    void Update()
    {
        // Continuously enforce cursor state
        HideAndLockCursor();
    }

    private void HideAndLockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


}
