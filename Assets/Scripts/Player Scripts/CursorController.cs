﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Texture2D cursorSprite;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Vector2 cursorHotspot = new Vector2(cursorSprite.width / 2, cursorSprite.height / 2);
        Cursor.SetCursor(cursorSprite, cursorHotspot, CursorMode.Auto);
    }
}