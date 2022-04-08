using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;


public class InputAgent : Agent
{

    [SerializeField]
    private Transform ball, goalSelf, goalEnemy;
    [SerializeField]
    private Transform enemyTransform;
    [SerializeField]
    private Vector3 goalScale;
    [SerializeField]
    private MovementController enemyController;
    [SerializeField]
    private MovementController controller;
    [SerializeField]
    private CameraController cameraController;
    private float Debugaction1 = 0;
    public void OnEnable()
    {
        LazyInitialize();
    }
    public override void OnEpisodeBegin()
    {
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        enemyTransform = enemyController.transform;
        sensor.AddObservation(transform.localPosition); //3
        sensor.AddObservation(transform.rotation.eulerAngles); //6
        sensor.AddObservation(ball.localPosition); //9
        sensor.AddObservation((goalSelf.position - transform.position).normalized); //12
        sensor.AddObservation(goalSelf.rotation.eulerAngles); //15
        sensor.AddObservation((goalEnemy.position - transform.position).normalized); //18
        sensor.AddObservation(goalEnemy.rotation.eulerAngles); //21
        sensor.AddObservation(goalScale); //24
        sensor.AddObservation(enemyTransform.localPosition); //27
        sensor.AddObservation(enemyTransform.rotation.eulerAngles); //30
        sensor.AddObservation(enemyController.GetPowerShotEnabled()); //31
        sensor.AddObservation(controller.GetPowerShotEnabled()); //32
        sensor.AddObservation(goalEnemy.localPosition); //35
        sensor.AddObservation(goalSelf.localPosition); //38
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float x = actions.ContinuousActions[0];
        float y = actions.ContinuousActions[1];
        Debugaction1 = x;
        int xMove = actions.DiscreteActions[0] - 1;
        int zMove = actions.DiscreteActions[1] - 1;
        Vector3 moveDir = new Vector3();
        cameraController.ManuallyMoveCamera(new Vector2(x, y));
        Vector3 lookDir = cameraController.transform.forward;
        lookDir = Vector3.ProjectOnPlane(lookDir, Vector3.up).normalized;
        moveDir = (xMove * cameraController.transform.right + zMove * cameraController.transform.forward).normalized;
        moveDir = Vector3.ProjectOnPlane(moveDir, Vector3.up).normalized;
        controller.SetLookDir(lookDir);
        controller.SetMoveDir(moveDir);
        if (actions.DiscreteActions[2] == 1)
        {
            controller.Jump();
        }
        controller.Shoot(actions.DiscreteActions[3] == 1);
        if (actions.DiscreteActions[4] == 1)
        {
            controller.UsePowerShot();
        }
        if (actions.DiscreteActions[5] == 1)
        {
            controller.Run();
        }
        SetReward(-0.01f);
    }
    
}
