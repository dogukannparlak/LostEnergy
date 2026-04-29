using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

    [Header("Animasyon (AnimationPlayer)")]
    [Tooltip("Karakterdeki AnimationPlayer bileşeni (boş bırakılırsa otomatik bulunur).")]
    public AnimationPlayer animationPlayer;
    [Tooltip("Idle animasyonunun listeki indeksi")]
    public int idleIndex   = 0;
    [Tooltip("Yürüme animasyonunun listeki indeksi")]
    public int walkIndex   = 1;
    [Tooltip("Koşma animasyonunun listeki indeksi")]
    public int runIndex    = 2;
    [Tooltip("Zıplama animasyonunun listeki indeksi")]
    public int jumpIndex   = 3;

    private CharacterController _cc;
    private float               _verticalVelocity;
    private float               _yaw;
    private float               _pitch;
    private bool                _isDead;
    private bool                _wasGrounded;
    private int                 _currentAnimIndex = -1;

#if UNITY_EDITOR
    // Component ilk eklendiğinde CharacterController'ı otomatik ayarla
    private void Reset()
    {
        var cc = GetComponent<CharacterController>();
        if (cc == null) return;
        cc.height = 1.8f;
        cc.center = new Vector3(0f, 0.93f, 0f);
        cc.radius = 0.3f;
        cc.stepOffset = 0.3f;
        cc.skinWidth  = 0.08f;
    }
#endif

    void Start()
    {
        _cc = GetComponent<CharacterController>();

        if (animationPlayer == null)
            animationPlayer = GetComponent<AnimationPlayer>();

        if (cameraTransform == null)
            Debug.LogError("[PlayerController3P] 'cameraTransform' is not assigned!", this);

        if (cameraPivot == null)
            Debug.LogWarning("[PlayerController3P] 'cameraPivot' is not assigned.", this);

        LockCursor();

        if (cameraTransform != null)
        {
            _yaw   = cameraTransform.eulerAngles.y;
            _pitch = cameraTransform.eulerAngles.x;
        }

        _wasGrounded = IsGrounded();
        PlayAnim(idleIndex);
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
        UpdateAnimation();
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

    void UpdateAnimation()
    {
        if (animationPlayer == null) return;

        bool grounded = IsGrounded();

        // Zıplama başladı
        if (!grounded && _wasGrounded)
        {
            PlayAnim(jumpIndex);
            _wasGrounded = false;
            return;
        }

        // Yere indi
        if (grounded && !_wasGrounded)
            _wasGrounded = true;

        if (!grounded) return;

        // Yatay hareket hızı
        Vector3 horizontal = new Vector3(_cc.velocity.x, 0f, _cc.velocity.z);
        float speed = horizontal.magnitude;

        int targetIndex;
        if (speed < 0.1f)
            targetIndex = idleIndex;
        else if (Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed)
            targetIndex = runIndex;
        else
            targetIndex = walkIndex;

        PlayAnim(targetIndex);
    }

    void PlayAnim(int index)
    {
        if (animationPlayer == null) return;
        if (_currentAnimIndex == index) return;
        _currentAnimIndex = index;
        animationPlayer.PlayByIndex(index);
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

#if UNITY_EDITOR
    /// <summary>
    /// CharacterController'ı karakterin mesh sınırlarına göre otomatik ayarlar.
    /// Inspector'daki sağ tık menüsünden çağır.
    /// </summary>
    [ContextMenu("CharacterController'ı Otomatik Ayarla (Gömülme Düzelt)")]
    private void AutoFitCharacterController()
    {
        var cc = GetComponent<CharacterController>();
        if (cc == null) return;

        // Tüm SkinnedMeshRenderer'ları topla
        var renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        if (renderers.Length == 0)
        {
            var meshRenderers = GetComponentsInChildren<MeshRenderer>();
            if (meshRenderers.Length == 0) { Debug.LogWarning("Mesh bulunamadı."); return; }
        }

        Bounds bounds = new Bounds(transform.position, Vector3.zero);
        foreach (var r in GetComponentsInChildren<Renderer>())
            bounds.Encapsulate(r.bounds);

        // Lokal koordinata çevir
        float height = bounds.size.y;
        float centerY = bounds.center.y - transform.position.y;
        float radius = Mathf.Max(bounds.size.x, bounds.size.z) * 0.25f;

        Undo.RecordObject(cc, "AutoFit CharacterController");
        cc.height = height;
        cc.center = new Vector3(0f, centerY, 0f);
        cc.radius = Mathf.Min(radius, height * 0.4f);

        EditorUtility.SetDirty(cc);
        Debug.Log($"[PlayerController3P] CharacterController ayarlandı → Height:{height:F2}  CenterY:{centerY:F2}  Radius:{cc.radius:F2}");
    }
#endif
}
