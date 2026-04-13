using UnityEngine;

public class OxygenPotion : MonoBehaviour, IInteractable
{
    [Header("Restore")]
    [Range(0f, 1f)]
    public float restorePercent = 0.8f;   // 80% O2

    [Header("Prompt")]
    public string prompt = "İksiri İç (+O2)";

    [Header("Bob Animation")]
    public float bobSpeed      = 2f;
    public float bobAmplitude  = 0.15f;

    private bool    _used;
    private Vector3 _startPos;

    void Start()  => _startPos = transform.position;

    void Update()
    {
        if (_used) return;
        float y = _startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
        transform.position = new Vector3(_startPos.x, y, _startPos.z);
    }

    public string GetPrompt() => prompt;

    public void Interact(PlayerInteraction interactor)
    {
        if (_used) return;
        _used = true;

        var oxy = interactor.GetComponentInParent<OxygenSystem>()
               ?? interactor.GetComponent<OxygenSystem>();

        if (oxy != null)
            oxy.RestoreToPercent(restorePercent);

        Destroy(gameObject);
    }
}
