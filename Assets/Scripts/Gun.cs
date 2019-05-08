using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //Configurable Values
   [Range(0.1f,3f)] [SerializeField] private float armLength;

    //cached values
    private Transform shoulder;
    private Vector3 point;
 


    //Weapon Fire
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate = 0.5F;
    private float nextFire = 0.5F;

    //Recoil camera shake
    private CameraController cam;

    void Start()
    {
        // if the sword is child object, this is the transform of the character (or shoulder)
        shoulder = transform.parent.transform;

        //instantiate the camera controller
        cam = FindObjectOfType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rotateAroundShoulder(point);
        lookAtCursor(point);
        fireGun();
    }

    private void fireGun()
    {
        //Fire shot at specified rate
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            cam.Shake((transform.parent.transform.position - transform.position).normalized ,1.5f, 0.05f);
        }
    }

    private void rotateAroundShoulder(Vector3 point)
    {
        // Get the direction between the shoulder and mouse (aka the target position)
        Vector3 shoulderToMouseDir = point - shoulder.position;
        shoulderToMouseDir.z = 0; // zero z axis since we are using 2d
                                  // we normalize the new direction so you can make it the arm's length
                                  // then we add it to the shoulder's position
        transform.position = shoulder.position + (armLength * shoulderToMouseDir.normalized);
    }

    private void lookAtCursor(Vector3 point)
    {
        var dir = point - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
