using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Detection")]
    public float detectRadius = 0.5f;
    public LayerMask interactMask = ~0;

    [Header("UI")]
    public TMP_Text promptText;

    [Header("Input")]
    public float interactInputBuffer = 0.2f;

    private IInteractable _currentTarget;
    private float _lastInteractPressedTime = -999f;
    private bool _wasDialogueActive;

    void Start() => SetPrompt(null);

    void Update()
    {
        DetectInteractable();
        HandleInteractInput();
    }

    void DetectInteractable()
    {
        IInteractable found = null;
        Collider[] nearby = Physics.OverlapSphere(transform.position, detectRadius, interactMask, QueryTriggerInteraction.Collide);
        foreach (var col in nearby)
        {
            IInteractable ia = col.GetComponentInParent<IInteractable>();
            if (ia != null) { found = ia; break; }
        }

        if (found != _currentTarget)
        {
            _currentTarget = found;
            SetPrompt(_currentTarget);
        }
    }

    void HandleInteractInput()
    {
        if (Keyboard.current == null) return;

        bool dialogueActive = DialogueManager.Instance != null && DialogueManager.Instance.IsActive;

        // Skip interaction while dialogue is active or just closed
        if (dialogueActive || _wasDialogueActive)
        {
            _wasDialogueActive = dialogueActive;
            return;
        }
        _wasDialogueActive = false;

        if (Keyboard.current.eKey.wasPressedThisFrame)
            _lastInteractPressedTime = Time.time;

        if (_currentTarget == null) return;

        if (Time.time - _lastInteractPressedTime <= interactInputBuffer)
        {
            string targetName = (_currentTarget as UnityEngine.Object)?.name ?? _currentTarget.GetType().Name;
            _currentTarget.Interact(this);
            LostEnergy.GameLogger.Instance?.LogEvent("INTERACT", targetName);
            _lastInteractPressedTime = -999f;
        }
    }

    void SetPrompt(IInteractable target)
    {
        if (promptText == null) return;
        promptText.text    = target != null ? $"E: {target.GetPrompt()}" : string.Empty;
        promptText.enabled = target != null;
    }
}
