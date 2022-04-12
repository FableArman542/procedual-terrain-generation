using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorVisibility : MonoBehaviour {

    public bool isVisible = true;
    public bool isLocked = true;

    void Start() {
        if (isVisible) Cursor.visible = false;
        if (isLocked) Cursor.lockState = CursorLockMode.Locked;
    }
}
