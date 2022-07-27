using UnityEngine;

[System.Serializable]
public class CoronationDialogue
{
    public string name;
    [TextArea (2,10)]
    public string phrase;
    public int font;
    public bool quirky;
    public bool center;
}
