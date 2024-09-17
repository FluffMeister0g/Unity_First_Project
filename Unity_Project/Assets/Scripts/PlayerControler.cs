using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    Rigidbody myRB;
    Camera playercam;
    Vector2 camRotation;


    [Header("Movement Stats")]
    public float sprintmult = 1.5f;
    public bool sprinting = false;
    public float groundDetection = 1f;
    public float speed = 10f;
    public float jumpHeight = 8f;
    [Header("User Settings")]
    public bool sprintToggle = false;
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
        if (!sprinting && !sprintToggle && Input.GetKey(KeyCode.LeftShift))
            sprinting = true;
        if (!sprinting && sprintToggle && (Input.GetAxisRaw("Vertical") > 0) && Input.GetKey(KeyCode.LeftShift))
                sprinting = true;
        
        // movement movement
        Vector3 temp = myRB.velocity;
        temp.x = Input.GetAxisRaw("Horizontal") * speed;
        temp.z = Input.GetAxisRaw("Vertical") * speed;

            if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, -transform.up, groundDetection))
                temp.y = jumpHeight;

        //sprinting
        if (sprinting)
            temp.z *= sprintmult;
        if (sprinting && !sprintToggle && Input.GetKeyUp(KeyCode.LeftShift))
            sprinting = false;
        if (sprinting && sprintToggle && (Input.GetAxisRaw("Vertical") <= 0))
            sprinting = false;

        myRB.velocity = (transform.forward * temp.z) + (transform.right * temp.x) + (transform.up * temp.y);

        
    }
}
