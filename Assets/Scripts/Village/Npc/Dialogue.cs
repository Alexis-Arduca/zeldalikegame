using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea] public string text;
    public List<DialogueChoice> choices;
    public bool isEndOfBranch;
    public string tags;
}

[System.Serializable]
public class DialogueChoice
{
    public string text;
    public int nextLineIndex;
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public string npcName;
    public List<DialogueLine> lines;
    public float typingSpeed = 0.05f;
}