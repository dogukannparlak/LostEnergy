using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    public DialogueData dialogue;

    public string GetPrompt() => "Konuş";

    public void Interact(PlayerInteraction interactor)
    {
        if (dialogue == null) return;
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
