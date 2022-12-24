using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;

[System.Serializable] // Crea diferentes ejes. Lo normal es que sean dos pero puede darse el caso de que hayan más
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;

    [Tooltip("Does This Axle push the car? True or False?")]
    public bool isDrivingWheels;

    [Tooltip("Does This Axle brake the car? True or False?")]
    public bool isBrakeWheels = true; // For Default is True

    [Tooltip("Does this wheel apply steer angle? True or False?")]
    public bool isSteeringWheels;
}
public class CarController : MonoBehaviour
{
    public PlayerInput playerInput;

    private Rigidbody carRigidbody;

    public List<AxleInfo> axleInfos; // the information about each individual axle

    public float maxWheelTorque; // maximum torque the motor can apply to wheel
    public float maxBrakeTorque; // maximum torque the brake can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have

    private float wheelTorque = 0;
    private float brakeTorque = 0;
    private float steeringAngle = 0;
    private float rpm = 0;
    private float speed = 0; // m/s

    private float fordward;
    private float backward;

    public void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        carRigidbody = GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = carRigidbody.centerOfMass + new Vector3(0, -1, 0);
    }

    public void Update()
    {
        speed = (float) Math.Round(carRigidbody.velocity.magnitude, 2);

        Inputs();
        PutTorque();
        UpdateAxis();

        rpm = axleInfos[0].leftWheel.rpm;

        //Debug.Log("fordward: " + fordward + " backward: " +  backward + " wheelTorque: " +  wheelTorque + " brakeTorque: " + brakeTorque + " steeringAngle: " + steeringAngle + " rpm: " + rpm + " m/s: " + speed + " Km/h: " + (speed * 3.6));
    }
    
    public void Inputs()
    {
        fordward = playerInput.actions.FindAction("Forward").ReadValue<float>();
        backward = playerInput.actions.FindAction("Backward").ReadValue<float>();
        steeringAngle = maxSteeringAngle * playerInput.actions.FindAction("Turn").ReadValue<float>();
    }

    public void PutTorque()
    {
        if (speed == 0 && fordward > 0)
        {
            wheelTorque = maxWheelTorque * fordward;
            brakeTorque = 0;
        }
        if (speed == 0 && backward > 0)
        {
            wheelTorque = maxWheelTorque * backward * (-1);
            brakeTorque = 0;
        }
        if (rpm > 0)
        {
            wheelTorque = maxWheelTorque * fordward;
            brakeTorque = maxBrakeTorque * backward;
        }
        else if (rpm < 0)
        {
            wheelTorque = maxWheelTorque * backward * (-1);
            brakeTorque = maxBrakeTorque * fordward;
        }
    }
    
    public void UpdateAxis()
    {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.isSteeringWheels)
            {
                axleInfo.leftWheel.steerAngle = steeringAngle;
                axleInfo.rightWheel.steerAngle = steeringAngle;
            }
            if (axleInfo.isDrivingWheels)
            {
                axleInfo.leftWheel.motorTorque = wheelTorque;
                axleInfo.rightWheel.motorTorque = wheelTorque;
            }
            if (axleInfo.isBrakeWheels)
            {
                axleInfo.leftWheel.brakeTorque = brakeTorque;
                axleInfo.rightWheel.brakeTorque = brakeTorque;
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    // finds the corresponding visual wheel and correctly applies the transform.
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
}
