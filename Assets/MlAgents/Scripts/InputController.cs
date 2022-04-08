using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private MovementController movementController;
    public Transform camera;
    private void Start()
    {
        movementController = GetComponent<MovementController>();
    }
    private void Update()
    {
        if (movementController == null || camera == null)
        {
            Console.WriteLine("Camera or movement controller not found!");
            return;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3();
        Vector3 lookDir = camera.forward;
        lookDir = Vector3.ProjectOnPlane(lookDir, Vector3.up).normalized;
        moveDir = (x * camera.right + z * camera.forward).normalized;
        moveDir = Vector3.ProjectOnPlane(moveDir, Vector3.up).normalized;
        movementController.SetMoveDir(moveDir);
        movementController.SetLookDir(lookDir);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            movementController.Jump();
        }
        movementController.Shoot(Input.GetKey(KeyCode.LeftControl));
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementController.Run();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            movementController.UsePowerShot();
        }
    }
}
