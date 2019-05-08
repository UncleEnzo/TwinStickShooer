using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform lookAt;
    public float boundX = 1.2f;
    public float boundY = 1.2f;
    private Player player;
    public float speed = .15f;
    private Vector3 desiredPosition;
    [SerializeField] private int cameraSensitivity;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void LateUpdate()
    {
      
        cameraFollowPlayer();
        //moveCameraWithCursor();
    }

    private void moveCameraWithCursor()
    {
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float xMove = (transform.position.x + cursorPosition.x) / cameraSensitivity;
        float yMove = (transform.position.y + cursorPosition.y) / cameraSensitivity;
        transform.position = new Vector3(xMove, yMove, 0f);
    }

    private void cameraFollowPlayer()
    {
        Vector3 delta = Vector3.zero;
        float dx = lookAt.transform.position.x - transform.position.x;
        float dy = lookAt.transform.position.y - transform.position.y;

        if (dx > boundX || dx < -boundX)
        {
            if (transform.position.x < lookAt.transform.position.x)
            {
                delta.x = dx - boundX;
            }
            else
            {
                delta.x = dx + boundX;
            }
        }
        if (dy > boundX || dy < -boundY)
        {
            if (transform.position.y < lookAt.transform.position.y)
            {
                delta.y = dy - boundY;
            }
            else
            {
                delta.y = dy + boundX;
            }
        }
        desiredPosition = transform.position += delta;
        transform.position = Vector3.Lerp(transform.position,desiredPosition, speed);

    }

  
}
