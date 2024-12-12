using System.Collections;
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

    private Player playerStats;
    private Rigidbody rb;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private bool jumpInput;
    public bool isGrounded;
    private bool isMovingForward;
    private bool isMovingBack;
    private bool isMovingRight;
    private bool isMovingLeft;
    private bool isSprinting;
    public bool isBlocking;
    public bool isPlayingAttackAnimation;

    private Animator playerAnimator;
    private float verticalLookRotation = 0f;
    private float maxVerticalLookAngle = 60f;

    private float sprintTimer = 0f;
    private float sprintStaminaDrainRate = 10f;
    private float attackStaminaDrainRate = 20f;
    private float jumpStaminaDrainRate = 20f;
    private float dodgeStaminaDrainRate = 10f;
    public bool isCameraLocked;

    [SerializeField]
    private PlayerSword playerSword;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!cameraTransform) cameraTransform = Camera.main.transform;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        playerAnimator = GetComponent<Animator>();
        playerStats = GetComponent<Player>();
    }

    private void Update()
    {
        HandleCameraRotation();
        CheckAttackAnimationState();
        if (isSprinting)
        {
            sprintTimer += Time.deltaTime;
            if (sprintTimer >= 1f)
            {
                playerStats.UseStamina(sprintStaminaDrainRate);
                sprintTimer = 0f;
            }
            if (playerStats.currentStamina < sprintStaminaDrainRate)
            {
                isSprinting = false;
                playerAnimator.SetBool("isSprinting", false);
            }
        }
        else
        {
            sprintTimer = 0f;
            playerStats.StopUsingStamina();
        }

    }

    private void FixedUpdate()
    {
        ApplyMovement();
        HandleJump();
        CheckGroundStatus();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) return;
        if (isBlocking) return;
        movementInput = context.ReadValue<Vector2>();
        isMovingForward = movementInput.y > 0;
        isMovingBack = movementInput.y < 0;
        isMovingRight = movementInput.x > 0;
        isMovingLeft = movementInput.x < 0;

        playerAnimator.SetBool("isMovingForward", isMovingForward);
        playerAnimator.SetBool("isMovingBack", isMovingBack);
        playerAnimator.SetBool("isMovingRight", isMovingRight);
        playerAnimator.SetBool("isMovingLeft", isMovingLeft);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && playerStats.currentStamina > 0)
        {
            PerformJump();
        }
    }

    private void PerformJump()
    {
        if (isBlocking) return;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        playerStats.UseStamina(jumpStaminaDrainRate);
        isGrounded = false;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed && playerStats.currentStamina > 0)
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
        if (context.started)
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

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && !isPlayingAttackAnimation & playerStats.currentStamina > 0)
        {
            playerStats.UseStamina(attackStaminaDrainRate);
            isPlayingAttackAnimation = true;
            playerAnimator.SetTrigger("Attack");
            playerSword.EnableCollider();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.started && playerStats.currentStamina > 0)
        {
            playerStats.UseStamina(dodgeStaminaDrainRate);
            playerAnimator.SetTrigger("Dodge");
            PerformDodge();
        }
        else if (context.canceled)
        {
            playerStats.StopUsingStamina();
        }
    }
    private void PerformDodge()
    {
        Vector3 backwardDirection = -transform.forward; 

        float dodgeSpeed = 10f;
        Vector3 dodgeVelocity = backwardDirection * dodgeSpeed;

        StartCoroutine(ApplyDodgeForce(dodgeVelocity, .1f));
    }

    private IEnumerator ApplyDodgeForce(Vector3 velocity, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position += velocity * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
        ResetMovement();
    }

    private void ApplyMovement()
    {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) return;
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y);
        moveDirection = cameraTransform.TransformDirection(moveDirection);
        moveDirection.y = 0;

        if (movementInput != Vector2.zero)
        {
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
        if (isCameraLocked || GameManager.Instance.IsMenuActive) return;

        float horizontalRotation = lookInput.x * rotationSpeed;
        transform.Rotate(0, horizontalRotation, 0);

        verticalLookRotation -= lookInput.y * rotationSpeed;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -maxVerticalLookAngle, maxVerticalLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0, 0);
    }


    private void CheckAttackAnimationState()
    {
        var animatorState = playerAnimator.GetCurrentAnimatorStateInfo(0);
        if (animatorState.IsTag("Attack"))
        {
            isPlayingAttackAnimation = true;
            isCameraLocked = true;
            playerSword.EnableCollider();
        }
        else
        {
            isPlayingAttackAnimation = false;
            isCameraLocked = false;
            playerStats.StopUsingStamina();
            playerSword.DisableCollider();
        }
    }

    private void CheckGroundStatus()
    {
        float rayLength = 0.2f;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;

        isGrounded = Physics.Raycast(rayOrigin, Vector3.down, rayLength);

        if (isGrounded) playerStats.StopUsingStamina();

        Color rayColor = isGrounded ? Color.green : Color.red;
        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, rayColor);
    }
    private void ResetMovement()
    {
        rb.linearVelocity = Vector3.zero;
    }
}