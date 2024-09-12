using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    Rigidbody myRB;
    Camera playercam;
    Vector2 camRotation;

    public float speed = 10f;
    public float jumpHeight = 8f;
    public Vector3 boxsize;
    public float maxDistance;
    public LayerMask layermask;
    //mouse movement
    public float mouseSensitivity = 2.0f;
    public float xSensitivity = 2.0f;
    public float ySensitivity = 2.0f;
    public float camRotationLimit = 90f;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        playercam = transform.GetChild(0).GetComponent<Camera>();
        Cursor.visible = false;
        camRotation = Vector2.zero;
        Cursor.lockState = CursorLockMode.Confined;
       
    }

    // Update is called once per frame
    void Update()
    {
       // camera movement
        Quaternion mouseLook = playercam.transform.rotation;
        camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        camRotation.y = Mathf.Clamp(camRotation.y, -90, 90);

        playercam.transform.localRotation = Quaternion.AngleAxis(camRotation.y, Vector3.left);
        transform.localRotation = Quaternion.AngleAxis(camRotation.x, Vector3.up);
        // movement movement
        Vector3 temp = myRB.velocity;
        temp.x = Input.GetAxisRaw("Horizontal") * speed;
        temp.z = Input.GetAxisRaw("Vertical") * speed;
        
        {
            if (Input.GetKeyDown(KeyCode.Space))
                temp.y = jumpHeight;
            myRB.velocity = (transform.forward * temp.z) + (transform.right * temp.x) + (transform.up * temp.y);
           
        }
            //attempt to limit jumping
            void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxsize);
        }
            bool GroundCheck()
        {
         if(Physics.BoxCast(transform.position,boxsize,-transform.up,transform.rotation,maxDistance,layermask))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}
