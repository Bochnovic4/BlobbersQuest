using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float sprintSpeedMultiplier = 1.5f;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float rotationSpeed = 2f;

    private Rigidbody rb;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private bool jumpInput;
    private bool isGrounded;
    private bool isMovingForward;
    private bool isMovingBack;
    private bool isMovingRight;
    private bool isMovingLeft;
    private bool isSprinting;
    private bool isBlocking;

    private Animator playerAnimator;
    private float verticalLookRotation = 0f;
    public float maxVerticalLookAngle = 60f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!cameraTransform) cameraTransform = Camera.main.transform;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        //rb.centerOfMass = new Vector3(0, -1, 0);
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleCameraRotation();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        HandleJump();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        isMovingForward = movementInput.y > 0;
        isMovingBack = movementInput.y < 0;
        isMovingRight = movementInput.x > 0;
        isMovingLeft = movementInput.x < 0;
        Debug.Log("move input: " + movementInput.ToString());
        playerAnimator.SetBool("isMovingForward", isMovingForward);
        playerAnimator.SetBool("isMovingBack", isMovingBack);
        playerAnimator.SetBool("isMovingRight", isMovingRight);
        playerAnimator.SetBool("isMovingLeft", isMovingLeft);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            jumpInput = true;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = true;
            playerAnimator.SetBool("isSprinting", isSprinting);
        }
        else if (context.canceled)
        {
            isSprinting = false;
            playerAnimator.SetBool("isSprinting", isSprinting);
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isBlocking = true;
            playerAnimator.SetBool("isBlocking", isBlocking);
        }
        if (context.canceled)
        {
            isBlocking = false;
            playerAnimator.SetBool("isBlocking", isBlocking);
        }
    }

    private void ApplyMovement()
    {
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y);
        moveDirection = cameraTransform.TransformDirection(moveDirection);
        moveDirection.y = 0;

        if (movementInput != Vector2.zero)
        {
            // Adjust speed if sprinting
            float currentSpeed = isSprinting ? moveSpeed * sprintSpeedMultiplier : moveSpeed;
            Vector3 movement = moveDirection.normalized * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }

    private void HandleJump()
    {
        if (jumpInput)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpInput = false;
            isGrounded = false;
        }
    }

    private void HandleCameraRotation()
    {
        // Rotate player horizontally based on look input
        float horizontalRotation = lookInput.x * rotationSpeed;
        transform.Rotate(0, horizontalRotation, 0);

        // Rotate camera for vertical look, with clamping to prevent flipping
        verticalLookRotation -= lookInput.y * rotationSpeed;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -maxVerticalLookAngle, maxVerticalLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Simple ground detection
        if (collision.contacts[0].point.y <= transform.position.y)
        {
            isGrounded = true;
        }
    }
}