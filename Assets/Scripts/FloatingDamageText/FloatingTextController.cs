using UnityEngine;
using System.Collections;

public class FloatingTextController : MonoBehaviour
{
    private static FloatingText popupText;
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
        if (!popupText)
        {
            popupText = Resources.Load<FloatingText>("Prefabs/PopupTextParent");
        }
    }

    public static FloatingText CreateFloatingText(string text, Transform location)
    {
        FloatingText instance = Instantiate(popupText);
        // Vector2 screenPosition = Camera.main.WorldToScreenPoint();

        instance.transform.SetParent(canvas.transform, false);
        instance.SetText(text);
        return instance;
    }
    public static void SetFloatingTextLocation(FloatingText instance, Transform location)
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + instance.randomPosMovementX, location.position.y + .7f + instance.randomPosMovementY));
        instance.transform.position = screenPosition;
    }
}
