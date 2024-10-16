using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform playerCamera;

    [SerializeField] private float sensitivity = 10;

    [Header("Movement")]
    [SerializeField] private CharacterController controller;

    [SerializeField] private float speed = 5;
    [SerializeField] private float mass = 1;


    private PlayerInputActions playerInputActions;
    private InputAction lookAction;
    private InputAction moveAction;

    private Vector2 look;
    private Vector3 velocity;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        lookAction = playerInputActions.Player.Look;
        lookAction.Enable();
        moveAction = playerInputActions.Player.Move;
        moveAction.Enable();
    }

    private void Update()
    {
        UpdateLook();
        UpdateMovement();
        UpdateGravity();
    }

    private void UpdateLook()
    {
        look += lookAction.ReadValue<Vector2>() * (sensitivity * Time.deltaTime);
        look.y = Mathf.Clamp(look.y, -90, 90);

        transform.localRotation = Quaternion.Euler(0, look.x, 0);
        playerCamera.localRotation = Quaternion.Euler(-look.y, 0, 0);
    }

    private void UpdateMovement()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();

        Vector3 input = new();
        input += transform.right * move.x;
        input += transform.forward * move.y;
        input = Vector3.ClampMagnitude(input, 1);

        controller.Move((input * speed + velocity) * Time.deltaTime);
    }

    private void UpdateGravity()
    {
        Vector3 gravity = Physics.gravity * (Time.deltaTime * mass);
        velocity.y = controller.isGrounded ? -1f : velocity.y + gravity.y;
    }
}