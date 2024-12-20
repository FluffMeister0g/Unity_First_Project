using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerControler : MonoBehaviour
{
    Rigidbody myRB;
    Camera playercam;
    Vector2 camRotation;
    Transform cameraHolder;

    [Header("Player Stats")]
    public bool takenDamage = false;
    public float damageCooldownTimer = 1f;
    public int health = 10;
    public int maxHealth = 10;
    public int healthPickupAmt = 5;
    [Header("Weapon Stats")]
    public AudioSource weaponSpeaker;
    public Transform weaponSlot;
    public bool canFire = true;
    public float fireRate = 0;
    public float shotVel = 0;
    public float currentClip = 0;
    public GameObject shot;
    public int weaponID = -1;
    public int fireMode = 0;
    public float clipSize = 0;
    public float maxAmmo = 0;
    public float currentAmmo = 0;
    public float reloadAmt = 0;
    public float bulletLifespan = 0;

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
        playercam = Camera.main;
        cameraHolder = transform.GetChild(0);

        
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
        playercam.transform.position = cameraHolder.position;

        if (Input.GetMouseButton(0) && canFire && currentClip > 0 && weaponID >= 0)
        {
            weaponSpeaker.Play();
            GameObject s = Instantiate(shot, weaponSlot.position, weaponSlot.rotation);
            s.GetComponent<Rigidbody>().AddForce(playercam.transform.forward * shotVel);
            Destroy(s, bulletLifespan);

            canFire = false;
            currentClip--;
            StartCoroutine("cooldownFire");
        }
        if (Input.GetKeyDown(KeyCode.R))
            reloadClip();

        playercam.transform.rotation = Quaternion.Euler(-camRotation.y, camRotation.x, 0);
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
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Pickup") && health < maxHealth)
        {
            if (health + healthPickupAmt > maxHealth)
                health = maxHealth;
            else health += healthPickupAmt;
            Destroy(collision.gameObject);
        }
    
        if ((collision.gameObject.tag == "Pickup") && currentAmmo < maxAmmo)
        {
            if (currentAmmo + reloadAmt > maxAmmo)
                currentAmmo = maxAmmo;
            else currentAmmo += reloadAmt;
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            other.transform.position = weaponSlot.position;
            other.transform.rotation = weaponSlot.rotation;
            other.transform.SetParent(weaponSlot);
            weaponSpeaker = other.gameObject.GetComponent<AudioSource>();
            switch(other.gameObject.name)
            {
                case "Weapon1":
                    weaponID = 0;
                    shotVel = 10000;
                    fireMode = 0;
                    fireRate = 0.5f;
                    currentClip = 20;
                    currentAmmo = 40;
                    reloadAmt = 20;
                    bulletLifespan = .5f;
                    break;

                default:
                    break;
            }
        }
        if (other.gameObject.tag == "Weapon1")
        {
            other.transform.position = weaponSlot.position;
            other.transform.rotation = weaponSlot.rotation;
            other.transform.SetParent(weaponSlot);

            switch (other.gameObject.name)
            {
                case "Weapon2":
                    weaponID = 1;
                    shotVel = 10000;
                    fireMode = 0;
                    fireRate = 0.00000000000001f;
                    currentClip = 20;
                    currentAmmo = 80;
                    reloadAmt = 50;
                    bulletLifespan = 1f;
                    break;

                default:
                    break;
            }
        }
    }
    public void reloadClip()
    {
        if (currentClip >= clipSize)
            return;
        else
        {
            float reloadCount = clipSize - currentClip;
            if (currentAmmo < reloadCount)
            {
                currentClip += currentAmmo;
                currentAmmo = 0;
            }
            else
            {
                currentClip += reloadCount;
                currentAmmo -= reloadCount;
                return;
            }
        }
    }
    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }
    IEnumerator cooldownDamage()
    {
        yield return new WaitForSeconds(damageCooldownTimer);
        takenDamage = false;
    }
}

