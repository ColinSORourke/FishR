using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "tutStep", menuName = "ScriptableObjects/tutStep", order = 1)]
public class TutorialStep : ScriptableObject
{
    //TextAreaAttribute(int minLines, int maxLines);
    [TextArea(15,20)]
    public string description;
    public int pose;
}
