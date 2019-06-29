using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    #region Singleton
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one Instance of Inventory found.");
        }
        Instance = this;
    }
    #endregion
    public static CursorController Instance;
    public Texture2D cursorSprite;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Vector2 cursorHotspot = new Vector2((cursorSprite.width / 2), (cursorSprite.height / 2));
        Cursor.SetCursor(cursorSprite, cursorHotspot, CursorMode.Auto);
    }
}
