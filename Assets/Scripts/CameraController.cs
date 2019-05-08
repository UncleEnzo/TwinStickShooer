using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform lookAt;
    public float boundX = 1.2f;
    public float boundY = 1.2f;
   // private Player player;
    public float speed = .15f;
    private Vector3 desiredPosition;
    [SerializeField] private int cameraSensitivity;


    //new set of variables
    public Transform player;
    Vector3 target, mousePos, refVel, shakeOffset;
    float cameraDist = 3.5f;
    float smoothTime = 0.2f, zStart;
    float shakeMag, shakeTimeEnd;
    Vector3 shakeVector;
    bool shaking;



    // Start is called before the first frame update
    void Start()
    {
        target = player.position;
        zStart = transform.position.z;

     //   player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        mousePos = CaptureMousePos();
        shakeOffset = UpdateShake();
        target = UpdateTargetPos();
        UpdateCameraPosition();
      
      //  cameraFollowPlayer();
        //moveCameraWithCursor();
    }

    Vector3 CaptureMousePos()
    {
        Vector2 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        ret *= 2;
        ret -= Vector2.one;
        float max = 0.9f;
        if(Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max)
        {
            ret = ret.normalized;
        }
        return ret;
    }

    Vector3 UpdateTargetPos()
    {
        Vector3 mouseOffSet = mousePos * cameraDist;
        Vector3 ret = player.position + mouseOffSet;
        ret += shakeOffset;
        ret.z = zStart;
        return ret;
    }

    void UpdateCameraPosition()
    {
        Vector3 tempPos;
        tempPos = Vector3.SmoothDamp(transform.position, target, ref refVel, smoothTime);
        transform.position = tempPos;
    }

    public void Shake(Vector3 direction, float magnitude, float length)
    {
        shaking = true;
        shakeVector = direction;
        shakeMag = magnitude;
        shakeTimeEnd = Time.time + length;
    }

    Vector3 UpdateShake()
    {
        if (!shaking || Time.time > shakeTimeEnd)
        {
            shaking = false;
            return Vector3.zero;
        }
        Vector3 tempOffSet = shakeVector;
        tempOffSet *= shakeMag;
        return tempOffSet;
    }


    //old stuff

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
