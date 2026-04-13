using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController3P : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed   = 5f;
    public float sprintSpeed = 9f;
    public float rotationSpeed = 720f;

    [Header("Jump & Gravity")]
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.15f;

    [Header("Camera Orbit")]
    public Transform cameraTransform;
    public Transform cameraPivot;
    public float mouseSensitivity = 0.2f;
    public float cameraDistance = 5f;
    public float pitchMin = -30f;
    public float pitchMax =  60f;

    private CharacterController _cc;
    private float               _verticalVelocity;
    private float               _yaw;
    private float               _pitch;
    private bool                _isDead;

    void Start()
    {
        _cc       = GetComponent<CharacterController>();

        if (cameraTransform == null)
            Debug.LogError("[PlayerController3P] 'cameraTransform' atanmamış!", this);

        if (cameraPivot == null)
            Debug.LogWarning("[PlayerController3P] 'cameraPivot' atanmamış.", this);

        LockCursor();

        if (cameraTransform != null)
        {
            _yaw   = cameraTransform.eulerAngles.y;
            _pitch = cameraTransform.eulerAngles.x;
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        if (transform.position.y >= -1f)
        {
            _isDead = false;
        }
        else if (!_isDead)
        {
            _isDead = true;
            LostEnergy.GameManager.Instance?.PlayerDied("Düştün Kör Musun Aq");
            return;
        }

        if (_isDead) return;

        if (DialogueManager.Instance != null && DialogueManager.Instance.IsActive) return;

        HandleCursorToggle();
        HandleCameraOrbit();
        HandleMovement();
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
    }

    void HandleCursorToggle()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible   = true;
        }

        if (Mouse.current != null
            && Mouse.current.leftButton.wasPressedThisFrame
            && Cursor.lockState == CursorLockMode.None)
        {
            LockCursor();
        }
    }

    void HandleCameraOrbit()
    {
        if (cameraTransform == null)           return;
        if (Cursor.lockState != CursorLockMode.Locked) return;
        if (Mouse.current == null)             return;

        Vector2 delta = Mouse.current.delta.ReadValue();

        _yaw   += delta.x * mouseSensitivity;
        _pitch -= delta.y * mouseSensitivity;
        _pitch  = Mathf.Clamp(_pitch, pitchMin, pitchMax);

        Transform pivot = cameraPivot != null ? cameraPivot : transform;

        Quaternion rotation         = Quaternion.Euler(_pitch, _yaw, 0f);
        Vector3    desiredPosition  = pivot.position - rotation * Vector3.forward * cameraDistance;

        cameraTransform.SetPositionAndRotation(desiredPosition, rotation);
    }

    void HandleMovement()
    {
        if (Keyboard.current == null) return;

        bool grounded = IsGrounded();

        if (grounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f;

        if (grounded && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        _verticalVelocity += gravity * Time.deltaTime;

        float h =
            (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1f : 0f) -
            (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed  ? 1f : 0f);

        float v =
            (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed   ? 1f : 0f) -
            (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed ? 1f : 0f);

        Vector3 camForward = cameraTransform != null
            ? Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized
            : transform.forward;

        Vector3 camRight = cameraTransform != null
            ? Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized
            : transform.right;

        Vector3 moveDir = camForward * v + camRight * h;
        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        float speed = Keyboard.current.leftShiftKey.isPressed ? sprintSpeed : walkSpeed;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation   = Quaternion.RotateTowards(
                transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        _cc.Move((moveDir * speed + Vector3.up * _verticalVelocity) * Time.deltaTime);
    }

    bool IsGrounded()
    {
        if (_cc.isGrounded) return true;

        float   radius = _cc.radius * 0.9f;
        Vector3 origin = transform.position + Vector3.up * (radius + 0.01f);

        return Physics.SphereCast(
            origin, radius, Vector3.down, out _,
            groundCheckDistance + 0.01f,
            ~LayerMask.GetMask("Player"),
            QueryTriggerInteraction.Ignore);
    }
}
