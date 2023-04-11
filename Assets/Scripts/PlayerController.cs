using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

// credit: help with jump and double jump function from Unity3D School via YouTube
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    // for jumping
    public Vector3 jump;
    public float jumpForce = 2.0f;
    public float groundDistance = 0.5f;
    public bool candoublejump; 

    private Rigidbody rb;
    private int count; // counting the number of items collected !
    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        //controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        SetCountText();
        winTextObject.SetActive(false);
        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText() 
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 10) 
        {
            winTextObject.SetActive(true);
        }
    }

    // helper function for jump
    bool IsGrounded() 
    {
        // seeing if ball is touching ground 
        return Physics.Raycast(transform.position, Vector3.down, groundDistance);
    }

    // this update is just used for jump
    void Update() {
        // if the space bar is hit
        if (Input.GetKeyDown(KeyCode.Space)) {
            // if it was on the ground
            if (IsGrounded()) {
                candoublejump = true;
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            }
            // if it was in the air and can double jump
            else if(candoublejump) 
            {
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                candoublejump = false;
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Pickup")) {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }
}
