using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    public float MovementSpeed;
    public float RunSpeed;
    public float Sensitivity;
    public float JumpPower;
    public float Camera_xOffset;
    public float Camera_yOffset;
    public float CameraRange;
    float DistanceToGround;
    BoxCollider Collider;
    Rigidbody body;
    Camera cam;
    float x, y;
    bool CanJump, CanRun;
    bool ShiftDown, ShiftUp;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();
        DistanceToGround = Collider.bounds.extents.y; 
        cam = Camera.main;
    }
    bool IsGrounded() {
        Vector3 pos = transform.position + -transform.forward * transform.localScale.z;
        Vector3 secPos = transform.position + transform.forward * transform.localScale.z;
        return Physics.Raycast(pos, -Vector3.up, DistanceToGround + 0.1f) | 
        Physics.Raycast(secPos, -Vector3.up, DistanceToGround + 0.1f);
    }

    // Update is called once per frame
    void Jump() {
        CanRun = false;
        cam.fieldOfView = 60f;
        body.AddForce(transform.up * JumpPower);
    }
    void Update() {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
         
        Vector3 pos = transform.position + (-transform.forward * Camera_xOffset) + (transform.up * Camera_yOffset);
        cam.transform.position = pos;
        cam.transform.LookAt(transform.position + transform.forward * CameraRange);

        Cursor.lockState = CursorLockMode.Locked;
        transform.Rotate(new Vector3(0f, Sensitivity * Input.GetAxis("Mouse X"), 0f));

        CanJump = Input.GetKeyDown(KeyCode.Space);
        ShiftDown = Input.GetKeyDown(KeyCode.LeftShift);
        ShiftUp = Input.GetKeyDown(KeyCode.LeftShift);
    }
    void FixedUpdate()
    {
        Vector3 Direction = transform.right * x + transform.forward * y;
        bool Grounded = IsGrounded();
        if ((Input.GetKeyDown(KeyCode.LeftShift) | Input.GetKey(KeyCode.LeftShift)) & Grounded) {
            CanRun = true;
        }else if (Input.GetKeyUp(KeyCode.LeftShift) |!Input.GetKey(KeyCode.LeftShift)) {
            CanRun = false; 
        }
        float fieldOfView = CanRun ? 70f : 60f;
        float LerpSpeed = CanRun ? 0.15f : 0.075f;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fieldOfView, LerpSpeed);
        float Speed = CanRun & Input.GetKey(KeyCode.LeftShift) & Grounded ? RunSpeed : MovementSpeed; 

        body.MovePosition(transform.position + Direction * Speed * Time.deltaTime);
        if (CanJump & Grounded) {CanJump = false; Jump();}
    }
}
