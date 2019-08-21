using UnityEngine;
using System.Collections;

public class FloatingTextController : MonoBehaviour
{
    private static GameObject popupTextParentGO;
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
        if (!popupTextParentGO)
        {
            popupTextParentGO = Resources.Load<GameObject>("Prefabs/PopupTextParent");
        }
    }

    public static FloatingText CreateFloatingText(string text, Transform location)
    {
        GameObject instanceGO = ObjectPooler.SharedInstance.GetPooledObject(popupTextParentGO.name + "(Clone)");
        if (instanceGO != null)
        {
            instanceGO.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Could not find popup text in object pooler");
        }
        FloatingText instanceText = instanceGO.GetComponent<FloatingText>();
        instanceText.transform.SetParent(canvas.transform, false);
        instanceText.SetText(text);
        return instanceText;
    }
    public static void SetFloatingTextLocation(FloatingText instance, Transform location)
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + instance.randomPosMovementX, location.position.y + .7f + instance.randomPosMovementY));
        instance.transform.position = screenPosition;
    }
}
