using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public PlayerInput playerInput;
    public Transform cameraTransform;

    private Vector3 movementVector;
    private Vector3 gravityVector;
    private Vector2 cameraVector;

    public float walkSpeed = 1.3f; // m/s
    public float runSpeed = 4.0f; //m/s
    public float gravity = -9.8f;
    public int rotateSensibility = 100;
    public int cameraLimitAngleX = 50;

    private float isRunning = 0.0f; // 1 = true, he is running, and 0 = false, he isn't running

    public void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    public void Update()
    {
        Inputs();
        Movement();
        Camera();
        Gravity();
    }

    public void Inputs()
    {
        movementVector = playerInput.actions.FindAction("Movement").ReadValue<Vector3>();
        cameraVector = playerInput.actions.FindAction("Camera").ReadValue<Vector2>();
        isRunning = playerInput.actions.FindAction("Run").ReadValue<float>();
    }

    public void Movement()
    {
        if (isRunning == 0)
        {
            characterController.Move(transform.forward * movementVector.z * walkSpeed * Time.deltaTime);
            characterController.Move(transform.right * movementVector.x * walkSpeed * Time.deltaTime);
        }
        else 
        {
            characterController.Move(transform.forward * movementVector.z * runSpeed * Time.deltaTime);
            characterController.Move(transform.right * movementVector.x * runSpeed * Time.deltaTime);
        } 
    }

    public void Camera()
    {
        float rotateY = cameraVector.x * rotateSensibility * Time.deltaTime;
        float rotateX = cameraVector.y * rotateSensibility * Time.deltaTime;
        float cameraAngleX = cameraTransform.localRotation.x * 100;

        transform.Rotate(0, rotateY, 0);

        if ((cameraAngleX > -cameraLimitAngleX || rotateX < 0) && (cameraAngleX < cameraLimitAngleX || rotateX > 0))
        {
            cameraTransform.Rotate(rotateX * (-1), 0, 0);
        }
    }

    public void Gravity()
    {
        if (!characterController.isGrounded)
        {
            gravityVector.y += gravity * Time.deltaTime;
            characterController.Move(gravityVector);
        }
        else
        {
            gravityVector.y = 0;
        }
    }
}