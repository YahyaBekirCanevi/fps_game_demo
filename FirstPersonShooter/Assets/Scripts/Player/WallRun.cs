using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform orientation;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    
    [Header("Detection")]
    [SerializeField] private float wallDistance = .5f;
    [SerializeField] private float minimumJumpHeight = 1.5f;
    
    [Header("Wall Running")]
    [SerializeField] private float wallRunGravity = 1.5f;

    private Rigidbody rbController;

    private bool wallLeft = false;
    private bool wallRight = false;

    private RaycastHit leftRayHit, rightRayHit;
    private void Awake() {
        rbController = GetComponent<Rigidbody>();
    }
    private void Update() {
        CheckWall();
        if(CanWallRun()){
            if (wallLeft || wallRight) StartWallRun();
            else StopWallRun();
        }
        else StopWallRun();
    }

    private void StartWallRun() {
        rbController.useGravity = false;
        rbController.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);
    }

    private void StopWallRun() {
        rbController.useGravity = true;

        if(Input.GetKeyDown(jumpKey)) {
            ////
        }//////////////////////////////////
    }

    private void CheckWall(){
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftRayHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightRayHit, wallDistance);
    }

    private bool CanWallRun(){
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }
}
