using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    /// <summary>
    /// Name of the speaker for this line.
    /// </summary>
    public string speakerName;

    [TextArea(2, 4)]

    /// <summary>
    /// Dialogue text shown for this line.
    /// </summary>
    public string text;
}

[CreateAssetMenu(menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    /// <summary>
    /// Ordered list of dialogue lines.
    /// </summary>
    public DialogueLine[] lines;
}
