using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float xRot = 0, yRot = 0;
    private Vector3 moveDirection;
    private float movementMultiplier = 10;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 9;
    [SerializeField] private float midAirMoveSpeed = 0.5f;

    [Header("Sprint")]
    [SerializeField] private float walkSpeed = 7;
    [SerializeField] private float sprintSpeed = 10;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private float acceleration = 10;

    [Header("Jump")]
    [SerializeField] private float jumpVelocity = 6;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private bool isJump;
    [SerializeField] private bool isGrounded;
    [SerializeField] private LayerMask groundedLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private CapsuleCollider cc;

    [Header("Drag")]
    [SerializeField] private float groundDrag = 6;
    [SerializeField] private float airDrag = 0.1f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform orientation;

    private Rigidbody rbController;

    private void Awake() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rbController = GetComponent<Rigidbody>();
        rbController.freezeRotation = true;
    }

    private void Update()
    {
        isJump = isGrounded ? Input.GetKeyDown(jumpKey) : false;
        Rotate();
        if (isJump && isGrounded)
        {
            Jump();
        }
        Move();
    }
    private bool IsGrounded
    {
        get => Physics.CheckSphere(groundCheck.position, cc.radius + 0.01f, groundedLayer);
    }
    private void Rotate()
    {
        xRot += Input.GetAxis("Mouse X") * Manager.Instance.MouseSensitivity;
        yRot -= Input.GetAxis("Mouse Y") * Manager.Instance.MouseSensitivity;
        orientation.rotation = Quaternion.Euler(0, xRot, 0);
        cameraTransform.localRotation = Quaternion.Euler(yRot, xRot, 0);
    }

    private void Move()
    {
        float ver = Input.GetAxisRaw("Vertical");
        float hor = Input.GetAxisRaw("Horizontal");

        isGrounded = IsGrounded;

        moveSpeed = isGrounded ? 
            moveSpeed
            : midAirMoveSpeed;

        if(isGrounded) {
            rbController.velocity = new Vector3(rbController.velocity.x, 0, rbController.velocity.z);
            SpeedControl();
        }
        moveDirection = orientation.forward * ver + orientation.right * hor;
        moveDirection = moveDirection.normalized * movementMultiplier * moveSpeed;

        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, cc.height / (4/3), groundedLayer)){
            moveDirection = Vector3.ProjectOnPlane(moveDirection, hit.normal);
            /*float angle = Vector3.Angle(hit.normal, Vector3.up);
            moveDirection *= (90 - Mathf.Abs(angle)) / 90; */
        }
        rbController.AddForce(moveDirection, ForceMode.Acceleration);
        ControlDrag();
    }

    private void SpeedControl()
    {
        moveSpeed = Mathf.LerpUnclamped(
            moveSpeed,
            (Input.GetKey(sprintKey) ?
                sprintSpeed
                : walkSpeed),
            acceleration * Time.deltaTime);
    }

    private void Jump(){
        rbController.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
    }

/*     private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, moveDirection);
    } */

    private void ControlDrag()
    {
        rbController.drag = isGrounded ? groundDrag : airDrag;
    }
}
